using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DocumentManager : MonoBehaviour
{
    [SerializeField] 
    private PlayerSheetData pSheet;
    
    [SerializeField]
    private DeleteDocumentImage deleteDocumentImage;
    
    [SerializeField]
    private DocumentImage documentImagePrefab;
    
    [SerializeField]
    private RectTransform imageArea;

    [SerializeField]
    private DocumentItem documentItemPrefab;
    
    [SerializeField]
    private RectTransform documentArea;
    
    private DocumentImage currentImage;
    
    private List<DocumentItem> documentItems = new List<DocumentItem>();

    public DocumentImage CurrentImage
    {
        get { return currentImage; }
        set { currentImage = value; }
    }

    private void Start()
    {
        foreach (var docs in pSheet.documents)
        {
            DocumentItem newDoc = Instantiate(documentItemPrefab, documentArea);
            newDoc.SetDocumentData(new DocumentData(docs, false), this);
            documentItems.Add(newDoc);
        }
    }

    public void AddDocumentToList(DocumentData document)
    {
        if (pSheet.documents.Find(x => x.Code == document.Code) == null)
        {
            DocumentItem newDoc = Instantiate(documentItemPrefab, documentArea);
            newDoc.SetDocumentData(document, this);
            pSheet.documents.Add(new PlayerDocument(document));
            documentItems.Add(newDoc);
        }
        else
        {
            pSheet.documents.Find(x => x.Code == document.Code).Name = document.Name;
            documentItems.Find(x => x.documentData.Code == document.Code).SetDocumentData(document, this);
        }

        AddDocument(document);
    }

    public void AddDocument(DocumentData document)
    {
        DocumentImage newImage = Instantiate(documentImagePrefab, imageArea);
        newImage.SetDocument(document, this);
        currentImage = newImage;
    }

    public void CloseImage()
    {
        if(currentImage != null)
            currentImage.CloseImage();
    }
    
    public void ImageSize(bool increase)
    {
        if(currentImage != null)
            currentImage.ChangeSize(increase);
    }
    
    public void OpenDeleteImage(DocumentItem item)
    {
        deleteDocumentImage.OpenDeleteDocument(item);
    }

    public void DeleteNormal(PlayerDocument document)
    {
        pSheet.documents.Remove(pSheet.documents.Find(x => x.Code == document.Code));
    }
    
    public void DeleteHard(PlayerDocument document)
    {
        pSheet.documents.Remove(pSheet.documents.Find(x => x.Code == document.Code));

        SaveLoadSystem.DeleteFIle(document.Code, "png", "Documents/");
    }
}
