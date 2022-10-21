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
    
    [SerializeField] 
    private MusicInfo musicPrefab;
    [SerializeField]
    private RectTransform musicContainer;
    
    [SerializeField]
    private List<MusicInfo> musicInfos = new List<MusicInfo>();

    public async void SearchAlbum()
    {
        var path = SaveLoadSystem.OpenFolderExplorer();
        if (path == null) return;

        AlbumData data = new AlbumData("","", null, 0);
        data.albumShortName = Path.GetFileName(path);
        var files = Directory.GetFiles(path);
        
        var i = 0;
        foreach (var file in files)
        {
            if (file.EndsWith(".png") || file.EndsWith(".jpg"))
            {
                data.albumCover = SaveLoadSystem.LoadImage("", file, false);
            }
            else if (file.EndsWith(".wav") || file.EndsWith(".mp3"))
            {
                data.musicDataList.Add(new MusicData(Path.GetFileName(file), data.albumCover, await SaveLoadSystem.LoadAudio("", file, false), i));
                i++;
            }
        }
        
        Instantiate(albumPrefab, albumContainer).SetAlbum(data, this);
    }
    
    public void SelectAlbum(AlbumData albumData)
    {
        foreach (Transform msc in musicContainer)
        {
            Destroy(msc.gameObject);
        }
        
        foreach (var msc in albumData.musicDataList)
        {
            Instantiate(musicPrefab, musicContainer).SetMusicInfo(new MusicData(msc.musicName, msc.musicCover, msc.musicClip, msc.musicOrder), this);
        }
    }

    public void PlayMusic(MusicData musicData)
    {
        GetComponent<AudioSource>().clip = musicData.musicClip;
        GetComponent<AudioSource>().Play();
    }
}
