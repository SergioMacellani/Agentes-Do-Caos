using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSheetData pSheet;

    [SerializeField] private ConvertCSVData _csv;
    
    [SerializeField] private TextMeshProUGUI _playerName;
    
    [SerializeField] private DiceRoll _diceScript;
    [SerializeField] private PlayerBars _barsScript;
    [SerializeField] private PlayerStatus _statusScript;
    [SerializeField] private PlayerInventory _inventoryScript;
    [SerializeField] private PlayerPotion _potionsScript;
    [SerializeField] private PlayerNotes _notesScript;

    private void Awake()
    {
        //_csv.ConvertPlayer(pSheet);
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
    }
}
