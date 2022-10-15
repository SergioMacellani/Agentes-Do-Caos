using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private PlayerSheetData pSheet;
    [SerializeField]
    private ColorPaletteData cPalette;

    [Space]
    [Header("CSV")]
    [SerializeField] private ConvertCSVData csv;
    [SerializeField] private bool generateNewCsvData = false;
    
    [Space]
    [Header("UI")]
    [SerializeField] private Image savingIcon;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TMP_InputField abilityPoints;
    
    [Space]
    [Header("Managers")]
    [SerializeField] private DiceManager diceScript;
    [SerializeField] private EssentialsManager essentialsScript;
    [SerializeField] private StatusManager statusScript;
    [SerializeField] private SkillsManager skillsScript;
    [SerializeField] private MagicsManager magicsScript;
    [SerializeField] private InventoryManager inventoryScript;
    [SerializeField] private PotionsManager potionsScript;
    [SerializeField] private NotesManager notesScript;
    
    [Space]
    [Header("Colors")]
    [SerializeField] private Material skyboxMaterial;

    private void Awake()
    {
        pSheet = GameInfo.PlayerSheetData;
        if(generateNewCsvData) csv.ConvertPlayer(ref pSheet);
    }

    private void Start()
    {
        //SaveLoadSystem.SaveFile(JsonUtility.ToJson(pSheet, true),"hanna", "chaos","/characters/");
        //StartCoroutine(AJAX.GetRequestAsync(LoadData));
        SetSheetValues();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void LoadData(string data)
    {
        pSheet.SetData(data);

        SetSheetValues();
    }

    private void SetSheetValues()
    {
        playerName.text = pSheet.playerName.showName;
        abilityPoints.text = pSheet.abilityPoints.ToString();
        
        essentialsScript.SetValue(pSheet);
        inventoryScript.SetValue(pSheet);
        notesScript.SetValue(pSheet);
        potionsScript.SetValue(pSheet.potions);
        statusScript.SetValue(pSheet);
        diceScript.SetValue(pSheet);
        skillsScript.SetSkills(1,2,2,1);
        magicsScript.SetMagicPoints(pSheet.magics);
        BackgroundColor();
        StartCoroutine(SaveTimer());
    }

    public void SaveData(bool saveExit = false)
    {
        StartCoroutine(SavePlayerSheetAsync(saveExit));
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) SaveData();
    }

    private void OnApplicationQuit()
    {
        SavePlayerSheet();
    }

    private void SavePlayerSheet()
    {
        pSheet.essential = essentialsScript.GetValue();
        pSheet.stats = statusScript.GetValue();
        pSheet.inventory = inventoryScript.GetValue();
        pSheet.potions = potionsScript.GetValue();
        pSheet.texts = notesScript.GetValue();
    }

    private IEnumerator SavePlayerSheetAsync(bool saveExit)
    {
        savingIcon.gameObject.SetActive(true);
        
        pSheet.essential = essentialsScript.GetValue();
        pSheet.stats = statusScript.GetValue();
        pSheet.inventory = inventoryScript.GetValue();
        pSheet.potions = potionsScript.GetValue();
        pSheet.texts = notesScript.GetValue();
        
        savingIcon.gameObject.SetActive(false);
        
        if(saveExit) GameInfo.LoadScene("Menu");
        
        yield break;
    }
    
    private void BackgroundColor()
    {
        skyboxMaterial.SetColor("_Tint", ColorPaletteManager.GetColor("Normal"));
    }

    public void ImportCSV(string path)
    {
        csv.ConvertPlayer(ref pSheet, path);
    }

    public void ImportChaos(string path)
    {
        
    }

    public void DownloadChaos(string path)
    {
        
    }

    public void ShareChaos()
    {
        
    }
    
    private IEnumerator SaveTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            SaveData();
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
