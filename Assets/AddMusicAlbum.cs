using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AddMusicAlbum : MonoBehaviour
{
    [SerializeField]
    private WindowManager windowManager;
    
    [SerializeField] 
    private TMP_InputField _inputLink;
    [SerializeField]
    private TMP_InputField _inputName;
    [SerializeField]
    private TMP_InputField _inputShortName;

    [SerializeField] 
    private Image _albumCover;
    [SerializeField]
    private Sprite _defaultAlbumCover;
    [SerializeField] 
    private TextMeshProUGUI _albumMusicValues;

    [SerializeField] 
    private TextMeshProUGUI _loadingText;
    
    [SerializeField]
    private MusicManager musicManager;
    
    private AlbumData _albumData;

    private void OnEnable()
    {
        _albumData = null;
        _inputLink.text = "";
        _inputName.text = "";
        _inputShortName.text = "";
        _albumCover.sprite = _defaultAlbumCover;
        _albumMusicValues.text = "";
    }

    public async void AlbumFromDisk()
    {
        var path = SaveLoadSystem.OpenFolderExplorer("Album Folder", new string[2]{"mp3", "wav"});
        if (path == null) return;

        await SetAlbumInfo(path);
    }
    
    public async void AlbumFromLink()
    {
        var link = _inputLink.text;
        if (link == null) return;

        DownloadAlbum(link);
    }
    
    private async void DownloadAlbum(string link)
    {
        StartCoroutine(AJAX.GetData("", _loadingText, async data =>
        {
            if(data == null) return;
            
            SaveLoadSystem.SaveFile(data,"albumtemp", "zip", "temp/");
            ZipFile.ExtractToDirectory(SaveLoadSystem.path + "temp/albumtemp.zip", SaveLoadSystem.path + $"music/album/temp");
            await SetAlbumInfo(SaveLoadSystem.path + $"music/album/temp");
        }, link));
        windowManager.SwitchMenu(2);
    }

    private async Task SetAlbumInfo(string path)
    {
        _albumData = await CreateAlbumItem(path);
        windowManager.SwitchMenu(3);
        
        File.Delete(SaveLoadSystem.path + "temp/albumtemp.zip");
        Directory.Delete(SaveLoadSystem.path + "temp");
        _albumCover.sprite = _albumData.albumCover;
        _albumMusicValues.text = _albumData.musicDataList.Count + " Musicas";
    }

    public async void AddAlbum()
    {
        if(_albumData == null) return;
        
        _albumData.albumLongName = _inputName.text;
        _albumData.albumShortName = _inputShortName.text == "" ? _inputName.text : _inputShortName.text;
        _albumData.albumCover = _albumCover.sprite;
        _albumData.albumLink = SaveLoadSystem.path + $"music/album/{_albumData.albumShortName}/";
        
        SaveLoadSystem.SaveFile(JsonUtility.ToJson(_albumData, true), _albumData.albumShortName, "acmp", "music/info/");
        Directory.Move(SaveLoadSystem.path + $"music/album/temp", _albumData.albumLink);
        SaveLoadSystem.SaveFile(_albumCover.sprite.texture.EncodeToPNG(), "cover", "png", $"music/album/{_albumData.albumShortName}/");

        await musicManager.CreateAlbumItem(_albumData.albumLink, _albumData);
        gameObject.SetActive(false);
    }

    public void ChangeCover()
    {
        var path = SaveLoadSystem.OpenFileExplorer("Album Cover", new string[]{"png","jpg","jpeg"});
        _albumCover.sprite = SaveLoadSystem.LoadImage(path);
    }
    
    private async Task<AlbumData> CreateAlbumItem(string path)
    {
        var data = new AlbumData(Path.GetFileName(path), "", null, 0, path);
        
        List<string> files = Directory.GetFiles(path).ToList();
        
        var find = files.Find(x => x.Contains(".jpg") || x.Contains(".png"));
        data.albumCover = find != null ? SaveLoadSystem.LoadImage("", find, false) : _defaultAlbumCover;
        
        Texture2D texture = Resize(data.albumCover.texture, 4, 4);
        data.albumColor = texture.GetPixel(Random.Range(0, texture.height),Random.Range(0, texture.height));
        
        data.musicDataList = new List<MusicData>();
        
        var i = 0;
        foreach (var file in files.Where(file => file.EndsWith(".wav") || file.EndsWith(".mp3")))
        {
            var music = new MusicData(Path.GetFileNameWithoutExtension(file), data.albumCover, await SaveLoadSystem.LoadAudio("", file, false), i + 1);
            data.musicDataList.Add(music);
            i++;
        }
        
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
}
