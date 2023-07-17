using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ConvertCSVData : MonoBehaviour
{
    private List<List<string>> csvArray = new List<List<string>>();

    private float csv2D(int x, int y)
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

    private string csv2DString(int x, int y)
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

    public void ConvertPlayer(ref PlayerSheetData pSheet, string path = "Assets/Resources/Ficha - Agentes do Caos II - Teste.csv")
    {
        Debug.Log(path);
        string csv = File.ReadAllText(path);
        string[] arrayY = csv.Split('\n');

        csvArray.Clear();
        foreach (var y in arrayY)
        {
            string[] arrayX = y.Split(';');
            List<string> csvX = arrayX.ToList();
            csvArray.Add(csvX);
        }
        
        //CreateCSV();
        
        pSheet.playerName.SetName(csv2DString(0, 1));
        pSheet.SetEssentials(GetArray(2,5,9));
        pSheet.SetPotions(GetArray(18,24,15, false, false));
        pSheet.SetSkills(GetArray(9,13,9));
        pSheet.SetTechniques(TechiniquesGetValue());
        pSheet.SetMagics(MagicsGetValue());
        Debug.Log("Inventario");
        pSheet.inventory.SetValues(GetArrayString(2,22,23),GetArray(2,22,26), GetStatsF(csv2D(22,26), csv2D(22,26)));
        Debug.Log("Notas");
        pSheet.SetNotes(GetArrayString(38,45,0, false, true), Get2DArrayString(28,34,12,15, true), Get2DArrayString(27,34,22,25, true));
    }

    private float[] TechiniquesGetValue()
    {
        List<float> TechniquesValues = new List<float>();
        Debug.Log("Tecnicas");
        Debug.Log("Psique");
        TechniquesValues.AddRange(GetArray(17, 24, 3, false, false));
        Debug.Log("Combate");
        TechniquesValues.AddRange(GetArray(17, 24, 9, false, false));
        Debug.Log("Psique");
        TechniquesValues.AddRange(GetArray(28,34,3, false, false));
        Debug.Log("Sentidos");
        TechniquesValues.AddRange(GetArray(28, 32, 9, false, false));
        
        return TechniquesValues.ToArray();
    }
    
    private float[] MagicsGetValue()
    {
        List<float> MagicsValues = new List<float>();
        Debug.Log("Magias");
        Debug.Log("Primordial");
        MagicsValues.AddRange(GetArray(2, 5, 15, false, false));
        Debug.Log("Elemental");
        MagicsValues.AddRange(GetArray(2, 8, 20, false, false));
        Debug.Log("Sub");
        MagicsValues.AddRange(GetArray(9, 13, 16, false, false));
        
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
    
    public string[] GetArrayString(int x1, int x2, int y, bool invertXY = false, bool debug = false)
    {
        List<string> array = new List<string>();
        
        for (int x = x1; x < x2; x++)
        {
            array.Add(csv2DString(x,y));
            
            if(debug) Debug.Log(csv2DString(x, y) + $" - [{x}, {y}]");
        }

        return array.ToArray();
    }
    
    public string[] Get2DArrayString(int x1, int x2, int y1, int y2, bool debug = false)
    {
        List<string> array = new List<string>();
        
        for (int x = x1; x < x2; x++)
        {
            array.Add(csv2DString(x,y1) + " - " + csv2DString(x,y2));
            if(debug) Debug.Log(csv2DString(x,y1) + " - " + csv2DString(x,y2) + $" -> [{x}, {y1}]");
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
}
