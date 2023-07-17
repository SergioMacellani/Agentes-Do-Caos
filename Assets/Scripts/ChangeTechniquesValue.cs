using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTechniquesValue : MonoBehaviour
{
    [SerializeField] private UpdateTechniques _updateTechniques;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TMP_InputField valueText;
    [SerializeField] private Image[] modifierImages;
    
    private Color _activeColor = new Color(1,1,1,.25f);
    private Color _inactiveColor = new Color(.35f,.35f,.35f,.25f);
    
    private int value;
    public int baseValue;
    private int[] modiferValues = { 1, 5, 10 };
    private int valueModifier = 1;
    
    private TechniqueItem _technique;
    private CanvasGroup _canvasGroup => GetComponent<CanvasGroup>();

    public virtual void OpenChangeInput(TechniqueItem technique, int legacy)
    {
        _technique = technique;
        CanvasGroupSettings(true);
        valueText.text = technique.GetValue.ToString();
        value = int.Parse(valueText.text);
        baseValue = legacy;
        ChangeModifier(0);
        titleText.text = $"MODIFICAR VALOR DE {technique.GetName.ToUpper()}";
    }
    
    public void ChangeValue(int val)
    {
        value += val * valueModifier;
        if(value < baseValue)
            value = baseValue;
        
        valueText.text = value.ToString("F0");
    }
    
    public void ChangeValue(string val)
    {
        value = int.Parse(val);
        if(value < baseValue)
            value = baseValue;
        
        valueText.text = value.ToString("F0");
    }
    
    public void ChangeModifier(int index)
    {
        for (int i = 0; i < modifierImages.Length; i++)
        {
            modifierImages[i].color = i == index ? _activeColor : _inactiveColor;
        }
    
        valueModifier = modiferValues[index];
    }
    
    public void SaveValue()
    {
        _technique.SaveValue(value);
        _updateTechniques.UpdateItem(new Technique(_technique.GetName, value));
        CanvasGroupSettings(false);
        _technique = null;
    }

    public void CancelChange()
    {
        CanvasGroupSettings(false);
        _technique = null;
    }
    
    private void CanvasGroupSettings(bool active)
    {
        _canvasGroup.alpha = active ? 1 : 0;
        _canvasGroup.interactable = active;
        _canvasGroup.blocksRaycasts = active;
    }
}
