using System.Collections;
using System.Collections.Generic;
using Ninito.UsualSuspects;
using UnityEngine;

public class TechniquesManager : MonoBehaviour
{
    [SerializeField]
    private TechniqueItem techniquePrefab;
    
    [SerializeField]
    private Transform[] techiniquesContainers;
    
    public void SetTechniques(SerializedDictionary<string, int> tecnics)
    {
        int i = 1;

        foreach (var tecnic in tecnics)
        {
            TechniqueItem techniqueItem;
            
            if (i < 8)
            {
                techniqueItem = Instantiate(techniquePrefab, techiniquesContainers[0]);   
            }
            else if (i < 15)
            {
                techniqueItem = Instantiate(techniquePrefab, techiniquesContainers[1]);  
            }
            else if (i < 21)
            {
                techniqueItem = Instantiate(techniquePrefab, techiniquesContainers[2]);  
            }
            else
            {
                techniqueItem = Instantiate(techniquePrefab, techiniquesContainers[3]);  
            }

            techniqueItem.Initialize(tecnic.Key, tecnic.Value);
            i++;
        }
    }
}
