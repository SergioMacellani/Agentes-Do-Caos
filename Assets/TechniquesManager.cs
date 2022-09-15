using System.Collections;
using System.Collections.Generic;
using Ninito.UsualSuspects;
using UnityEngine;

public class TechniquesManager : MonoBehaviour
{
    [SerializeField]
    private TechniqueItem techniquePrefab;
    
    public void SetTechniques(SerializedDictionary<string, int> tecnics)
    {
        foreach (var tecnic in tecnics)
        {
            TechniqueItem techniqueItem = Instantiate(techniquePrefab, transform);
            techniqueItem.Initialize(tecnic.Key, tecnic.Value);
        }
    }
}
