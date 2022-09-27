using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentActiveAuto : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar;
    
    [SerializeField]
    private GameObject[] content;

    private void Start()
    {
        OnValueChanged();
    }

    public void OnValueChanged()
    {
        for (int i = 0; i < content.Length; i++)
        {
            float value = (float)i / (content.Length - 1);
            
            if (scrollbar.value >= value && scrollbar.value < value+.25f)
            {
                content[i].SetActive(true);
            }
            else
            {
                content[i].SetActive(false);
            }
        }
    }
}
