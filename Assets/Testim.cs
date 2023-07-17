using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Testim : MonoBehaviour
{
    public Image image;

    public void PickImage()
    {
        if (!NativeGallery.IsMediaPickerBusy())
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                if (path != null)
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(bytes);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    image.sprite = sprite;
                }
            }, "Select a PNG image", "image/png");
        }
    }

    public void PickImageLegacy()
    {
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.intent.action.PICK");
        intent.Call<AndroidJavaObject>("setType", "image/*");
        currentActivity.Call("startActivityForResult", intent, 1);
    }

    public void OnActivityResult(string data)
    {
        if (data != null)
        {
            byte[] bytes = File.ReadAllBytes(data);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
        }
    }
}