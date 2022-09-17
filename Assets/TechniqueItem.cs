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

    private Technique technique;
    
    public void Initialize(Technique value, TechniquesManager techniquesManager)
    {
        technique = value;
        _name.text = technique.Name;
        _value.text = technique.Value.ToString();
        _techniquesManager = techniquesManager;
    }
    
    public void SelectTechnique()
    {
        _techniquesManager.SelectTechnique(technique);
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
