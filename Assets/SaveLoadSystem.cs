using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadSystem
{
    private static string DirPath = Application.persistentDataPath+"/data/";

    public static void SaveFile(string text, string path, string format)
    {
        if (!Directory.Exists(DirPath))
            Directory.CreateDirectory(DirPath);

        File.WriteAllText(DirPath + $"{path}.{format}", text);
    }
}
