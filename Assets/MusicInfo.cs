using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI musicOrder;
    [SerializeField]
    private Image musicCover;
    [SerializeField]
    private TextMeshProUGUI musicName;
    [SerializeField]
    private Image musicStatus;
    
    private MusicData data;
    private MusicManager musicManager;
    
    public void SetMusicInfo(MusicData musicData, MusicManager mm)
    {
        musicManager = mm;
        data = musicData;
        musicOrder.text = data.musicOrder.ToString();
        musicCover.sprite = data.musicCover;
        musicName.text = data.musicName;
    }

    public void PlayMusic()
    {
        musicManager.PlayMusic(data);
    }
}

[System.Serializable]
public class MusicData
{
    public string musicName;
    public Sprite musicCover;
    public AudioClip musicClip;
    public int musicOrder;
    
    public MusicData(string name, Sprite cover, AudioClip clip, int order)
    {
        musicName = name;
        musicCover = cover;
        musicClip = clip;
        musicOrder = order;
    }
}
