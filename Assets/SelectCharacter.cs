using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private CharacterInfo charPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private RectTransform addChar;
    [SerializeField] private RectTransform masterChar;

    private string[] charDirectory = Array.Empty<string>();
    private ScrollSnap scrollSnap => GetComponent<ScrollSnap>();

    private bool updateScroll = false;
    
    private void OnEnable()
    {
        DetectCharacters();
    }

    private void LateUpdate()
    {
        if (!updateScroll) return;
        
        updateScroll = false;
        scrollSnap.UpdateSnap(true);
    }

    public void OpenMasterScene()
    {
        GameInfo.LoadScene("MasterPage");
    }

    private void DetectCharacters()
    {
        if (!Directory.Exists($"{SaveLoadSystem.path}characters/")) Directory.CreateDirectory($"{SaveLoadSystem.path}characters/");
        
        PlayerSheetData pSheet = ScriptableObject.CreateInstance<PlayerSheetData>();
        string[] directory = Directory.GetDirectories($"{SaveLoadSystem.path}characters/");

        if (directory.Length == charDirectory.Length)
        {
            var recreate = directory.Where((t, i) => t != charDirectory[i]).Any();
            if (!recreate) return;
        }

        charDirectory = directory;

        content.gameObject.SetActive(false);
        
        foreach (Transform charBtt in content)
        {
            if(charBtt != addChar && charBtt !=  masterChar) Destroy(charBtt.gameObject);
        }
        
        foreach (var dir in charDirectory)
        {
            string json = "";
            SaveLoadSystem.LoadFile("/chardata.chaos", out json, dir, false);
            pSheet.SetData(json);

            CharacterInfo character = Instantiate(charPrefab, content);
            
            character.SetCharacterInfo(pSheet.playerName, SaveLoadSystem.LoadImage("/0.png", dir, false), pSheet.playerColors);
        }
        
        addChar.SetAsLastSibling();
        content.gameObject.SetActive(true);
        updateScroll = true;
    }
}
