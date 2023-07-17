using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ninito.UsualSuspects;
using TMPro;
using UnityEngine;

public class UpdateTechniques : MonoBehaviour
{
    private SerializedDictionary<string, int> tecnics;
    
    [SerializeField]
    private ChangeTechniquesValue changeTechniquesValue;
    
    [SerializeField]
    private TechniquesManager techniquesManager;
    
    [SerializeField]
    private TechniqueItem techniquePrefab;

    [SerializeField] private TextMeshProUGUI pointText;
    
    [Space]

    [SerializeField]
    private Transform[] psiqueTechiniquesContainers;
    [SerializeField]
    private Transform[] combatTechiniquesContainers;
    [SerializeField]
    private Transform ocultismTechiniquesContainer;
    [SerializeField]
    private Transform senseTechiniquesContainer;

    public void Initialize(PlayerSheetData pSheet)
    {
        tecnics = pSheet.techniques.Techniques;
        SetTechniques();
        gameObject.SetActive(true);
        updatePointText();
    }
    
    public void SetTechniques()
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

            techniqueItem.Initialize(new Technique(tecnic.Key, tecnic.Value), changeTechniquesValue);
            i++;
        }
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

    public void UpdateItem(Technique technique)
    {
        tecnics[technique.Name] = technique.Value;
        updatePointText();
    }
    
    public void SaveUpdate()
    {
        techniquesManager.SetTechniques(tecnics);
        GameManager.Instance.SetTechniques(tecnics);
        gameObject.SetActive(false);
    }
    
    public void CancelUpdate()
    {
        gameObject.SetActive(false);
        ClearTechniques();
    }

    private void updatePointText()
    {
        int p = 0;
        foreach (var tec in tecnics)
        {
            p += tec.Value;
        }

        pointText.text = $"PONTOS: {p}";
    }
}
