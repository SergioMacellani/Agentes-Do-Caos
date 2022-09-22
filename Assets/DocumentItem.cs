using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DocumentItem : MonoBehaviour
{
    private DocumentManager documentManager;
    
    public DocumentData documentData;
    public TextMeshProUGUI titleText;
    
    public void SetDocumentData(DocumentData data, DocumentManager dm)
    {
        documentData = data;
        titleText.text = data.Name;
        documentManager = dm;
    }
    
    public void AddDocument()
    {
        documentManager.AddDocument(documentData);
    }
}
