using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeInputValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    private float value;
    private bool isFloat = true;
    private float[] modiferValues = { .25f, 1, 10 };
    
    public void ChangeValue(float val)
    {
        value = val;
        valueText.text = value.ToString(isFloat ? "F2" : "F0");
    }
}
