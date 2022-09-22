using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DocumentImage : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField]
    private DocumentManager _documentManager;
    private RectTransform _rectTransform => GetComponent<RectTransform>();
    private Sprite _sprite => GetComponent<Image>().sprite;

    private void OnEnable()
    {
        ImageSize();
    }

    private void ImageSize()
    {
        float aspectRatio = _sprite.rect.width / _sprite.rect.height;
        
        if(_rectTransform.sizeDelta.x > _rectTransform.sizeDelta.y)
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.x / aspectRatio);
        else
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.y * aspectRatio, _rectTransform.sizeDelta.y);
    }
    
    public void SetDocument(DocumentData data, DocumentManager dm)
    {
        GetComponent<Image>().sprite = data.Image;
        gameObject.name = data.Name;
        _documentManager = dm;
        ImageSize();
    }
    
    public void ChangeSize(bool increase)
    {
        float increaseValue = increase ? 0.1f : -0.1f;
        if(_rectTransform.localScale.x + increaseValue > 0)
            _rectTransform.localScale += new Vector3(increaseValue,increaseValue, 0);
    }
    
    public void CloseImage()
    {
        Destroy(gameObject);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _documentManager.CurrentImage = this;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(!Application.isPlaying) ImageSize();
    }
#endif
}

[System.Serializable]
public class DocumentData
{
    public string Name;
    public string Code;
    public Sprite Image;
    
    public DocumentData(string name, string code, Sprite image)
    {
        Name = name;
        Code = code;
        Image = image;
    }
    
    public DocumentData(string code, Sprite image)
    {
        Name = "Document " + code;
        Code = code;
        Image = image;
    }
    
    public DocumentData(PlayerDocument data, bool importImage = true)
    {
        Name = data.Name;
        Code = data.Code;
        if(importImage) Image = SaveLoadSystem.LoadImage(data.Code + ".png", "Documents/");
    }

    public void ImportImage()
    {
        Image = SaveLoadSystem.LoadImage(Code + ".png", "Documents/");
    }
}

