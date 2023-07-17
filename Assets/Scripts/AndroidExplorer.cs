#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class AndroidExplorer
{
    private static AndroidJavaObject _context;
    private static readonly int READ_REQUEST_CODE = 42;

    static AndroidExplorer()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }

    public static void OpenFileExplorer(string fileType = "*/*", Action<string> onPathSelected = null)
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_OPEN_DOCUMENT"));

        intentObject.Call<AndroidJavaObject>("setType", fileType);

        AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activityObject = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

        activityObject.Call("startActivityForResult", intentObject, READ_REQUEST_CODE);

        // Create a listener to handle the result of the file explorer activity
        AndroidJavaObject listener = new AndroidJavaObject("com.unity3d.player.UnityPlayer$OnActivityResultListener");
        listener.Call("registerSelf");
        AndroidJNI.PushLocalFrame(10);
        IntPtr methodId = AndroidJNIHelper.GetMethodID(listener.GetRawClass(), "onActivityResult", "(IILandroid/content/Intent;)V");
        AndroidJNI.CallObjectMethod(listener.GetRawObject(), methodId, AndroidJNIHelper.CreateJNIArgArray(new object[] { READ_REQUEST_CODE, -1, null }));
        AndroidJNI.PopLocalFrame(IntPtr.Zero);

        // Call the callback with the selected path when the file explorer activity returns
        AndroidJavaClass coroutineClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        coroutineClass.CallStatic("UnitySendMessage", "FileExplorerMessageReceiver", "OnFileSelected", "");
        coroutineClass.CallStatic("StartCoroutine", "OnPathSelected(" + onPathSelected.Method.Name + ")");
    }

    public static IEnumerator OnPathSelected(Action<string> onPathSelected)
    {
        while (true)
        {
            string path = PlayerPrefs.GetString("selected_file_path");
            if (!string.IsNullOrEmpty(path))
            {
                PlayerPrefs.DeleteKey("selected_file_path");
                onPathSelected.Invoke(path);
                yield break;
            }
            yield return null;
        }
    }
}
#endif
