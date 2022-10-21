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
    
    private AudioClip musicClip;
    
    public void SetMusicInfo(int order, Sprite cover, string name, bool isPlaying)
    {
        musicOrder.text = order.ToString();
        musicCover.sprite = cover;
        musicName.text = name;
        musicStatus.gameObject.SetActive(isPlaying);
    }
}

[System.Serializable]
public class MusicData
{
    public string musicName;
    public Sprite musicCover;
    public AudioClip musicClip;
    
    public MusicData(string name, Sprite cover, AudioClip clip)
    {
        musicName = name;
        musicCover = cover;
        musicClip = clip;
    }
}
