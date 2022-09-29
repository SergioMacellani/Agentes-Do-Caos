using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private PlayerSheetData pSheet;
    [SerializeField]
    private ColorPaletteData cPalette;

    [Space]
    [Header("CSV")]
    [SerializeField] private ConvertCSVData _csv;
    public bool generateNewCSVData = false;
    
    [Space]
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private DiceRoll _diceScript;
    [SerializeField] private PlayerBars _barsScript;
    [SerializeField] private PlayerStatus _statusScript;
    [SerializeField] private SkillsManager _skillsScript;
    [SerializeField] private MagicsManager _magicsScript;
    [SerializeField] private PlayerInventory _inventoryScript;
    [SerializeField] private PlayerPotion _potionsScript;
    [SerializeField] private PlayerNotes _notesScript;
    
    [Space]
    [Header("Colors")]
    [SerializeField] private Material _background;

    private void Awake()
    {
        if(generateNewCSVData) _csv.ConvertPlayer(pSheet);
        ColorPaletteManager.LoadData(cPalette);
    }

    private void Start()
    {
        _playerName.text = pSheet.playerName.firstName + " " + pSheet.playerName.lastName;
        
        _barsScript.SetValue(pSheet);
        _inventoryScript.SetValue(pSheet);
        _notesScript.SetValue(pSheet);
        _potionsScript.SetValue(pSheet.potions);
        _statusScript.SetValue(pSheet);
        _diceScript.SetValue(pSheet);
        _skillsScript.SetSkills(1,2,2,1);
        _magicsScript.SetMagicPoints(pSheet.magics);

        BackgroundColor();
    }
    
    private void BackgroundColor()
    {
        _background.SetColor("_Tint", ColorPaletteManager.GetColor("Normal"));
    }

    public void ImportCSV(string path)
    {
        _csv.ConvertPlayer(pSheet, path);
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
