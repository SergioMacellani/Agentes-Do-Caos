using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChaosRoll : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _value;
    [SerializeField] 
    private DiceRoll _diceRoll;

    private StatusBar chaosBar;

    public void Initialize(StatusBar value)
    {
        chaosBar = value;
        _value.text = chaosBar.GetPercentage.ToString();

        if (_diceRoll.CurrentTechnique.Name == "Caos") SelectTechnique();
    }
    
    public void SelectTechnique()
    {
        _diceRoll.SetTechnique(new Technique("Caos", chaosBar.GetPercentage));
    }
}
