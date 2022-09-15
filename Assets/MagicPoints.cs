using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicPoints : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI title;
    
    [SerializeField]
    private GameObject[] magicContainers;

    [SerializeField] private int index = 0;
    
    private GameObject activeContainer;

    private void Awake()
    {
        UpdateContainer();
    }
    
    public void NextContainer()
    {
        if (index < magicContainers.Length - 1) index++;
        else index = 0;

        ActivateContainer();
    }
    
    public void PreviousContainer()
    {
        if (index > 0) index--;
        else index = magicContainers.Length - 1;

        ActivateContainer();
    }
    
    private void ActivateContainer()
    {
        activeContainer.SetActive(false);
        activeContainer = magicContainers[index];
        activeContainer.SetActive(true);
        
        title.text = activeContainer.name;
    }

    private void UpdateContainer()
    {
        foreach (var container in magicContainers)
        {
            container.SetActive(false);
        }
        
        activeContainer = magicContainers[index];
        activeContainer.SetActive(true);
        
        title.text = activeContainer.name;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateContainer();
    }
#endif
}
