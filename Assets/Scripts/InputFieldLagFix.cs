using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldLagFix : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private bool isString = false;
    [SerializeField]
    private bool isFloat = true;
    [SerializeField]
    private string title = "";
    private TMP_InputField _inputField;
    
    public string GetInputText => _inputField.text;
    
    public void Start()
    {
        TryGetComponent(out _inputField);
        _inputField.enabled = false;
    }

    public void SaveValue(string value)
    {
        _inputField.text = value;
        _inputField.onEndEdit.Invoke(value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isString)
            _inputField.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isString)
            _inputField.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isString)
            GameObject.FindWithTag("ChangeValue").GetComponent<ChangeInputValue>().OpenChangeInput(this, isFloat, title);
    }
}
