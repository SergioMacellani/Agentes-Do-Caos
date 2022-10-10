using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform content;
    [SerializeField] private RectTransform addChar;

    private string[] charDirectory;

    private ScrollSnap scrollSnap => GetComponent<ScrollSnap>();
    private void OnEnable()
    {
        DetectCharacters();
    }

    private void DetectCharacters()
    {
        PlayerSheetData pSheet = ScriptableObject.CreateInstance<PlayerSheetData>();
        string[] directory = Directory.GetDirectories($"{SaveLoadSystem.path}characters/");
        if (charDirectory == directory) return;
        else charDirectory = directory;

        content.gameObject.SetActive(false);
        
        foreach (Transform charBtt in content)
        {
            if(charBtt != addChar) Destroy(charBtt.gameObject);
        }
        
        foreach (var dir in charDirectory)
        {
            string json = "";
            SaveLoadSystem.LoadFile("/chardata.chaos", out json, dir, false);
            pSheet.SetData(json);
            
            Transform character = Instantiate(prefab, content).transform;
            
            character.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = SaveLoadSystem.LoadImage("/0.png", dir, false);
            character.GetComponentInChildren<TextMeshProUGUI>().text = pSheet.playerName.showName;
        }
        
        addChar.SetAsLastSibling();
        content.gameObject.SetActive(true);
        
        scrollSnap.horizontalScrollbar.value = 0;
        scrollSnap.UpdateSnap();
    }
}
