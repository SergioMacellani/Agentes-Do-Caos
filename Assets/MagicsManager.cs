using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicsManager : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI title;
    
    [SerializeField]
    private GameObject[] magicContainers;

    [SerializeField] private int index = 0;

    [Space] 
    [SerializeField] private List<TextMeshProUGUI> primordialMagics = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> essentialMagics = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> subMagics = new List<TextMeshProUGUI>();

    private GameObject activeContainer;

    private void Awake()
    {
        UpdateContainer();
    }

    public void SetMagicPoints(PlayerMagics magic)
    {
        primordialMagics[0].text = magic.Magics["Caos"].ToString();
        primordialMagics[1].text = magic.Magics["Tempo"].ToString();
        primordialMagics[2].text = magic.Magics["Vazio"].ToString();
        
        essentialMagics[0].text = magic.Magics["Fogo"].ToString();
        essentialMagics[1].text = magic.Magics["Energia"].ToString();
        essentialMagics[2].text = magic.Magics["Mente"].ToString();
        essentialMagics[3].text = magic.Magics["Vida"].ToString();
        essentialMagics[4].text = magic.Magics["Gelo"].ToString();
        essentialMagics[5].text = magic.Magics["Ar"].ToString();
        
        subMagics[0].text = magic.Magics["Sangue"].ToString();
        subMagics[1].text = magic.Magics["Absoluto"].ToString();
        subMagics[2].text = magic.Magics["Climatico"].ToString();
        subMagics[3].text = magic.Magics["Água"].ToString();
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
