using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    public Color col = Color.white;
    public Texture2D HUE;
    public Image[] Saturation;
    public Image Valor;
    
    public Texture2D Skin;
    public Texture2D Iris;

    public Slider HUESlider;
    public Slider SaturationSlider;
    public Slider ValorSlider;
    
    public Slider SkinSlider;
    public Slider IrisSlider;

    public TMP_InputField HEXCodeInput;
    public Image ColorShow;
    public Image ColorShow2;
    
    public GameObject HueGameObject;
    public GameObject DoubleColorGameObject;

    public SetStyleColor StyleColor;

    private bool DoubleColor = false;
    private float h, s, v;

    public void UpdateHUE()
    {
        float sv;
        Color.RGBToHSV(col, out h, out s, out v);
        Color.RGBToHSV(HUE.GetPixel((int)HUESlider.value, 0), out h, out sv, out sv);
        col = Color.HSVToRGB(h,s,v);
        
        HUESlider.handleRect.GetComponent<Image>().color = HUE.GetPixel((int)HUESlider.value, 0);
        SaturationSlider.handleRect.GetComponent<Image>().color = col;
        ValorSlider.handleRect.GetComponent<Image>().color = col;

        Saturation[1].color = Color.HSVToRGB(h,1,v);
        Valor.color = col;

        UpdateColor();
    }
    
    public void UpdateSaturation()
    {
        Color.RGBToHSV(col, out h, out s, out v);
        col = Color.HSVToRGB(h,SaturationSlider.value/100,v);
        Color.RGBToHSV(col, out h, out s, out v);
        
        SaturationSlider.handleRect.GetComponent<Image>().color = col;
        ValorSlider.handleRect.GetComponent<Image>().color = col;
        
        Saturation[1].color = Color.HSVToRGB(h,1,v);
        Valor.color = Color.HSVToRGB(h,s,1);
        
        UpdateColor();
    }
    
    public void UpdateValor()
    {
        Color.RGBToHSV(col, out h, out s, out v);
        col = Color.HSVToRGB(h,s,ValorSlider.value/100);
        Color.RGBToHSV(col, out h, out s, out v);
        
        SaturationSlider.handleRect.GetComponent<Image>().color = col;
        ValorSlider.handleRect.GetComponent<Image>().color = col;
        
        Saturation[0].color = Color.HSVToRGB(0,0,ValorSlider.value/100);
        Saturation[1].color = Color.HSVToRGB(h,1,v);

        UpdateColor();
    }
    
    public void UpdateColor()
    {
        string hex = ColorUtility.ToHtmlStringRGB(col);
        HEXCodeInput.text = $"#{hex}";

        if (DoubleColorGameObject.activeSelf)
        {
            StyleColor.UpdateColor(ColorShow.color, ColorShow2.color);
            if (!DoubleColor)
            {
                ColorShow.color = col;  
            }
            else
            {
                ColorShow2.color = col;
            }
        }
        else
        {
            ColorShow.color = col;  
            StyleColor.UpdateColor(ColorShow.color, ColorShow.color);
        }
        
    }

    public void UpdateHEX()
    {
        string hex = HEXCodeInput.text;
        
        ColorUtility.TryParseHtmlString(hex, out col);
        Color.RGBToHSV(col, out h, out s, out v);

        HUESlider.value = h*1024;
        SaturationSlider.value = s*100;
        ValorSlider.value = v*100;
    }

    public void ChangeHUESlider(GameObject ColorSlider)
    {
        if (ColorSlider.name != "HUE")
        {
            if (ColorSlider.name == "SKIN")
            {
                ColorSlider.SetActive(true);
                IrisSlider.gameObject.SetActive(false);
                UpdateSkin();
            }
            else if (ColorSlider.name == "IRIS")
            {
                SkinSlider.gameObject.SetActive(false);
                ColorSlider.SetActive(true);
                UpdateIris();
            }
            HueGameObject.SetActive(false);
            UpdateValor();
            UpdateSaturation();
        }
        else
        {
            SkinSlider.gameObject.SetActive(false);
            IrisSlider.gameObject.SetActive(false);
            HueGameObject.SetActive(true);
            
            HUESlider.value = h*1024;
            SaturationSlider.value = s*100;
            ValorSlider.value = v*100;
        }
    }
    
    public void UpdateSkin()
    {
        Color.RGBToHSV(col, out h, out s, out v);
        Color.RGBToHSV(Skin.GetPixel((int)SkinSlider.value, 0), out h, out s, out v);
        col = Color.HSVToRGB(h,s,v);
        
        SkinSlider.handleRect.GetComponent<Image>().color = Skin.GetPixel((int)SkinSlider.value, 0);

        SaturationSlider.value = s*100;
        ValorSlider.value = v*100;
        
        UpdateColor();
    }
    
    public void UpdateIris()
    {
        Color.RGBToHSV(col, out h, out s, out v);
        Color.RGBToHSV(Iris.GetPixel((int)IrisSlider.value, 0), out h, out s, out v);
        col = Color.HSVToRGB(h,s,v);
        
        IrisSlider.handleRect.GetComponent<Image>().color = Iris.GetPixel((int)IrisSlider.value, 0);

        SaturationSlider.value = s*100;
        ValorSlider.value = v*100;
        
        UpdateColor();
    }

    public void DoubleColors()
    {
        if (DoubleColorGameObject.activeSelf)
        {
            DoubleColorGameObject.SetActive(false);
            ColorShow.GetComponent<Button>().interactable = false;
            DoubleColor = false;
        }
        else
        {
            DoubleColorGameObject.SetActive(true);
            ColorShow.GetComponent<Button>().interactable = true;
            DoubleColor = true;
        }
    }

    public void DoubleSelect(bool select)
    {
        DoubleColor = select;
    }

    public void SetColor(string hex)
    {
        ColorUtility.TryParseHtmlString("#"+hex, out col);
        UpdateColor();
        UpdateHEX();
    }
}
