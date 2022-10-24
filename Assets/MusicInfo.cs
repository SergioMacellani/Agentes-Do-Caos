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
    private StatesButton musicStatus;
    
    private MusicData data;
    private MusicManager musicManager;
    private bool shuffle = false;
    private int shuffleOrder = 0;
    
    public AudioClip GetAudioClip => data.musicClip;
    public string GetMusicName => data.musicName;
    public int GetMusicOrder => shuffle ? shuffleOrder : data.musicOrder;
    public int GetRealMusicOrder => data.musicOrder;
    
    public void SetMusicInfo(MusicData musicData, MusicManager mm, bool first = false)
    {
        musicManager = mm;
        data = musicData;
        musicOrder.text = data.musicOrder.ToString();
        musicCover.sprite = data.musicCover;
        musicName.text = data.musicName;
        
        if(first) musicManager.PlayMusic(this, false);
    }
    
    public void SetShuffle(int order)
    {
        shuffle = true;
        shuffleOrder = order;
        musicOrder.text = shuffleOrder.ToString();
    }
    
    public void ResetOrder()
    {
        shuffle = false;
        shuffleOrder = 0;
        musicOrder.text = data.musicOrder.ToString();
    }

    public void PlayMusic()
    {
        musicManager.PlayMusic(this);
    }
    
    public void PauseMusic()
    {
        musicManager.PauseMusic();
    }

    public void SetStatus(bool status)
    {
        musicStatus.SetValue(status);
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
