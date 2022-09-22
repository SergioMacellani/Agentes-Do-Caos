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
    
    private bool isOpen = false;

    private void Awake()
    {
        TryGetComponent(out rectTransform);
    }

    private void OnEnable()
    {
        
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

        if (dragDistance <= -60 && !isOpen)
        {
            isOpen = true;
            animator.SetBool("isOpen", true);
        }
        else if (dragDistance >= 60)
        {
            if (isOpen != animator.GetBool("isOpen")) animator.Play("FichaMenuClose");

            animator.SetBool("isOpen", false);
            isOpen = false;
        }
        else
        {
            rectTransform.anchoredPosition = rectTransformPos;
        }

        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        Debug.Log(dragDistance);
    }
}
