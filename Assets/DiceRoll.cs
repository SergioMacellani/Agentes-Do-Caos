using System.Collections;
using System.Collections.Generic;
using Ninito.UsualSuspects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown techniquesDropdown;    
    [SerializeField]
    private TechniquesManager techniquesManager;
    [SerializeField] 
    private StatusBar chaosBar;
    
    private int DiceRolls = 10;

    private List<int> diceTechniques = new List<int>();

    private TextMeshProUGUI diceValue;
    private TextMeshProUGUI valueType;

    private void Awake()
    {
        transform.GetChild(1).TryGetComponent(out diceValue);
        transform.GetChild(0).TryGetComponent(out valueType);
    }

    public void SetValue(PlayerSheetData pSheet)
    {
        List<string> options = new List<string>();
        foreach (var d in pSheet.techniques.Techniques)
        {
            options.Add($"{d.Key}: {d.Value}");
            diceTechniques.Add(d.Value);
        }
        options.Add($"Caos: {chaosBar.GetCurrentValue}");
        diceTechniques.Add(chaosBar.GetCurrentValue);
        
        techniquesDropdown.AddOptions(options);
        techniquesManager.SetTechniques(pSheet.techniques.Techniques);
        diceValue.text = "0";
        valueType.text = "";
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
        float normal = diceTechniques[techniquesDropdown.value];
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
}
