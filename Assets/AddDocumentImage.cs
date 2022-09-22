using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AddDocumentImage : MonoBehaviour
{
    private Sprite downloadedSprite;
    [SerializeField]
    private DocumentManager documentManager;
    
    [SerializeField]
    private GameObject _addDocumentImage;
    [SerializeField]
    private GameObject _downloadingDocumentImage;    
    [SerializeField]
    private GameObject _previewDocumentImage;
    
    [SerializeField]
    private Image _previewImage;
    [SerializeField]
    private Sprite _errorImage;

    private TMP_InputField _documentKey => _addDocumentImage.GetComponentInChildren<TMP_InputField>();
    private TextMeshProUGUI _downloadPercentage => _downloadingDocumentImage.GetComponentInChildren<TextMeshProUGUI>();
    private TMP_InputField _documenName => _previewDocumentImage.GetComponentInChildren<TMP_InputField>();
    
    public void AddImage()
    {
        StartCoroutine(DownloadImage(_documentKey.text));
        _addDocumentImage.SetActive(false);
        _downloadingDocumentImage.SetActive(true);
    }

    public void AddDocument()
    {
        if (downloadedSprite == _errorImage)
        {
            CancelAdd();
            return;    
        }
        
        if(_documenName.text == "") documentManager.AddDocumentToList(new DocumentData(_documentKey.text, downloadedSprite));
        else documentManager.AddDocumentToList(new DocumentData(_documenName.text, _documentKey.text, downloadedSprite));
        
        SaveLoadSystem.SaveFile(downloadedSprite.texture.EncodeToPNG(), _documentKey.text, "png", "Documents/");
        
        CloseAddDocumentImage();
    }

    public void CancelAdd()
    {
        CloseAddDocumentImage();
    }

    private void DownloadComplete()
    {
        _previewImage.sprite = downloadedSprite;
        _downloadingDocumentImage.SetActive(false);
        _previewDocumentImage.SetActive(true);
    }
    
    private void CloseAddDocumentImage()
    {
        _previewDocumentImage.SetActive(false);
        _addDocumentImage.SetActive(true);
        _documentKey.text = "";
        _documenName.text = "";
        gameObject.SetActive(false);
    }

    private IEnumerator DownloadImage(string documentKey)
    {
        UnityWebRequest www = new UnityWebRequest($"https://sergiom.dev/rpg/documents/{documentKey}.png");
        www.downloadHandler = new DownloadHandlerTexture();
        var webRequest = www.SendWebRequest();
        while (!webRequest.webRequest.isDone)
        {
            _downloadPercentage.text = www.downloadProgress.ToString("P0");
            yield return new WaitForSeconds(.2f);
        }

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            downloadedSprite = _errorImage;
            _documenName.text = "Error 404";
            DownloadComplete();
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            downloadedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            DownloadComplete();
        }
    }
}
