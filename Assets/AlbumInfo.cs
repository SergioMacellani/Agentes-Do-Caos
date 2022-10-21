using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumInfo : MonoBehaviour
{
    [SerializeField]
    private AlbumData data;
    
    [SerializeField]
    private Image albumCover;
    [SerializeField]
    private TextMeshProUGUI albumName;
    [SerializeField]
    private Image albumStatus;

    private int albumStatusIndex = 0;
    private Color[] albumStatusColors = new Color[3] { Color.red, Color.yellow, Color.green };
    private MusicManager musicManager;

    public void SelectAlbum()
    {
        musicManager.SelectAlbum(data);
    }
    
    public void SetAlbum(AlbumData data, MusicManager mm)
    {
        musicManager = mm;
        this.data = data;
        SetAlbumName(this.data.albumShortName);
        SetAlbumCover(this.data.albumCover);
        SetAlbumStatus(this.data.albumStatus);
    }
    
    public void SetAlbumName(string albumName) => this.albumName.text = albumName;
    public void SetAlbumCover(Sprite sprite) => albumCover.sprite = sprite;
    public void SetAlbumStatus(int status)
    { 
        albumStatusIndex = status;
        albumStatus.color = albumStatusColors[albumStatusIndex];
    }
}

[System.Serializable]
public class AlbumData
{
    public string albumShortName;
    public string albumLongName;
    public Sprite albumCover;
    public int albumStatus;
    
    public List<MusicData> musicDataList = new List<MusicData>();
    
    public AlbumData(string albumShortName, string albumLongName, Sprite albumCover, int albumStatus)
    {
        this.albumShortName = albumShortName;
        this.albumLongName = albumLongName;
        this.albumCover = albumCover;
        this.albumStatus = albumStatus;
    }
}
