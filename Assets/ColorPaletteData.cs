using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette", menuName = "ADC/Color Palette")]
public class ColorPaletteData : ScriptableObject
{
    public List<ColorKey> colors = new List<ColorKey>();
    
    public void AddColor(ColorKey colorKey)
    {
        colors.Add(new ColorKey(colorKey));
    }
    
    public void RemoveColor(ColorKey colorKey)
    {
        colors.Remove(colorKey);
    }
    
    public bool ContainsColor(string key)
    {
        return colors.Find(x => x.name == key) != null;
    }

    public Color GetColor(string key)
    {
        return colors.Find(x => x.name == key).color;
    }
}

[System.Serializable]
public class ColorKey
{
    public string name;
    public Color color;
    
    public ColorKey(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }
    
    public ColorKey()
    {
        this.name = "New Color";
        this.color = Color.white;
    }
    
    public ColorKey(ColorKey colorKey)
    {
        this.name = colorKey.name;
        this.color = colorKey.color;
    }
}

public static class ColorPaletteManager
{
    private static ColorPaletteData colors;
    
    private static void DataNotFound()
    {
        Debug.LogWarning("Color Palette Data not found! Searching in Resources folder...");
        try
        {
            LoadData();
            
        }
        catch (Exception e)
        {
            Debug.LogWarning("Color Palette Data not found in Resources folder! Creating new Color Palette Data...");
            CreatePalette();
        }
    }

    public static void LoadData(string path = "Color Palette")
    {
        colors = Resources.Load<ColorPaletteData>(path);
    }
    
    public static void LoadData(ColorPaletteData data)
    {
        colors = data;
    }

    public static bool HasColor(string key)
    {
        if (colors == null) DataNotFound();
        
        return colors.ContainsColor(key);
    }
    
    public static Color GetColor(string key)
    {
        if (colors == null) LoadData();
        
        return colors.GetColor(key);
    }

    public static void CreatePalette()
    {
        colors = ScriptableObject.CreateInstance<ColorPaletteData>();
        colors.colors.Add(new ColorKey("Light", Color.white));
        colors.colors.Add(new ColorKey("Normal", Color.gray));
        colors.colors.Add(new ColorKey("Dark", Color.black));
    }
}