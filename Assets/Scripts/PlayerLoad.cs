using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerLoad : MonoBehaviour
{
    public loading LoadingCanvas;
    [HideInInspector]
    public List<PlayerInfo> playerInfo;

    public List<Toggle> StatsToggle;
    public int playerSelect;
    private int iconType;
    
    [SerializeField]
    private List<Image> imageChangeColorNormal;
    [SerializeField]
    private List<Image> imageChangeColorDark;
    [SerializeField]
    private List<Image> imageChangeColorBright;

    public Image PlayerIcon;
    public CreatePlayerOrder PlayerCreateIcon;
    public TextMeshProUGUI PlayerName;
    public InventoryLoad InventoryLoadScript;
    private AJAX ajax;

    void Start()
    {
        TryGetComponent(out ajax);
        if (PlayerPrefs.GetInt("OfflineMode") != 2)
        {
            playerInfo = LoadingCanvas.playerInfo;
            playerSelect = PlayerPrefs.GetInt("PlayerSelect");
            PlayerName.text = playerInfo[playerSelect].Name;
            PlayerIcon.sprite = playerInfo[playerSelect].playerIcons[iconType];
        }
        else
        {
            PlayerIcon.gameObject.SetActive(false);
            PlayerCreateIcon.gameObject.SetActive(true);
            StartCoroutine(LoadCreatedIcon());
        }
        StartCoroutine(LayoutColor());
    }

    public void IconUpdate()
    {
        if (PlayerPrefs.GetInt("OfflineMode") != 2)
        {
            if (StatsToggle[0].isOn && !StatsToggle[2].isOn)
            {
                if (StatsToggle[1].isOn)
                {
                    iconType = 3;
                }
                else
                {
                    iconType = 1;
                }
            }
            else if (StatsToggle[2].isOn)
            {
                iconType = 4;
            }
            else if (StatsToggle[1].isOn)
            {
                iconType = 2;
            }
            else
            {
                iconType = 0;
            }

            PlayerIcon.sprite = playerInfo[playerSelect].playerIcons[iconType];
        }
    }

    private IEnumerator LayoutColor()
    {
        if (PlayerPrefs.GetInt("OfflineMode") != 2)
        {
            for (int i = 0; i < imageChangeColorNormal.Count; i++)
            {
                imageChangeColorNormal[i].color = playerInfo[playerSelect].playerColors[0];
            }

            for (int i = 0; i < imageChangeColorDark.Count; i++)
            {
                imageChangeColorDark[i].color = playerInfo[playerSelect].playerColors[1];
            }

            for (int i = 0; i < imageChangeColorBright.Count; i++)
            {
                imageChangeColorBright[i].color = playerInfo[playerSelect].playerColors[2];
            }
        }
        else
        {
            while (!ajax.SlotDic().ContainsKey("color_ex"))
            {
                Debug.Log("Loading...");
            }

            Color col;
            ColorUtility.TryParseHtmlString("#" + ajax.SlotDic()["color_ex"], out col);
            float h = 0, s = 0, v = 0;
            Color.RGBToHSV(col, out h, out s, out v);
            
            for (int i = 0; i < imageChangeColorNormal.Count; i++)
            {
                imageChangeColorNormal[i].color = col;
            }

            for (int i = 0; i < imageChangeColorDark.Count; i++)
            {
                imageChangeColorDark[i].color = Color.HSVToRGB(h, s, v - .25f);
            }

            for (int i = 0; i < imageChangeColorBright.Count; i++)
            {
                imageChangeColorBright[i].color = Color.HSVToRGB(h, s - .25f, v);
            }
            StartCoroutine(InventoryLoadScript.LoadInventory());
        }

        yield break;
    }

    private IEnumerator LoadCreatedIcon()
    {
        while (!ajax.SlotDic().ContainsKey("colors"))
        {
            Debug.Log("Loading...");
        }

        PlayerName.text = ajax.SlotDic()["name"];
        
        string[] icons = ajax.SlotDic()["design"].Split(new char[]{'\n', '\r'});
        string[] colors = ajax.SlotDic()["colors"].Split(new char[] {'\n', '\r'});
        Color col;
        
        for (int i = 0; i < PlayerCreateIcon.PlayerPieces.Count; i++)
        {
            if (i < 2 || i > 6)
            {
                PlayerCreateIcon.PlayerPieces[i] = Resources.Load<Sprite>($"PresetsIcon/{PlayerCreateIcon.PiecesLocal[i].name}/{icons[i]}");
            }
            else
            {
                if (i < 4)
                {
                    PlayerCreateIcon.PlayerPieces[i] = Resources.Load<Sprite>($"PresetsIcon/Hair/{icons[i]}");
                }
                else
                {
                    PlayerCreateIcon.PlayerPieces[i] = Resources.Load<Sprite>($"PresetsIcon/Eye/{PlayerCreateIcon.PiecesLocal[i].name}/{icons[i]}");
                }
            }
        }
        for (int i = 0; i < PlayerCreateIcon.PiecesColors.Count; i++)
        {
            ColorUtility.TryParseHtmlString("#"+colors[i], out col); 
            PlayerCreateIcon.PiecesColors[i] = col;
        }

        PlayerCreateIcon.ColorPieces();
        PlayerCreateIcon.SpritePieces();
        
        yield break;
    }
}
