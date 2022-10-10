using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WebImportManager : MonoBehaviour
{
    [Header("Search")]
    [SerializeField]
    private GameObject searchPanel;
    [SerializeField]
    private GameObject loadingIcon;
    [SerializeField]
    private TMP_InputField searchInput;
    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private TextMeshProUGUI characterName;
    [SerializeField]
    private Sprite nullCharacterImage;
    
    [Header("Import")]
    [SerializeField]
    private GameObject importPanel;
    [SerializeField]
    private TextMeshProUGUI importText;
    [SerializeField]
    private TextMeshProUGUI importPercentageText;

    private PlayerSheetData pSheet;
    private bool playerSheetImported = false;
    
    [Space]
    public UnityEvent OnPlayerSheetImported;
    public bool UseDatabase { get; set; } = true;

    private void Awake()
    {
        pSheet = ScriptableObject.CreateInstance<PlayerSheetData>();
    }

    private void OnEnable()
    {
        searchPanel.SetActive(true);
        importPanel.SetActive(false);
        loadingIcon.SetActive(false);
        characterImage.sprite = nullCharacterImage;
        searchInput.text = "";
        characterName.text = "";
        playerSheetImported = false;
    }

    public void SearchCharacter(string urlPath)
    {
        loadingIcon.SetActive(false);
        loadingIcon.SetActive(true);
        characterImage.sprite = nullCharacterImage;
        characterName.text = "Carregando...";
        playerSheetImported = false;
        
        if (UseDatabase)
        {
            string path = $"{urlPath}/chardata.chaos";
            StartCoroutine(AJAX.GetRequestAsync(path, LoadedCharacter));
        }
        else
        {
            StartCoroutine(AJAX.GetRequestAsync("", LoadedCharacter, urlPath));
        }
    }
    
    private void LoadedCharacter(string data)
    {
        if (data != null)
        {
            pSheet.SetData(data);
            LoadImage();
        }
        else
        {
            characterImage.sprite = nullCharacterImage;
            characterName.text = "Personagem não encontrado";
            loadingIcon.SetActive(false);
        }
    }
    
    private void LoadImage()
    {
        if (UseDatabase)
        {
            string path = $"{pSheet.playerName.firstName.ToLower()}/0.png";
            StartCoroutine(AJAX.GetImage(path, characterName, LoadedImage));
        }
        else
        {
            StartCoroutine(AJAX.GetImage("", characterName, LoadedImage, pSheet.playerImages.imageLink[0]));
        }
    }

    private void LoadedImage(Sprite sprite)
    {
        if (sprite != null)
        {
            characterImage.sprite = sprite;
            characterName.text = pSheet.playerName.showName;
            loadingIcon.SetActive(false);
            playerSheetImported = true;
        }
        else
        {
            characterImage.sprite = nullCharacterImage;
            characterName.text = "Personagem não encontrado";
            loadingIcon.SetActive(false);
        }
    }
    
    public void ImportCharacter()
    {
        if (!playerSheetImported) return;
        
        searchPanel.SetActive(false);
        importPanel.SetActive(true);
        StartCoroutine(ImportCharacterAsync());
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator ImportCharacterAsync()
    {
        string playerName = pSheet.playerName.firstName.ToLower();

        SaveLoadSystem.SaveFile(JsonUtility.ToJson(pSheet, true),"chardata", "chaos",$"characters/{playerName}/");
        importText.text = $"Importando... 1/5";
        SaveLoadSystem.SaveFile(characterImage.sprite.texture.EncodeToPNG(), "0", "png", $"characters/{playerName}/");
        importText.text = $"Importando... 2/5";

        ImportImages(playerName,1);
        
        yield break;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ImportImages(string playerName, int i)
    {
        string path = $"{playerName}/{i}.png";
        StartCoroutine(AJAX.GetImage(UseDatabase ? path : "", importPercentageText, callback =>
        {
            importText.text = $"Importando... {i+2}/5";
            importPercentageText.text = $"";
            SaveLoadSystem.SaveFile(callback.texture.EncodeToPNG(), i.ToString(), "png", $"characters/{playerName}/");
            if (i < 3) ImportImages(playerName, i + 1);
            else StartCoroutine(CloseImport());
        }, UseDatabase ? "" : pSheet.playerImages.imageLink[i]));
    }

    private IEnumerator CloseImport()
    {
        yield return new WaitForSeconds(1f);
        OnPlayerSheetImported.Invoke();
    }
}
