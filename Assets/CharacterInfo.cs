using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private Image portraitImage;
    [SerializeField]
    private Image maskImage;
    
    private readonly PlayerName _playerName = new PlayerName();
    
    public void SetCharacterInfo(PlayerName name, Sprite portrait, Color mask)
    {
        _playerName.SetName(name);
        nameText.text = name.showName;
        portraitImage.sprite = portrait;
        
        Color.RGBToHSV(mask, out var h, out var s, out var v);
        mask = Color.HSVToRGB(h, s, Mathf.Clamp01(v - .30f));
        maskImage.color = new Color(mask.r, mask.g, mask.b, maskImage.color.a);
    }

    public void SelectCharacter()
    {
        SaveLoadSystem.LoadFile("chardata.chaos", out var json, $"characters/{_playerName.dataName}/");
        GameInfo.LoadPlayerSheetData(json);
        GameInfo.LoadScene();
    }
}
