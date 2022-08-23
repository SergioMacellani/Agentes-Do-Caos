using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] [Range(0,1)]
    private float _value;
    
    [SerializeField] [Range(0,1)]
    private float _dangerValue;

    [SerializeField] 
    private bool _inverseDanger;

    [SerializeField]
    private BarColors _barColors;
    
    [SerializeField]
    private BarImages _barImages;

    [SerializeField] 
    private string _barName;

    [SerializeField]
    private TMP_InputField _actualValueText;
    [SerializeField]
    private TMP_InputField _maxValueText;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    
    private Animator _animation;

    public int GetCurrentValue => int.Parse(_actualValueText.text);

    private void Awake()
    {
        TryGetComponent(out _animation);
    }
    
    public void SetValue(StatsValue value)
    {
        _actualValueText.text = value.current.ToString();
        _maxValueText.text = value.max.ToString();
        
        _value = float.Parse(_actualValueText.text) / float.Parse(_maxValueText.text);
        CheckDanger();
        UpdateBar();
    }

    public void SetValue()
    {
        _value = float.Parse(_actualValueText.text) / float.Parse(_maxValueText.text);
        CheckDanger();
        UpdateBar();
    }

    private void UpdateBar()
    {
        _barImages.fillImage.fillAmount = _value;
    }

    private void UpdateColors()
    {
        _barImages.fillImage.color = _barColors.FillColor;
        _barImages.backgroundImage.color = _barColors.BackgroundColor;
        _barImages.lineImage.color = _barColors.LineColor;
    }

    private void CheckDanger()
    {
        if (!_inverseDanger)
        {
            if (_value > _dangerValue) _animation.Play("NormalBar");
            else _animation.Play("DangerBar");
        }
        else
        {
            if (_value < _dangerValue) _animation.Play("NormalBar");
            else _animation.Play("DangerBar");
        }
    }
    
    private void UpdateName()
    {
        _nameText.text = _barName;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateBar();
        UpdateColors();
        UpdateName();
    }
#endif
    
}

[System.Serializable]
public struct BarColors
{
    public Color FillColor;
    public Color BackgroundColor;
    public Color LineColor;
}

[System.Serializable]
public struct BarImages
{
    public Image fillImage;
    public Image backgroundImage;
    public Image lineImage;
}
