using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DocumentItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private DocumentManager documentManager;
    
    public DocumentData documentData;
    public TextMeshProUGUI titleText;
    
    private float ClickTime;
    
    public void SetDocumentData(DocumentData data, DocumentManager dm)
    {
        documentData = data;
        titleText.text = data.Name;
        documentManager = dm;
    }
    
    public void AddDocument()
    {
        if(documentData.Image != null) documentManager.AddDocument(documentData);
        else
        {
            documentData.ImportImage();
            documentManager.AddDocument(documentData);
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        ClickTime = Time.realtimeSinceStartup;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (ClickTime + .35f < Time.realtimeSinceStartup)
        {
            if(documentData.Image == null) documentData.ImportImage();
            documentManager.OpenDeleteImage(this);
        }
        else
        {
            AddDocument();
        }
    }
}
