using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class SaveLoadSystem
{
    private static string DirPath = Application.persistentDataPath+"/data/";
    private static string CachePath = Application.temporaryCachePath+"/";
    public static string path => DirPath;

    public static void SaveFile(string text, string name, string format, string path = "", bool useDir = true)
    {
        if (!Directory.Exists($"{((useDir) ? DirPath : null)}{path}"))
            Directory.CreateDirectory($"{((useDir) ? DirPath : null)}{path}");

        File.WriteAllText($"{((useDir) ? DirPath : null)}{path}{name}.{format}", text);
    }
    
    public static void SaveFile(byte[] text, string name, string format, string path = "", bool useDir = true)
    {
        if (!Directory.Exists($"{((useDir) ? DirPath : null)}{path}"))
            Directory.CreateDirectory($"{((useDir) ? DirPath : null)}{path}");

        File.WriteAllBytes($"{((useDir) ? DirPath : null)}{path}{name}.{format}", text);
    }
    
    public static void LoadFile(string name, out string json, string path = "", bool useDir = true)
    {
        string pathToLoad = $"{((useDir) ? DirPath : null)}{path}{name}";
        
        if (!File.Exists(pathToLoad))
        {
            json = null;
            return;
        }

        json = File.ReadAllText(pathToLoad);
    }
    
    public static void LoadFile(string path, out string json)
    {
        if (!File.Exists(path))
        {
            json = null;
            return;
        }

        json = File.ReadAllText(path);
    }

    public static Sprite LoadImage(string name, string path = "", bool useDir = true)
    {
        string pathToLoad = $"{((useDir) ? DirPath : null)}{path}{name}";
        
        if (!File.Exists(pathToLoad)) return null;

        byte[] bytes = File.ReadAllBytes(pathToLoad);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    public static async Task<AudioClip> LoadAudio(string name, string path = "", bool useDir = true)
    {
        string pathToLoad = $"{((useDir) ? DirPath : null)}{path}{name}";
        
        if (!File.Exists(pathToLoad)) return null;
        
        var clip = await AJAX.GetAudio(path);
 
        return clip;
    }

    public static void DeleteFIle(string name, string format, string path = "")
    {
        if (File.Exists($"{DirPath}{path}{name}.{format}"))
            File.Delete($"{DirPath}{path}{name}.{format}");
    }
    
    public static string OpenFileExplorer(string title = "", string[] fileExtensions = null, string directory = "")
    {
        string path = null;
        
        for (int i = 0; i < fileExtensions.Length; i++)
        {
            fileExtensions[i] = NativeFilePicker.ConvertExtensionToFileType(fileExtensions[i]);
        }
        NativeFilePicker.PickFile((p) => path = p, fileExtensions);
        
#if (PLATFORM_ANDROID || PLATFORM_IOS) && !UNITY_EDITOR
        path = Path.Combine(CachePath, path);
#endif
        
        return path;
    }
    
    public static string OpenFolderExplorer(string title = "", string[] fileExtensions = null, string directory = "", string defaultName = "")
    {
        string path = null;

#if (!PLATFORM_ANDROID && !PLATFORM_IOS) || UNITY_EDITOR
        path = EditorUtility.OpenFolderPanel($"S{title}", directory, defaultName);
#else
        NativeFilePicker.RequestPermission();
        for (int i = 0; i < fileExtensions.Length; i++)
        {
            fileExtensions[i] = NativeFilePicker.ConvertExtensionToFileType(fileExtensions[i]);
        }
        NativeFilePicker.PickFile((p) => path = p, fileExtensions);
#endif
        
        return path;
    }
}
