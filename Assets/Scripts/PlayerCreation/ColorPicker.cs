using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ColorPicker : MonoBehaviour
{
    public Color pickerColor = new Color(.565f,.565f,.565f);
    public GraphicTarget[] graphicTarget;
    
    public Texture2D HUE;
    public ColorBar HUEBar;
    public ColorBar SaturationBar;
    public ColorBar ValueBar;

    public TMP_InputField HEXCodeInput;
    
    public UnityEvent<Color> OnColorChanged;

    private float h, s, v;

    private void OnEnable()
    {
        Color.RGBToHSV(pickerColor, out h, out s, out v);
        
        HUEBar.value = h*1024;
        SaturationBar.value = s;
        ValueBar.value = v;

        UpdateHEXText();
    }

    public void UpdateHUE()
    {
        Color.RGBToHSV(pickerColor, out h, out s, out v);
        Color.RGBToHSV(HUE.GetPixel((int)HUEBar.value, 0), out h, out _, out _);
        pickerColor = Color.HSVToRGB(h,s,v);
        
        HUEBar.barHandle.color = HUE.GetPixel((int)HUEBar.value, 0);
        SaturationBar.barHandle.color = pickerColor;
        ValueBar.barHandle.color = pickerColor;

        SaturationBar.barForeground.color = Color.HSVToRGB(h,1,v);
        ValueBar.barBackground.color = pickerColor;

        UpdateHEXText();
    }
    
    public void UpdateSaturation()
    {
        Color.RGBToHSV(pickerColor, out h, out s, out v);
        pickerColor = Color.HSVToRGB(h,SaturationBar.value,v);
        Color.RGBToHSV(pickerColor, out h, out s, out v);
        
        SaturationBar.barHandle.color = pickerColor;
        ValueBar.barHandle.color = pickerColor;
        
        SaturationBar.barForeground.color = Color.HSVToRGB(h,1,v);
        ValueBar.barBackground.color = Color.HSVToRGB(h,s,1);
        
        UpdateHEXText();
    }
    
    public void UpdateValor()
    {
        Color.RGBToHSV(pickerColor, out h, out s, out v);
        pickerColor = Color.HSVToRGB(h,s,ValueBar.value);
        Color.RGBToHSV(pickerColor, out h, out s, out v);
        
        SaturationBar.barHandle.color = pickerColor;
        ValueBar.barHandle.color = pickerColor;
        
        SaturationBar.barBackground.color = Color.HSVToRGB(0,0,ValueBar.value);
        SaturationBar.barForeground.color = Color.HSVToRGB(h,1,v);

        UpdateHEXText();
    }
    
    public void UpdateHEXText()
    {
        string hex = ColorUtility.ToHtmlStringRGB(pickerColor);
        HEXCodeInput.text = $"#{hex}";

        UpdateGraphic();
    }

    public void UpdateHEX(string hex)
    {
        if (hex[0] != '#')
        {
            hex = $"#{hex}";
            HEXCodeInput.text = hex;
        }
        
        ColorUtility.TryParseHtmlString(hex, out pickerColor);
        Color.RGBToHSV(pickerColor, out h, out s, out v);

        HUEBar.value = h*1024;
        SaturationBar.value = s;
        ValueBar.value = v;

        UpdateGraphic();
    }

    public void UpdateInputValue()
    {
        pickerColor = Color.HSVToRGB(HUEBar.inputValue, SaturationBar.inputValue, ValueBar.inputValue);
        Color.RGBToHSV(pickerColor, out h, out s, out v);
        
        HUEBar.value = h*1024;
        SaturationBar.value = s;
        ValueBar.value = v;

        UpdateHEXText();
    }

    private void UpdateGraphic()
    {
        HUEBar.barValue.text = ((HUEBar.value * 256) / 1024).ToString("##0");
        SaturationBar.barValue.text = (SaturationBar.value * 256).ToString("##0");
        ValueBar.barValue.text = (ValueBar.value * 256).ToString("##0");
        
        foreach (var target in graphicTarget)
        {
            target.target.color = new Color(pickerColor.r, pickerColor.g, pickerColor.b, target.alpha);
        }
        List<ColorKey> colorKeys = new List<ColorKey>();
        colorKeys.Add(new ColorKey("Light", Color.HSVToRGB(h,Mathf.Clamp01(s-.35f),v)));
        colorKeys.Add(new ColorKey("Normal", pickerColor));
        colorKeys.Add(new ColorKey("Dark", Color.HSVToRGB(h,s,Mathf.Clamp01(v-.30f))));
        colorKeys.Add(new ColorKey("Menu", Color.HSVToRGB(h,s,.25f)));

        ColorPaletteManager.CreatePalette(colorKeys.ToArray());
        OnColorChanged.Invoke(pickerColor);
    }

    public void SetColor(string hex)
    {
        ColorUtility.TryParseHtmlString("#"+hex, out pickerColor);
        UpdateHEXText();
    }
}

[System.Serializable]
public class ColorBar
{
    public Slider barSlider;
    public Image barHandle;
    public Image barBackground;
    public Image barForeground;
    public TMP_InputField barValue;

    public float value
    {
        get => barSlider.value;
        set => barSlider.value = value;
    }

    public float inputValue => float.Parse(barValue.text)/256;
}

[System.Serializable]
public class GraphicTarget
{
    public Graphic target;
    [Range(0, 1)] public float alpha = 1;
}
