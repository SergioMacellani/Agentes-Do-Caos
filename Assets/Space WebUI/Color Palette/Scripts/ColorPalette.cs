using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[Icon("Assets/Space WebUI/Color Palette/Images/colorPalette_icon.png")]
[BaseTypeRequired(typeof(Graphic))]
public class ColorPalette : MonoBehaviour
{
   private Graphic graphic => GetComponent<Graphic>();
   private protected Color color;
   
   [SerializeField]
   private protected string colorName = "Normal";
   
   [SerializeField]
   private protected bool applyAlpha = true;

   private void OnEnable()
   {
      SetColor();
   }

   protected virtual void SetColor()
   {
      if (ColorPaletteManager.HasColor(colorName))
      {
         Color nextColor = ColorPaletteManager.GetColor(colorName);
         if(color == nextColor) return;
         color = nextColor;
         graphic.color = applyAlpha ? color : new Color(color.r, color.g, color.b, graphic.color.a);
      }
   }

#if UNITY_EDITOR
   private void OnValidate()
   {
      if(!Application.isPlaying) SetColor();
   }
#endif
}