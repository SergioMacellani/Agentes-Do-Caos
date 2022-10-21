using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class SaveLoadSystem
{
    private static string DirPath = Application.persistentDataPath+"/data/";
    public static string path => DirPath;

    public static void SaveFile(string text, string name, string format, string path = "")
    {
        if (!Directory.Exists($"{DirPath}{path}"))
            Directory.CreateDirectory($"{DirPath}{path}");

        File.WriteAllText($"{DirPath}{path}{name}.{format}", text);
    }
    
    public static void SaveFile(byte[] text, string name, string format, string path = "")
    {
        if (!Directory.Exists($"{DirPath}{path}"))
            Directory.CreateDirectory($"{DirPath}{path}");

        File.WriteAllBytes($"{DirPath}{path}{name}.{format}", text);
    }
    
    public static void LoadFile(string name, out string json, string path = "", bool useDir = true)
    {
        if (!File.Exists($"{((useDir) ? DirPath : null)}{path}{name}"))
        {
            json = null;
            return;
        }

        json = File.ReadAllText($"{((useDir) ? DirPath : null)}{path}{name}");
    }

    public static Sprite LoadImage(string name, string path = "", bool useDir = true)
    {
        if (!File.Exists($"{((useDir) ? DirPath : null)}{path}{name}")) return null;

        byte[] bytes = File.ReadAllBytes($"{((useDir) ? DirPath : null)}{path}{name}");
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
    
    public static async Task<AudioClip> LoadAudio(string name, string path = "", bool useDir = true)
    {
        if (!File.Exists($"{((useDir) ? DirPath : null)}{path}{name}")) return null;

        byte[] bytes = File.ReadAllBytes($"{((useDir) ? DirPath : null)}{path}{name}");
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        
        var clip = await AJAX.GetAudio(path);
 
        return clip;
    }

    public static void DeleteFIle(string name, string format, string path = "")
    {
        if (File.Exists($"{DirPath}{path}{name}.{format}"))
            File.Delete($"{DirPath}{path}{name}.{format}");
    }
    
    public static string OpenFileExplorer(string title = "", string fileExtension = ".csv", string directory = "")
    {
#if (!PLATFORM_ANDROID && !PLATFORM_IOS) || UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel($"S{title} (.{fileExtension})", directory, fileExtension);

        if (path.Length != 0) return path;
        else return null;
#endif
        return null;
    }
    
    public static string OpenFolderExplorer(string title = "", string directory = "", string fileExtension = "")
    {
#if (!PLATFORM_ANDROID && !PLATFORM_IOS) || UNITY_EDITOR
        string path = EditorUtility.OpenFolderPanel($"S{title}", directory, fileExtension);

        if (path.Length != 0) return path;
        else return null;
#endif
        return null;
    }
}
