using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    [Header("Album")]
    [SerializeField] 
    private AlbumInfo albumPrefab;
    [SerializeField]
    private RectTransform albumContainer;
    
    [Header("Music")]
    [SerializeField] 
    private MusicInfo musicPrefab;
    [SerializeField]
    private RectTransform musicContainer;
    private MusicInfo selectedMusic;
    
    [Header("Midia Control")]
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private TextMeshProUGUI musicTime;
    [SerializeField]
    private TextMeshProUGUI musicDuration;
    [SerializeField]
    private StatesButton playButton;

    [Header("Playing")]
    [SerializeField]
    private TextMeshProUGUI playingMusicAlbum;
    [SerializeField] 
    private Image playingMusicCover;
    [SerializeField]
    private TextMeshProUGUI playingMusicName;
    
    [Header("Art")]
    [SerializeField]
    private Sprite defaultAlbumArt;
    
    [SerializeField]
    private List<MusicInfo> musicInfos = new List<MusicInfo>();

    public TextMeshProUGUI txt;
    
    private AudioSource audioSource;
    private bool isPlaying = false;
    private int repeat = 0;
    private bool shuffle = false;
    
    private List<int> musicIndex = new List<int>();

    private async void Awake()
    {
        TryGetComponent(out audioSource);
        await LoadMusicList();
    }

    private async Task LoadMusicList()
    {
        foreach (Transform a in albumContainer)
        {
            Destroy(a.gameObject);
        }
        
        var directory = SaveLoadSystem.path + "music/info";
        if(!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        
        List<string> files = Directory.GetFiles(directory).ToList();
        if(files.Count == 0) return;

        var first = true;
        int i = 0;
        foreach (var file in files)
        {
            if (!file.EndsWith(".acmp")) return;
            
            SaveLoadSystem.LoadFile(file, out var json);
            var data = JsonUtility.FromJson<AlbumData>(json);
            await CreateAlbumItem(data.albumLink, data, first);
            first = false;

            i++;
        }
    }

    private void FixedUpdate()
    {
        if(!isPlaying) return;
        
        musicSlider.value = audioSource.time;
        musicTime.text = GetTime(audioSource.time);

        if (!(audioSource.time >= audioSource.clip.length - .1f)) return;
        switch (repeat)
        {
            case 2 when selectedMusic.GetMusicOrder < musicInfos.Count:
                NextMusic();
                break;
            case 2:
                audioSource.Pause();
                selectedMusic.SetStatus(true);
                playButton.SetValue(true);
                musicSlider.value = 0;
                break;
            case 0:
                audioSource.time = 0;
                NextMusic();
                break;
            default:
                audioSource.time = 0;
                break;
        }
    }

    public async void SearchAlbum()
    {
        var path = SaveLoadSystem.OpenFolderExplorer("Album Folder", new string[2]{"mp3", "wav"});
        txt.text = path;
        if (path == null) return;

        var data = await CreateAlbumItem(path);

        SaveLoadSystem.SaveFile(JsonUtility.ToJson(data, true), data.albumShortName, "acmp", "music/info/");
    }

    public async Task<AlbumData> CreateAlbumItem(string path, AlbumData data = null, bool first = false)
    {
        data ??= new AlbumData(Path.GetFileName(path), "", null, 0, path);
        
        List<string> files = Directory.GetFiles(path).ToList();
        
        var find = files.Find(x => x.Contains(".jpg") || x.Contains(".png"));
        data.albumCover = find != null ? SaveLoadSystem.LoadImage("", find, false) : defaultAlbumArt;
        
        Texture2D texture = Resize(data.albumCover.texture, 4, 4);
        data.albumColor = texture.GetPixel(Random.Range(0, texture.height),Random.Range(0, texture.height));
        
        data.musicDataList = new List<MusicData>();
        
        var i = 0;
        foreach (var file in files.Where(file => file.EndsWith(".wav") || file.EndsWith(".mp3")))
        {
            var music = new MusicData(Path.GetFileName(file).Replace(Path.GetExtension(file), ""), data.albumCover, await SaveLoadSystem.LoadAudio("", file, false), i + 1);
            data.musicDataList.Add(music);
            i++;
        }

        Instantiate(albumPrefab, albumContainer).SetAlbum(data, this, first);
        return data;
    }
    
    Texture2D Resize(Texture2D texture2D,int targetX,int targetY)
    {
        RenderTexture rt=new RenderTexture(targetX, targetY,24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D,rt);
        Texture2D result=new Texture2D(targetX,targetY);
        result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
        result.filterMode = FilterMode.Point;
        result.Apply();
        return result;
    }

    public void SelectAlbum(AlbumData albumData)
    {
        foreach (Transform msc in musicContainer)
        {
            Destroy(msc.gameObject);
        }
        
        //var first = !isPlaying;
        var first = true;
        foreach (var msc in albumData.musicDataList)
        {
            var music = Instantiate(musicPrefab, musicContainer);
            music.SetMusicInfo(new MusicData(msc.musicName, msc.musicCover, msc.musicClip, msc.musicOrder), this, first);
            musicInfos.Add(music);
            first = false;
        }
        
        //if(isPlaying) return;
        playingMusicCover.sprite = albumData.albumCover;
        playingMusicAlbum.text = albumData.albumShortName;
        musicSlider.fillRect.GetComponent<Image>().color = albumData.albumColor;
    }

    public void PlayMusic(MusicInfo musicInfo, bool play = true)
    {
        if(selectedMusic != null) selectedMusic.SetStatus(true);
        selectedMusic = musicInfo;
        
        playButton.SetValue(!play);
        selectedMusic.SetStatus(!play);
        audioSource.clip = selectedMusic.GetAudioClip;
        musicDuration.text = GetTime(audioSource.clip.length);
        musicSlider.value = 0;
        musicSlider.maxValue = audioSource.clip.length;
        playingMusicName.text = selectedMusic.GetMusicName;
        musicTime.text = "00:00";
        
        if(!play) return;
        audioSource.Play();
        isPlaying = true;
    }
    
    public void PlayMusic()
    {
        selectedMusic.SetStatus(false);
        audioSource.Play();
        isPlaying = true;
    }
    
    public void PauseMusic(bool mediaControl = false)
    {
        if (!mediaControl) playButton.SetValue(true);
        else selectedMusic.SetStatus(true);
        
        audioSource.Pause();
        isPlaying = false;
    }
    
    public void ChangeMusicTime(float time)
    {
        audioSource.time = time;
    }
    
    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void NextMusic()
    {
        PlayMusic(musicInfos[selectedMusic.GetMusicOrder < musicInfos.Count ? selectedMusic.GetMusicOrder : 0]);
    }
    
    public void PreviousMusic()
    {
        var order = selectedMusic.GetMusicOrder - 2;
        PlayMusic(musicInfos[order >= 0 ? order : musicInfos.Count - 1]);
    }
    
    public void SetRepeat(int value)
    {
        repeat = value;
    }
    
    public void SetShuffle(int value)
    {
        if (value == 0)
        {
            shuffle = true;
            musicInfos.Sort(SortShuffle);
            int i = 0;
            foreach (var msc in musicInfos)
            {
                msc.transform.SetSiblingIndex(i);
                msc.SetShuffle(i + 1);
                i++;
            }
        }
        else
        {
            shuffle = false;
            musicInfos.Sort(SortShuffle);
            int i = 0;
            foreach (var msc in musicInfos)
            {
                msc.transform.SetSiblingIndex(i);
                msc.ResetOrder();
                i++;
            }
        }
    }

    private int SortShuffle(MusicInfo a, MusicInfo b)
    {
        if(shuffle) return Random.Range(-2, 3);
        else
        {
            if(a.GetRealMusicOrder > b.GetRealMusicOrder) return 1;
            if(a.GetRealMusicOrder < b.GetRealMusicOrder) return -1;
            return 0;
        }
    }

    private string GetTime(float time)
    {
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
