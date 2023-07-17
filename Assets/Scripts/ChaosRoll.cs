using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChaosRoll : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _value;
    [SerializeField] 
    private DiceManager diceManager;

    private StatusBar chaosBar;

    public void Initialize(StatusBar value)
    {
        chaosBar = value;
        _value.text = chaosBar.GetPercentage.ToString();

        if (diceManager.CurrentTechnique.Name == "Caos") SelectTechnique();
    }
    
    public void SelectTechnique()
    {
        diceManager.SetTechnique(new Technique("Caos", chaosBar.GetPercentage));
    }
}
