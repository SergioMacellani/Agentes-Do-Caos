using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectCreate : MonoBehaviour
{
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI ButtonText;
    public TextMeshProUGUI SizeSlotText;
    public CreatePlayerOrder IconStyle;
    public Image TriquetaSprite;
    public Image CircleSprite;
    public Image InsideSprite;

    public GameObject DeleteBtt;
    public Sprite EmptySlot;

    [HideInInspector]
    public int playerSelect;
    private bool createSlot = false;

    private List<Dictionary<string, string>> playerSlotSave = new List<Dictionary<string, string>>();

    private void Start()
    {
        playerSelect = PlayerPrefs.GetInt("PlayerCreateSelect");

        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.GetString($"Save{i}").StartsWith("#"))
                LoadSlots(PlayerPrefs.GetString($"Save{i}"));
        }
        
        if (!PlayerPrefs.GetString($"Save{playerSelect}").StartsWith("#"))
        {
            if(createSlot)
                return;

            ButtonText.text = "CRIAR PERSONAGEM";
            createSlot = true;
            PlayerName.text = "Slot Vazio";

            for (int i = 0; i < IconStyle.PlayerPieces.Count - 1; i++)
            {
                IconStyle.PlayerPieces[i] = EmptySlot;
            }
            for (int i = 0; i < IconStyle.PiecesColors.Count - 1; i++)
            {
                IconStyle.PiecesColors[i] = Color.gray;
            }
            IconStyle.ColorPieces();
            IconStyle.SpritePieces();
            
            TriquetaSprite.color = Color.white;
            CircleSprite.color = Color.white;
            InsideSprite.color = Color.white;
            
            DeleteBtt.SetActive(false);
        }
        else
        {
            ButtonText.text = "SELECIONAR";
            createSlot = false;
            LoadSave();
        }
        SizeSlotText.text = $"{playerSelect+1}/10";
    }

    public void NextBack(bool next)
    {
        if (next)
        {
            if (playerSelect < 9)
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
                playerSelect = 9;
            }
        }

        SizeSlotText.text = $"{playerSelect+1}/10";
        
        if (Slot() == "")
        {
            if(createSlot)
                return; 
            
            ButtonText.text = "CRIAR PERSONAGEM";
            createSlot = true;
            PlayerName.text = "Slot Vazio";
            TriquetaSprite.color = Color.white;
            CircleSprite.color = Color.white;
            InsideSprite.color = Color.white;

            for (int i = 0; i < IconStyle.PlayerPieces.Count - 1; i++)
            {
                IconStyle.PlayerPieces[i] = EmptySlot;
            }
            for (int i = 0; i < IconStyle.PiecesColors.Count - 1; i++)
            {
                IconStyle.PiecesColors[i] = Color.gray;
            }
            
            TriquetaSprite.color = Color.white;
            CircleSprite.color = Color.white;
            InsideSprite.color = Color.white;
            
            IconStyle.ColorPieces();
            IconStyle.SpritePieces();
            DeleteBtt.SetActive(false);
        }
        else
        {
            ButtonText.text = "SELECIONAR";
            createSlot = false;
            DeleteBtt.SetActive(true);
            LoadSave();
        }
    }

    private void LoadSlots(string slt)
    {
        int i = 0;
        bool addLine = false;
        string valueLine = "";
        string keyLine = "";
        Dictionary<string, string> playerSelected = new Dictionary<string, string>();
        foreach (string ln in System.Text.RegularExpressions.Regex.Split(slt, "\r\n|\r|\n"))
        {
            if(ln.StartsWith("#"))
                continue;
            if(ln.Trim().Length == 0)
                continue;
            
            string line = ln.TrimStart();

            if (line.Contains("=[") || addLine)
            {
                if (addLine)
                {
                    if (!line.StartsWith("]"))
                    {
                        valueLine += $"{line}\n";
                    }
                    else
                    {
                        playerSelected.Add(keyLine, valueLine);
                        keyLine = "";
                        valueLine = "";
                        addLine = false;
                    }
                }
                else
                {
                    string[] s = line.Split(new char[] {'='}, 2);
                    keyLine = s[0];
                    addLine = true;
                }
            }
            else
            {
                string[] s = line.Split(new char[] {'='}, 2);

                if (s.Length == 2)
                {
                    if (s[0].Length == 0)
                    {
                        Debug.LogError("Key Error! Line:" + i);
                    }
                    else
                    {
                        playerSelected.Add(s[0], s[1]);
                    }
                }
                else
                {
                    if (s[0].Length == 0)
                    {
                        Debug.LogError("Key Error! Line:" + i);
                    }
                    else
                    {
                        playerSelected.Add(s[0], s[1]);
                    }
                }
            }

            i++;
        }

        playerSlotSave.Add(playerSelected);
    }

    private void LoadSave()
    {
        Color col;
        string[] icons = DicSlot()["design"].Split(new char[]{'\n', '\r'});
        string[] colors = DicSlot()["colors"].Split(new char[]{'\n', '\r'});
        PlayerName.text = DicSlot()["name"];
        ColorUtility.TryParseHtmlString("#"+DicSlot()["color_ex"], out col);
        TriquetaSprite.color = col;
        CircleSprite.color = col;
        InsideSprite.color = col;

        for (int i = 0; i < IconStyle.PlayerPieces.Count; i++)
        {
            if (i < 2 || i > 6)
            {
                IconStyle.PlayerPieces[i] = Resources.Load<Sprite>($"PresetsIcon/{IconStyle.PiecesLocal[i].name}/{icons[i]}");   
                Debug.Log($"PresetsIcon/{IconStyle.PiecesLocal[i].name}/{icons[i]}");
            }
            else
            {
                if (i < 4)
                {
                    IconStyle.PlayerPieces[i] = Resources.Load<Sprite>($"PresetsIcon/Hair/{icons[i]}");
                }
                else
                {
                    IconStyle.PlayerPieces[i] = Resources.Load<Sprite>($"PresetsIcon/Eye/{IconStyle.PiecesLocal[i].name}/{icons[i]}");
                }
            }
        }
        for (int i = 0; i < IconStyle.PiecesColors.Count; i++)
        {
            ColorUtility.TryParseHtmlString("#"+colors[i], out col); 
            IconStyle.PiecesColors[i] = col;
        }

        IconStyle.ColorPieces();
        IconStyle.SpritePieces();
    }

    public void DeleteSave()
    {
        PlayerPrefs.SetString($"Save{playerSelect}", "");
        
        ButtonText.text = "CRIAR PERSONAGEM";
        createSlot = true;
        PlayerName.text = "Slot Vazio";
        TriquetaSprite.color = Color.white;
        CircleSprite.color = Color.white;
        InsideSprite.color = Color.white;

        for (int i = 0; i < IconStyle.PlayerPieces.Count - 1; i++)
        {
            IconStyle.PlayerPieces[i] = EmptySlot;
        }
        for (int i = 0; i < IconStyle.PiecesColors.Count - 1; i++)
        {
            IconStyle.PiecesColors[i] = Color.gray;
        }
        IconStyle.ColorPieces();
        IconStyle.SpritePieces();
    }

    private string Slot()
    {
        return PlayerPrefs.GetString($"Save{playerSelect}");
    }
    
    private Dictionary<string, string> DicSlot()
    {
        return playerSlotSave[playerSelect];
    }

    public bool createPlayer() => createSlot;
}
