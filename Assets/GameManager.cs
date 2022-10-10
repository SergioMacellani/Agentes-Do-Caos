using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
        if(generateNewCsvData) csv.ConvertPlayer(pSheet);
        ColorPaletteManager.LoadData(cPalette);
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
        playerName.text = pSheet.playerName.firstName + " " + pSheet.playerName.lastName;
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
    }
    
    private void BackgroundColor()
    {
        skyboxMaterial.SetColor("_Tint", ColorPaletteManager.GetColor("Normal"));
    }

    public void ImportCSV(string path)
    {
        csv.ConvertPlayer(pSheet, path);
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
}
