using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class FileManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;
    [SerializeField]
    private string fileExtension = "csv";

    [SerializeField] 
    private FileManagerEvent onFileLoaded;
    
    private string path;
    
    [System.Serializable]
    public class FileManagerEvent : UnityEvent<string>
    {}
    
    public void OpenFileExplorer()
    {
        path = EditorUtility.OpenFilePanel($"Selecione a sua ficha (.{fileExtension})", "", fileExtension);
        
        if (path.Length != 0)
        {
            onFileLoaded.Invoke(path);
        }
    }
}
