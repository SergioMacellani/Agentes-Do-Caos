using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeightItem : MonoBehaviour
{
    private TMP_InputField textInput;
    private float oldNum;
    [HideInInspector]
    public WeightUpdate WeightUpdateScript;

    private void Start()
    {
        TryGetComponent(out textInput);
        oldNum = Single.Parse(textInput.text);
        WeightUpdateScript.WeightTotal += oldNum;
        WeightUpdateScript.UpdateWeight();
    }

    public void UpdateWeight()
    {
        WeightUpdateScript.WeightTotal -= oldNum;
        WeightUpdateScript.WeightTotal += Single.Parse(textInput.text);
        oldNum = Single.Parse(textInput.text);
        WeightUpdateScript.UpdateWeight();
    }
}
