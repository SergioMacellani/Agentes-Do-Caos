using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadSystem
{
    private static string DirPath = Application.persistentDataPath+"/data/";

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
    
    public static void LoadFile(string name, out string json, string path = "")
    {
        if (!File.Exists($"{DirPath}{path}{name}"))
        {
            json = null;
            return;
        }

        json = File.ReadAllText($"{DirPath}{path}{name}");
    }
    
    public static Sprite LoadImage(string name, string path = "")
    {
        if (!File.Exists($"{DirPath}{path}{name}")) return null;

        byte[] bytes = File.ReadAllBytes($"{DirPath}{path}{name}");
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}
