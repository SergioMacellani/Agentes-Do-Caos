using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConvertCSVData : MonoBehaviour
{
    public List<List<string>> csvArray = new List<List<string>>();
    public PlayerSheetData pSheet;
    public Vector3 arrayPos;

    public float csv2D(int x, int y)
    {
        try
        {
            string csvText = csvArray[x][y];

            if (csvText.Contains(" kg")) csvText = csvText.Replace(" kg", "");
                
            return float.Parse(csvText);
        }
        catch
        {
            return -999;
        }
    }
    
    public string csv2DString(int x, int y)
    {
        try
        {
            return csvArray[x][y];
        }
        catch
        {
            return "null";
        }
    }
    
    void Start()
    {
        ConvertPlayer();
        Debug.Log("AA");
    }

    public void ConvertPlayer()
    {
        PlayerName pName = new PlayerName();
        PlayerPotions pPotions = new PlayerPotions();
        PlayerTechniques pTechniques = new PlayerTechniques();
        PlayerMagics pMagics = new PlayerMagics();
        PlayerInv pInventory = new PlayerInv();
        PlayerText pText = new PlayerText();

        string csv = File.ReadAllText("Assets/Resources/Ficha Hanna teste2.csv");
        string[] arrayY = csv.Split('\n');

        csvArray.Clear();
        for (int y = 0; y < arrayY.Length; y++)
        {
            string[] arrayX = arrayY[y].Split(';');
            List<string> csvX = new List<string>();
            for (int x = 0; x < arrayX.Length; x++)
            {
                csvX.Add(arrayX[x]);
            }
            csvArray.Add(csvX);
        }
        CreateCSV();
        
        pSheet.playerName.SetName(csv2DString(0, 1));
        pSheet.SetEssentials(GetArray(2,5,9));
        pSheet.SetPotions(GetArray(11,17,22));
        pSheet.SetTechniques(TechiniquesGetValue());
        pSheet.SetMagics(MagicsGetValue());
        pSheet.inventory.SetValues(GetArrayString(2,22,26),GetArray(2,22,29), GetStatsF(csv2D(22,29), csv2D(23,29)));
        
        pText.Notes = csv2DString(20, 19);
        pText.Traumas = $"{csv2DString(31,19)} - {csv2DString(31,22)}";
        pSheet.texts = pText;
        
        //CreateCSV();
        Debug.Log(pSheet.potions.Potions.Count);
        SaveLoadSystem.SaveFile(JsonUtility.ToJson(pSheet, true),"psheeddt", "chaos");
    }

    private float[] TechiniquesGetValue()
    {
        List<float> TechniquesValues = new List<float>();
        TechniquesValues.AddRange(GetArray(17, 26, 3));
        TechniquesValues.AddRange(GetArray(17, 26, 9));
        TechniquesValues.AddRange(GetArray(28,34,3));
        TechniquesValues.AddRange(GetArray(28, 36, 9));
        
        return TechniquesValues.ToArray();
    }
    
    private float[] MagicsGetValue()
    {
        List<float> MagicsValues = new List<float>();
        MagicsValues.AddRange(GetArray(2, 5, 16));
        MagicsValues.AddRange(GetArray(9, 15, 16));
        MagicsValues.AddRange(GetArray(2, 6, 23));
        
        return MagicsValues.ToArray();
    }

    public void CreateCSV()
    {
        string json = "";
        foreach (var cs in csvArray)
        {
            foreach (var c in cs)
            {
                json += c + ";";
            }
        }

        SaveLoadSystem.SaveFile(json,"csvTest", "csv");
    }

    public float[] GetArray(int x1, int x2, int y, bool invertXY = false, bool debug = false)
    {
        List<float> array = new List<float>();
        
        for (int x = x1; x < x2; x++)
        {
            float csv = csv2D(x, y);
            if(csv != -999)
                array.Add(csv);
            if(debug) Debug.Log(csv2DString(x, y) + $" - [{x}, {y}]");
        }

        return array.ToArray();
    }
    
    public string[] GetArrayString(int x1, int x2, int y, bool invertXY = false)
    {
        List<string> array = new List<string>();
        
        for (int x = x1; x < x2; x++)
        {
            array.Add(csv2DString(x,y));
        }

        return array.ToArray();
    }

    public StatsValue GetStats(int c, int m)
    {
        StatsValue stats = new StatsValue();
        stats.SetValue(c, m);
        return stats;
    }
    
    public StatsValueFloat GetStatsF(float c, float m)
    {
        StatsValueFloat stats = new StatsValueFloat();
        stats.SetValue(c, m);
        return stats;
    }
    
    /*
    public static void SavePlayer(PlayerSheetData data)
    {
        if (!Directory.Exists(DirPath))
            Directory.CreateDirectory(DirPath);

        string json = "";

        json = JsonUtility.ToJson(data, true);
        
        File.WriteAllText(DirPath + "/pSheet.adc", json);
    }*/

}
