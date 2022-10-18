using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Icon("Assets/Space WebUI/Mobile Toggle/Images/toggleScript_icon.png")]
[HelpURL("http://sergiom.dev/rpg/ficha")]

[AddComponentMenu("UI/Mobile Toggle", 30)]
[RequireComponent(typeof(RectTransform))]
public class MobileToggle : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement
{
    #region Private Fields
    
    [Space]
    [Header("Is On Control")]
    [Tooltip("Is the toggle currently on or off?")]
    [SerializeField]
    private bool m_IsOn;
    
    [Space(5)]
    [Tooltip("Will Toggle have color animation?")]
    [SerializeField]
    private bool m_AnimateColor = true;
    [Tooltip("How fast the color animation will be?")]
    [SerializeField] [Range(0,1)] 
    private float m_ColorAnimationSpeed = 0.1f;
    
    [SerializeField]
    private Color m_OnColor = new Color(0,1,0.445529f,1);
    [SerializeField]
    private Color m_OffColor = Color.white;
    
    [Header("Animation Control")]
    [Tooltip("Will Toggle have animation?")]
    [SerializeField]
    private bool m_AnimateToggle = true;
    
    [SerializeField]
    private ToggleAnimation m_ToggleAnimation;
    
    [Header("Text Control")]
    [Tooltip("Will Toggle have animation?")]
    [SerializeField]
    private bool m_HaveText = true;
    
    [SerializeField]
    private TextOption m_ToggleText;

    [Header("Graphic Targets")]
    [SerializeField]
    private Image m_ToggleBackground;
    [SerializeField]
    private RectTransform m_ToggleCircle;
    
    [Space(20)]
    [SerializeField]
    private Toggle.ToggleEvent onValueChanged = new Toggle.ToggleEvent();

    private Animator m_animator => GetComponent<Animator>();
    private RectTransform m_backgroundRect => m_ToggleBackground.rectTransform;
    
    #endregion

    #region Public Fields
    public bool isOn
    {
        get => m_IsOn;
        set => Set(value);
    }
    public void Set(bool value)
    {
        if (m_IsOn == value) return;

        m_IsOn = value;
        
        onValueChanged.Invoke(m_IsOn);
        UpdateGraphic();
    }

    public string text
    {
        get => m_ToggleText.text;
        set => m_ToggleText.text = value;
    }
    
    public Color onColor
    {
        get => m_OnColor;
        set
        {
            m_OnColor = value;
            UpdateGraphic();
        }
    }

    #endregion

    #region Private Void Functions

    private void InternalMobileToggle()
    {
        if (!IsActive() || !IsInteractable())
            return;

        isOn = !isOn;
    }

    private void UpdateGraphic()
    {
        AnimateToggle();
        ColorToggle();
    }
    
    private void AnimateToggle()
    {
        if (m_AnimateToggle)
        {
            try
            {
                m_animator.Play(m_ToggleAnimation.GetAnimation(m_IsOn));
                m_animator.speed = m_ToggleAnimation.speed;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                
                m_AnimateToggle = false;
                TransformToggle();
            }
        }
        else
        {
            TransformToggle();
        }
    }

    private void TransformToggle()
    {
        float x = ((m_backgroundRect.sizeDelta.x - m_ToggleCircle.sizeDelta.x) / 2 - 2.5f) * (m_IsOn ? 1 : -1);
        
        m_ToggleCircle.anchoredPosition = new Vector2(x, m_ToggleCircle.anchoredPosition.y);
    }
    
    private void ColorToggle(bool ignoreAnimation = false)
    {
        if (m_AnimateColor && !ignoreAnimation && Application.isPlaying)
            StartCoroutine(CrossFadeColorAsync(m_IsOn ? m_OnColor : m_OffColor));
        else
            m_ToggleBackground.color = m_IsOn ? m_OnColor : m_OffColor;
    }

    private void TextToggle()
    {
        if (!m_HaveText) return;
        
        m_ToggleText.UpdateText();
    }
    
    protected void OnValidate()
    {
        TextToggle();
        ColorToggle(true);
        
        FixRectTransform();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        
        FixRectTransform();
    }
    
    private void FixRectTransform()
    {
#if UNITY_EDITOR
        m_ToggleCircle.sizeDelta = new Vector2(m_backgroundRect.rect.height - 5, m_ToggleCircle.sizeDelta.y);
        m_ToggleBackground.pixelsPerUnitMultiplier = (60 * 4) / m_backgroundRect.rect.height;
        TransformToggle();
#endif
    }

    #endregion

    #region IPointerClickHandler & ISubmitHandler implementation

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        InternalMobileToggle();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        InternalMobileToggle();
    }

    #endregion

    #region ICanevasElement implementation

    public virtual void Rebuild(CanvasUpdate executing)
    {
        FixRectTransform();
    }

    public virtual void LayoutComplete()
    {}

    public virtual void GraphicUpdateComplete()
    {}

    #endregion
    
    private IEnumerator CrossFadeColorAsync(Color targetColor, bool ignoreTimeScale = true)
    {
        float elapsedTime = 0f;
        Color startColor = m_ToggleBackground.color;
        while (elapsedTime < m_ColorAnimationSpeed)
        {
            m_ToggleBackground.color = Color.Lerp(startColor, targetColor, (elapsedTime / m_ColorAnimationSpeed));
            elapsedTime += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
            yield return null;
        }
        m_ToggleBackground.color = targetColor;
    }
}

    #region Classes
[System.Serializable]
public class ToggleAnimation
{
    [SerializeField]
    private string m_OnAnimationClip;
    [SerializeField]
    private string m_OffAnimationClip;
    
    [SerializeField]
    private float m_AnimationSpeed = 1f;
    
    public float speed => m_AnimationSpeed;
    public string GetAnimation(bool isOn)
    {
        return isOn ? m_OnAnimationClip : m_OffAnimationClip;
    }
}

[System.Serializable]
public class TextOption
{
    [SerializeField]
    private TMP_Text m_TargetText;
    [SerializeField]
    private string m_Text;
    
    public string text
    {
        get => m_Text;
        set
        {
            m_TargetText.text = value;
            m_Text = value;
        }
    }
    
    public void UpdateText() => m_TargetText.text = m_Text; 
}
    #endregion