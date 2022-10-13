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
    
    private PlayerName playerName;
    
    public void SetCharacterInfo(PlayerName name, Sprite portrait, Color mask)
    {
        playerName = name;
        nameText.text = name.showName;
        portraitImage.sprite = portrait;
        maskImage.color = new Color(mask.r, mask.g, mask.b, maskImage.color.a);
    }

    public void SelectCharacter()
    {
        string json;
        SaveLoadSystem.LoadFile("chardata.chaos", out json, $"characters/{nameText.text}/");
        GameInfo.LoadPlayerSheetData(json);
    }
}
