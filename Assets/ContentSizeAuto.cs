using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    [AddComponentMenu("Layout/Content Size Auto")]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [Icon("Packages/com.unity.2d.animation/Editor/Assets/SkinningModule/Icons/Selected/Weight_Slider@4x.png")]
    [HelpURL("http://sergiom.dev/rpg/ficha")]
    public class ContentSizeAuto : MonoBehaviour
    {
        [SerializeField] protected bool m_AutoScaleWidth = false;
        [SerializeField] protected bool m_AutoScaleHeight = false;
        
        [Space(5f)]
        [SerializeField] protected bool m_IgnoreDisableCells = false;

        protected int m_ContentCells
        {
            get
            {
                if (IgnoreDisableCells)
                {
                    int cells = 0;
                    foreach (Transform child in transform)
                    {
                        if (child.gameObject.activeSelf)
                            cells++;
                    }

                    return cells;
                }
                else
                {
                    return transform.childCount;
                }
            }
        }
        protected Vector2 m_ScreenSize => new Vector2(rectCanvas.sizeDelta.x, rectCanvas.sizeDelta.y);

        public bool AutoScaleWidth
        {
            get { return m_AutoScaleWidth; }
            set { SetProperty(ref m_AutoScaleWidth, value); }
        }

        public bool AutoScaleHeight
        {
            get { return m_AutoScaleHeight; }
            set { SetProperty(ref m_AutoScaleHeight, value); }
        }
        
        public bool IgnoreDisableCells
        {
            get { return m_IgnoreDisableCells; }
            set { SetProperty(ref m_IgnoreDisableCells, value); }
        }
        
        [System.NonSerialized] private RectTransform m_Rect;
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }
        
        [System.NonSerialized] private Canvas m_Canvas;
        private Canvas mainCanvas
        {
            get
            {
                if (m_Canvas == null)
                    m_Canvas = GetComponentInParent<Canvas>().rootCanvas;
                return m_Canvas;
            }
        }
        
        [System.NonSerialized] private RectTransform m_RectCanvas;
        private RectTransform rectCanvas
        {
            get
            {
                if (m_RectCanvas == null)
                    m_RectCanvas = mainCanvas.GetComponent<RectTransform>();
                return m_RectCanvas;
            }
        }

        protected void OnRectTransformDimensionsChange()
        {
            AutoSizeGroup();
            SetDirty();
        }

        protected void AutoSizeGroup()
        {
            AutoSizeWidth();
            AutoSizeHeight();
        }

        protected void AutoSizeWidth()
        {
            if (AutoScaleWidth)
            {
                rectTransform.sizeDelta = new Vector2((m_ScreenSize.x)*(m_ContentCells-1), rectTransform.sizeDelta.y);
            }
        }
        
        protected void AutoSizeHeight()
        {
            if (AutoScaleHeight)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (m_ScreenSize.y)*(m_ContentCells-1));
            }
        }

        protected void SetProperty<T>(ref T currentValue, T newValue)
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return;
            currentValue = newValue;
            SetDirty();
        }
        
        protected void SetDirty()
        {
            if (!IsActive())
                return;
            
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
        
        public virtual bool IsActive()
        {
            return isActiveAndEnabled;
        }

        private void OnValidate()
        {
            AutoSizeGroup();
        }
    }
}
