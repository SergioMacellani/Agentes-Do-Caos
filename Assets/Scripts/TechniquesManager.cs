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
    private Transform[] psiqueTechiniquesContainers;
    [SerializeField]
    private Transform[] combatTechiniquesContainers;
    [SerializeField]
    private Transform ocultismTechiniquesContainer;
    [SerializeField]
    private Transform senseTechiniquesContainer;
    
    public void SetTechniques(SerializedDictionary<string, int> tecnics)
    {
        ClearTechniques();
        
        var i = 1;

        foreach (var tecnic in tecnics)
        {
            TechniqueItem techniqueItem;
            
            if (i < 5)
            {
                techniqueItem = Instantiate(techniquePrefab, psiqueTechiniquesContainers[0]);   
            }
            else if (i < 8)
            {
                techniqueItem = Instantiate(techniquePrefab, psiqueTechiniquesContainers[1]);   
            }
            else if (i < 12)
            {
                techniqueItem = Instantiate(techniquePrefab, combatTechiniquesContainers[0]);  
            }
            else if (i < 15)
            {
                techniqueItem = Instantiate(techniquePrefab, combatTechiniquesContainers[1]);  
            }
            else if (i < 21)
            {
                techniqueItem = Instantiate(techniquePrefab, ocultismTechiniquesContainer);  
            }
            else
            {
                techniqueItem = Instantiate(techniquePrefab, senseTechiniquesContainer);  
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
    
    private void ClearTechniques()
    {
        foreach (Transform child in psiqueTechiniquesContainers[0])
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in psiqueTechiniquesContainers[1])
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in combatTechiniquesContainers[0])
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in combatTechiniquesContainers[1])
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in ocultismTechiniquesContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in senseTechiniquesContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
