using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameInfo
{
    public static PlayerSheetData PlayerSheetData;
    
    public static void LoadPlayerSheetData(string json)
    {
        if(PlayerSheetData == null) PlayerSheetData = ScriptableObject.CreateInstance<PlayerSheetData>();
        
        PlayerSheetData.SetData(json);
        ColorPaletteManager.SetPallete(PlayerSheetData.playerColors);
    }

    public static void LoadScene(string scene = "PlayerFichaNew")
    {
        SceneManager.LoadScene(scene);
    }
}
