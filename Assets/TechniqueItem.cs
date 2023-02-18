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
    [SerializeField] 
    private TechniquesManager _techniquesManager;
    [SerializeField]
    private ChangeTechniquesValue _changeTechniquesValue;

    private Technique technique;
    private int legacyValue;

    public int GetValue => technique.Value;
    public string GetName => technique.Name;
    
    public void Initialize(Technique value, TechniquesManager techniquesManager)
    {
        technique = value;
        _name.text = technique.Name;
        _value.text = technique.Value.ToString();
        _techniquesManager = techniquesManager;
    }
    
    public void Initialize(Technique value, ChangeTechniquesValue changeTechniquesValue)
    {
        technique = value;
        legacyValue = value.Value;
        _name.text = technique.Name;
        _value.text = technique.Value.ToString();
        _changeTechniquesValue = changeTechniquesValue;
    }
    
    public void SelectTechnique()
    {
        _techniquesManager.SelectTechnique(technique);
    }

    public void UpdateValue()
    {
        _changeTechniquesValue.OpenChangeInput(this, legacyValue);
    }

    public void SaveValue(int value)
    {
        technique.Value = value;
        _value.text = value.ToString();
    }
}

[System.Serializable]
public class Technique
{
    public string Name;
    public int Value;
    
    public Technique(string name, int value)
    {
        Name = name;
        Value = value;
    }
}
