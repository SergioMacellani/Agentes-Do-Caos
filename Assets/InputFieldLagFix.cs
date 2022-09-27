using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldLagFix : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_InputField _inputField;
    public void Start()
    {
        TryGetComponent(out _inputField);
        _inputField.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _inputField.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inputField.enabled = false;
    }
}
