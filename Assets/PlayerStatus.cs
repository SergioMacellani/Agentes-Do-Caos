using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private Toggle injured;
    [SerializeField] private Toggle insane;
    [SerializeField] private Toggle unconscious;

    public void SetValue(PlayerSheetData pSheet)
    {
        injured.isOn = pSheet.stats.injured;
        insane.isOn = pSheet.stats.insane;
        unconscious.isOn = pSheet.stats.unconscious;
    }
}
