using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatesButton : Selectable, IPointerClickHandler, ISubmitHandler
{
    [SerializeField] private int state = 0;
    
    [SerializeField] private Image stateTargetGraphic;
    [SerializeField] private List<Sprite> statesSprites = new List<Sprite>();
    
    public List<UnityEvent<int>> onClicks;
    
    public void SetValue(bool value)
    {
        SetValue(value ? 0 : 1);
    }
    public void SetValue(int value)
    {
        state = value;
        stateTargetGraphic.sprite = statesSprites[state];
    }
    
    private void Press()
    {
        if (!IsActive() || !IsInteractable()) return;
        
        onClicks[state].Invoke(state);

        state++;
        if (state >= onClicks.Count) state = 0;
        
        ChangeButton();
    }

    private void ChangeButton()
    {
        if(targetGraphic == null) return;
        stateTargetGraphic.sprite = statesSprites[state];
    }
    
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        
        if(onClicks == null) onClicks = new List<UnityEvent<int>>(statesSprites.Count);
        else if(onClicks.Count < statesSprites.Count) onClicks.AddRange(new UnityEvent<int>[statesSprites.Count - onClicks.Count]);
        else if(onClicks.Count > statesSprites.Count) onClicks.RemoveRange(statesSprites.Count, onClicks.Count - statesSprites.Count);
        state = Mathf.Clamp(state, 0, statesSprites.Count - 1);
        SetValue(state);
        
    }
#endif

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        Press();
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        Press();

        // if we get set disabled during the press
        // don't run the coroutine.
        if (!IsActive() || !IsInteractable())
            return;

        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }

    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }
}
