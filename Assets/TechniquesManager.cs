using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ninito.UsualSuspects;
using UnityEngine;

public class TechniquesManager : MonoBehaviour
{
    [SerializeField]
    private DiceManager diceManager;
    
    [SerializeField]
    private TechniqueItem techniquePrefab;

    [SerializeField] 
    private GameObject parentGameObject;
    
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

            techniqueItem.Initialize(new Technique(tecnic.Key, tecnic.Value), this);
            i++;
        }
        
        SelectTechnique(new Technique(tecnics.Keys.ToArray()[0], tecnics.Values.ToArray()[0]));
        parentGameObject.SetActive(false);
    }
    
    public void SelectTechnique(Technique technique)
    {
        diceManager.SetTechnique(technique);
        OpenClose();
    }

    public void OpenClose()
    {
        parentGameObject.SetActive(!parentGameObject.activeSelf);
    }
}
