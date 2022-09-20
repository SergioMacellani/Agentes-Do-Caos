using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class ColorPalette : MonoBehaviour
{
   private Graphic graphic => GetComponent<Graphic>();
   [SerializeField]
   private string colorName = "Normal";

   private void Start()
   {
      SetColor();
   }

   public void SetColor()
   {
      if(ColorPaletteManager.HasColor(colorName))
         graphic.color = ColorPaletteManager.GetColor(colorName);
   }

   private void OnValidate()
   {
      SetColor();
   }
}
