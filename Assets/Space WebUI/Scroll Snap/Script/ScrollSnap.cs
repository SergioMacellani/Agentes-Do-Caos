using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Icon("Assets/Space WebUI/Scroll Snap/Images/scrollSnap_icon.png")]
[AddComponentMenu("UI/Scroll Snap", 37)]
public class ScrollSnap : ScrollRect
{
    public float snapVelocity = .1f;
    public float distanceDivisor = 1.5f;
    public float sizeLimit = 0.25f;
    
    private int centerIten = 0;
    private float[] _pagePositions = new float[]{};
    private Canvas _canvas => GetComponentInParent<Canvas>();
    private RectTransform _rectTransform => GetComponent<RectTransform>();

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        ItensDistanceSize();
    }
    
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        UpdateSnap();
        content.GetChild(centerIten).GetComponent<Button>().interactable = true;
    }
    
    public void UpdateSnap()
    {
        CalculatePagePositions();
        ContentItensDistance();
        ItensDistanceSize();
    }
    
    private void CalculatePagePositions()
    {
        if (_pagePositions.Length == content.childCount) return;
            
        _pagePositions = new float[content.childCount];
        float parts = 1/(float)(_pagePositions.Length-1);
            
        for (int i = 0; i < _pagePositions.Length; i++)
        {
            _pagePositions[i] = parts * i;
        }
    }
    private void ContentItensDistance()
    {
        int i = 0;
        int shorterDistance = int.MaxValue;
        
        foreach (Transform child in content.transform)
        {
            if(shorterDistance > (int)Vector2.Distance(child.position, transform.position))
            {
                shorterDistance = (int)Vector2.Distance(child.position, transform.position);
                centerIten = i;
            }
            
            child.GetComponent<Button>().interactable = false;
            i++;
        }
        
        if (horizontalScrollbar != null) SnapSync(horizontalScrollbar, _pagePositions[centerIten]);
        if (verticalScrollbar != null) SnapSync(verticalScrollbar,_pagePositions[centerIten]);
    }

    private void ItensDistanceSize()
    {
        float width = _canvas.pixelRect.width/2;
        float widthDiference = 960/width;
        foreach (Transform child in content.transform)
        {
            float distance = (int)Vector2.Distance(child.position, transform.position);
            float sizeValue = Mathf.Clamp((1 - ((distance*widthDiference)/(960/distanceDivisor))),sizeLimit,1);
            child.localScale = Vector3.one * sizeValue;
        }
    }

    private void SnapSync(Scrollbar scroll, float targetValue)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) scroll.value = targetValue;
        else StartCoroutine(SnapAsync(scroll, targetValue));
#else
        StartCoroutine(SnapAsync(scroll, targetValue));
#endif
    }
    
#if UNITY_EDITOR
    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        if (!Application.isPlaying) UpdateSnap();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        if (!Application.isPlaying) UpdateSnap();
        else ItensDistanceSize();
    }
#endif

    private IEnumerator SnapAsync(Scrollbar scroll, float targetValue, bool ignoreTimeScale = true)
    {
        float elapsedTime = 0f;
        float startValue = scroll.value;
        while (elapsedTime < snapVelocity)
        {
            scroll.value = Mathf.Lerp(startValue, targetValue, (elapsedTime / snapVelocity));
            elapsedTime += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
            ItensDistanceSize();
            yield return null;
        }
        scroll.value = targetValue;
        ItensDistanceSize();
    }
}
