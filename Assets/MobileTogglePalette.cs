using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTogglePalette : ColorPalette
{
    private MobileToggle mobileToggle => GetComponent<MobileToggle>();
    protected override void SetColor()
    {
        if (ColorPaletteManager.IsGray())
        {
            mobileToggle.onColor = new Color(0,1,0.4470588f, 1);
            return;
        }
        
        if (ColorPaletteManager.HasColor(colorName))
        {
            Color nextColor = ColorPaletteManager.GetColor(colorName);
            if (color == nextColor) return;

            color = nextColor;
            mobileToggle.onColor = applyAlpha ? color : new Color(color.r, color.g, color.b, mobileToggle.onColor.a);
        }
    }
}
