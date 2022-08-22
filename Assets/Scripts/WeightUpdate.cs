using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightUpdate : MonoBehaviour
{
    [HideInInspector]
    public float WeightTotal;
    
    private TMP_InputField textInput;

    private void Start()
    {
        TryGetComponent(out textInput);
    }
    
    public void UpdateWeight()
    {
        textInput.text = WeightTotal.ToString("#.###");
    }
}
