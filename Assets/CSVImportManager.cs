using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSVImportManager : MonoBehaviour
{
    [SerializeField]
    private Color characterColor;
    
    [SerializeField]
    private List<Image> characterImages = new List<Image>();
    private float h, s, v;
    public void Awake()
    {
        Color.RGBToHSV(characterColor, out h, out s, out v);
        List<ColorKey> colorKeys = new List<ColorKey>();
        colorKeys.Add(new ColorKey("Light", Color.HSVToRGB(h,Mathf.Clamp01(s-.35f),v)));
        colorKeys.Add(new ColorKey("Normal", characterColor));
        colorKeys.Add(new ColorKey("Dark", Color.HSVToRGB(h,s,Mathf.Clamp01(v-.30f))));
        colorKeys.Add(new ColorKey("Menu", Color.HSVToRGB(h,s,.25f)));

        ColorPaletteManager.CreatePalette(colorKeys.ToArray());
    }

    public void SetAvatar(int i)
    {
        string path = SaveLoadSystem.OpenFileExplorer("Selecione o seu avatar", "png");
        if (path == null) return;
        
        characterImages[i].sprite = SaveLoadSystem.LoadImage("", path, false);
    }
}
