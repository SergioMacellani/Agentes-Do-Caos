using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] 
    private AlbumInfo albumPrefab;
    [SerializeField]
    private RectTransform albumContainer;
    
    public async void SearchAlbum()
    {
        var path = SaveLoadSystem.OpenFolderExplorer();
        if (path == null) return;

        AlbumData data = new AlbumData("","", null, 0);
        data.albumShortName = Path.GetFileName(path);
        var files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            if (file.EndsWith(".png") || file.EndsWith(".jpg"))
            {
                data.albumCover = SaveLoadSystem.LoadImage("", file, false);
            }
            else if (file.EndsWith(".wav") || file.EndsWith(".mp3"))
            {
                data.musicDataList.Add(new MusicData(Path.GetFileName(file), data.albumCover, await SaveLoadSystem.LoadAudio("", file, false)));
            }
        }
        
        Instantiate(albumPrefab, albumContainer).SetAlbum(data);
        GetComponent<AudioSource>().clip = data.musicDataList[0].musicClip;
        GetComponent<AudioSource>().Play();
    }
}
