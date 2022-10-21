using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public Canvas _canvas;
    public GameObject _magicArmor;
    public RectTransform _techniqueContainer;

    public void ColorSheet(bool colorSheet)
    {
        if(ColorPaletteManager.IsGray() && !colorSheet) return;
        
        _canvas.gameObject.SetActive(false);
        ColorPaletteManager.LoadData((colorSheet ? "Color Palette" : "Grey Palette"));
        _canvas.gameObject.SetActive(true);
    }
    
    public void MagicArmor(bool magicArmor)
    {
        _magicArmor.SetActive(magicArmor);
    }
    
    public void TechniqueContainer(bool techniqueContainer)
    {
        _techniqueContainer.sizeDelta = new Vector2(techniqueContainer ? 0 : -690, 0);
    }
}
