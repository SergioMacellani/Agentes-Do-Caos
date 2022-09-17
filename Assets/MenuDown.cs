using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuDown : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] 
    private Animator animator;
    
    private Vector2 startPos;
    private Vector2 endPos;
    
    private RectTransform rectTransform;
    private Vector2 rectTransformPos;
    
    private float dragDistance => endPos.y - startPos.y;

    private void Awake()
    {
        TryGetComponent(out rectTransform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.position;
        rectTransformPos = rectTransform.anchoredPosition;
        animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
    }

    public void OnDrag(PointerEventData eventData)
    {
        endPos = eventData.position;
        
        rectTransform.anchoredPosition = rectTransformPos + new Vector2(0, dragDistance);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endPos = eventData.position;

        if (dragDistance <= -60 && !animator.GetBool("isOpen"))
        {
            animator.SetBool("isOpen", true);
        }
        else if (dragDistance >= 60 && animator.GetBool("isOpen"))
        {
            animator.SetBool("isOpen", false);
        }
        else
        {
            rectTransform.anchoredPosition = rectTransformPos;
        }

        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }
}
