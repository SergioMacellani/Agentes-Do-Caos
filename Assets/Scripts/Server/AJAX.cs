using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public static class AJAX
{
    private const string URL = "https://sergiom.dev/rpg/data/";

    public static IEnumerator GetRequestAsync(string path, System.Action<string> callback = null, string AURL = "")
    {
        UnityWebRequest www = UnityWebRequest.Get((AURL == "" ? URL : AURL) + path);

        yield return www.SendWebRequest();

        if (www.result is UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.url);
            callback?.Invoke(null);
        }
        else
        {
            callback?.Invoke(www.downloadHandler.text);
        }
    }
    
    public static IEnumerator GetImage(string path, TextMeshProUGUI text, System.Action<Sprite> callback = null, string AURL = "")
    {
        UnityWebRequest www = UnityWebRequest.Get((AURL == "" ? URL : AURL) + path);
        www.downloadHandler = new DownloadHandlerTexture();
        var webRequest = www.SendWebRequest();
        
        while (!webRequest.webRequest.isDone)
        {
            text.text = www.downloadProgress.ToString("P0");
            yield return new WaitForSeconds(.2f);
        }

        if (www.result is UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
            callback?.Invoke(null);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            callback?.Invoke(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));
        }
    }

    private static IEnumerator PostRequestAsync(string path, string key, string value)
    {
        WWWForm form = new WWWForm();
        form.AddField(key, value);

        UnityWebRequest www = UnityWebRequest.Post(URL + path, form);

        yield return www.SendWebRequest();

        if (www.result is UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.url);
        }
        else
        {
            
        }
    }
}
