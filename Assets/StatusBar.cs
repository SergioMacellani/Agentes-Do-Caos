using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    
    public UnityEvent onValueChange;

    public int GetCurrentValue => int.Parse(_actualValueText.text);
    public int GetMaxValue => int.Parse(_maxValueText.text);
    
    public int GetPercentage => (GetCurrentValue*100)/GetMaxValue;

    private void Awake()
    {
        TryGetComponent(out _animation);
    }
    
    public void SetValue(StatsValue value)
    {
        _actualValueText.text = value.current.ToString();
        _maxValueText.text = value.max.ToString();
        
        _value = (float)GetCurrentValue / (float)GetMaxValue;
        CheckDanger();
        UpdateBar();
    }
    
    public void SetValue()
    {
        _value = (float)GetCurrentValue / (float)GetMaxValue;
        onValueChange.Invoke();
        CheckDanger();
        UpdateBar();
    }
    
    public void SetMaxValue(int value)
    {
        _maxValueText.text = value.ToString();
        
        _value = (float)GetCurrentValue / (float)GetMaxValue;
        CheckDanger();
        UpdateBar();
    }

    public void UpdateArmorLifeBar(StatusBar bar)
    {
        SetMaxValue(bar.GetCurrentValue*3);
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
