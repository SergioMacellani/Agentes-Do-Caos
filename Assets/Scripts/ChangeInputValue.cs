using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeInputValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TMP_InputField valueText;
    [SerializeField] private Image[] modifierImages;
    
    private Color _activeColor = new Color(1,1,1,.25f);
    private Color _inactiveColor = new Color(.35f,.35f,.35f,.25f);
    
    private float value;
    private bool isFloat = true;
    private float[] modiferValues = { .25f, .5f, 1, 5, 10 };
    private float valueModifier = 1;
    
    private InputFieldLagFix _input;
    private CanvasGroup _canvasGroup => GetComponent<CanvasGroup>();

    public virtual void OpenChangeInput(InputFieldLagFix input, bool isFloat = true, string title = "")
    {
        _input = input;
        this.isFloat = isFloat;
        CanvasGroupSettings(true);
        valueText.text = input.GetInputText;
        value = float.Parse(input.GetInputText);
        ChangeModifier(2);
        titleText.text = $"MODIFICAR VALOR{(title != "" ? $" DE {title}" : $"")}";
        
        modifierImages[0].GetComponent<Button>().interactable = isFloat;
        modifierImages[1].GetComponent<Button>().interactable = isFloat;
    }
    
    public void ChangeValue(float val)
    {
        value += val * valueModifier;
        valueText.text = value.ToString(isFloat ? "0.##" : "F0");
    }
    
    public void ChangeValue(string val)
    {
        value = float.Parse(val);
        valueText.text = value.ToString(isFloat ? "0.##" : "F0");
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
        _input.SaveValue(valueText.text);
        CanvasGroupSettings(false);
        _input = null;
    }

    public void CancelChange()
    {
        CanvasGroupSettings(false);
        _input = null;
    }
    
    private void CanvasGroupSettings(bool active)
    {
        _canvasGroup.alpha = active ? 1 : 0;
        _canvasGroup.interactable = active;
        _canvasGroup.blocksRaycasts = active;
    }
}