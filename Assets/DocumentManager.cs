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
    private DocumentImage documentImagePrefab;
    
    [SerializeField]
    private RectTransform imageArea;
    
    [SerializeField]
    private DocumentImage currentImage;
    
    [SerializeField]
    private DocumentItem documentItemPrefab;
    
    [SerializeField]
    private RectTransform documentArea;

    [SerializeField] private List<DocumentData> documentList = new List<DocumentData>();

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
            newDoc.SetDocumentData(new DocumentData(docs), this);
        }
    }

    public void AddDocumentToList(DocumentData document)
    {
        DocumentItem newDoc = Instantiate(documentItemPrefab, documentArea);
        newDoc.SetDocumentData(document, this);
        pSheet.documents.Add(new PlayerDocument(document));
        
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
        currentImage.CloseImage();
    }
    
    public void ImageSize(bool increase)
    {
        currentImage.ChangeSize(increase);
    }
}
