using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TechniqueItem : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _name;
    [SerializeField] 
    private TextMeshProUGUI _value;
    public void Initialize(string name, int value)
    {
        _name.text = name;
        _value.text = value.ToString();
    }
}
