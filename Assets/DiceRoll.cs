using System.Collections;
using System.Collections.Generic;
using Ninito.UsualSuspects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    [SerializeField]
    private TechniquesManager techniquesManager;
    [SerializeField]
    private ChaosRoll chaosRoll;
    [SerializeField] 
    private StatusBar chaosBar;
    
    [SerializeField] 
    private TextMeshProUGUI techniqueNameText;
    
    private int DiceRolls = 10;

    private TextMeshProUGUI diceValue;
    private TextMeshProUGUI valueType;
    
    private Technique currentTechnique;
    public Technique CurrentTechnique => currentTechnique;

    private void Awake()
    {
        transform.GetChild(1).TryGetComponent(out diceValue);
        transform.GetChild(0).TryGetComponent(out valueType);
    }

    public void SetValue(PlayerSheetData pSheet)
    {
        techniquesManager.SetTechniques(pSheet.techniques.Techniques);
        chaosRoll.Initialize(chaosBar);
        diceValue.text = "0";
        valueType.text = "";
    }
    
    public void SetTechnique(Technique technique)
    {
        currentTechnique = technique;
        techniqueNameText.text = $"{currentTechnique.Name}: {currentTechnique.Value}";
    }

    public void RollDice()
    {
        valueType.text = "";
        StopCoroutine(DiceRollTimer());
        StartCoroutine(DiceRollTimer());
    }

    private IEnumerator DiceRollTimer()
    {
        float numTime = .05f;
        for (int i = 0; i < DiceRolls; i++)
        {
            diceValue.text = Random.Range(1, 101).ToString();
            yield return new WaitForSeconds(numTime);
            numTime += .005f;
        }

        DiceNumber();
    }

    private void DiceNumber()
    {
        int diceResult = Random.Range(1, 101);
        diceValue.text = diceResult.ToString();
        valueType.text = GetValueType(diceResult);
    }

    private string GetValueType(int value)
    {
        float normal = currentTechnique.Value;
        int extreme = System.Convert.ToInt32(System.Math.Ceiling((15 * normal) / 100));
        int good = System.Convert.ToInt32(System.Math.Ceiling((50 * normal) / 100));

        switch (value)
        {
            case var n when (n == 100):
                return "Desastre";
            case var n when (n > normal):
                return "Falha";
            case var n when (n == 1):
                return "Perfeição";
            case var n when (n <= extreme):
                return "Extremo";
            case var n when (n <= good):
                return "Bom";
            case var n when (n <= normal):
                return "Normal";
            default:
                return "Error";
        }
    }
    
    public void OpenTechniques()
    {
        techniquesManager.OpenClose();
    }
}
