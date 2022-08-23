using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBars : MonoBehaviour
{
    [SerializeField] private StatusBar lifeBar;
    [SerializeField] private StatusBar chaosBar;
    [SerializeField] private StatusBar armorBar;
    [SerializeField] private StatusBar armorLifeBar;

    public void SetValue(PlayerSheetData pSheet)
    {
        lifeBar.SetValue(pSheet.essential.playerLife);
        chaosBar.SetValue(pSheet.essential.playerChaos);
        armorBar.SetValue(pSheet.essential.playerArmor);
        armorLifeBar.SetValue(pSheet.essential.playerArmorLife);
    }
}
