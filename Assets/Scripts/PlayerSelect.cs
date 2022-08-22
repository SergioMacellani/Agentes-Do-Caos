using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public int playerSelect;
    public int lostPlayersCount;
    private TextMeshProUGUI nickText;
    
    public List<Sprite> PlayerSprite;
    public List<string> PlayerNames;
    public int LostPlayers;

    public Image PlayerImage;
    
    void Start()
    {
        TryGetComponent(out nickText);
        playerSelect = PlayerPrefs.GetInt("PlayerSelect");
        nickText.text = PlayerNames[playerSelect];
        PlayerImage.sprite = PlayerSprite[playerSelect];
        lostPlayersCount = PlayerSprite.Count - LostPlayers;
    }

    public void NextBack(bool next)
    {
        if (next)
        {
            if (playerSelect < PlayerSprite.Count-1)
            {
                playerSelect++;
            }
            else
            {
                playerSelect = 0;
            }
        }
        else
        {
            if (playerSelect > 0)
            {
                playerSelect--;
            }
            else
            {
                playerSelect = PlayerSprite.Count-1;
            }
        }
        
        nickText.text = PlayerNames[playerSelect];
        PlayerImage.sprite = PlayerSprite[playerSelect];

        if (playerSelect >= lostPlayersCount)
        {
            PlayerImage.color = Color.black;
            nickText.fontStyle = FontStyles.Strikethrough;
        }
        else
        {
            PlayerImage.color = Color.white;
            nickText.fontStyle = FontStyles.Normal;
        }
    }
}
