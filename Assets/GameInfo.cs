using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo
{
    public static PlayerSheetData playerSheetData;
    
    public static void LoadPlayerSheetData(string json)
    {
        if(playerSheetData == null) playerSheetData = ScriptableObject.CreateInstance<PlayerSheetData>();
        
        playerSheetData = Resources.Load<PlayerSheetData>(json);
    }
}
