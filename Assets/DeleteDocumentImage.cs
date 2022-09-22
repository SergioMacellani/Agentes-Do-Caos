using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteDocumentImage : MonoBehaviour
{
    [SerializeField]
    private Image _previewImage;
    
    [SerializeField]
    private DocumentManager _documentManager;
    
    [SerializeField]
    private DocumentData _documentData;
    
    private DocumentItem _documentItem;
    
    public void OpenDeleteDocument(DocumentItem item)
    {
        gameObject.SetActive(true);
        _documentItem = item;
        _documentData = item.documentData;
        _previewImage.sprite = _documentData.Image;
    }
    
    public void CancelDelete()
    {
        gameObject.SetActive(false);
    }
    
    public void DeleteNormal()
    {
        _documentManager.DeleteNormal(new PlayerDocument((_documentData)));
        Destroy(_documentItem.gameObject);
        gameObject.SetActive(false);
    }
    
    public void DeleteHard()
    {
        _documentManager.DeleteHard(new PlayerDocument((_documentData)));
        Destroy(_documentItem.gameObject);
        gameObject.SetActive(false);
    }
}
