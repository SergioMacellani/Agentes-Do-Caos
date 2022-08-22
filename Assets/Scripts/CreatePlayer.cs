using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class CreatePlayer : MonoBehaviour
{
    private int keyPref = 11;

    [Header("Review Design")]
    public TextMeshProUGUI PlayerNameText;
    public TextMeshProUGUI PlayerSpeOccText;
    public TextMeshProUGUI PlayerBioText;
    
    public TextMeshProUGUI PlayerLifeText;
    public TextMeshProUGUI PlayerCaosText;
    public TextMeshProUGUI PlayerArmorText;

    public TMP_InputField PlayerNotesText;
    public Transform Inventory;
    public Transform Potions;

    public CreatePlayerOrder PlayerDesignReview;

    public Image TriquetaColor;
    public Image CircleColor;
    public Image InsideColor;

    [Header("Others Scripts")]
    public PlayerInfoCreate PlayerInfo;
    public CreatePlayerOrder PlayerView;
    public PlayerEssentials PlayerEssentials;
    public PlayerTecnics PlayerTecnics;
    public PlayerMagic PlayerMagic;
    public PlayerInventory PlayerInventory;
    public PlayerStory PlayerStory;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.GetString($"Save{i}") == "" && keyPref == 11)
                keyPref = i;
        }
    }

    public void UpdateReview()
    {
        PlayerNameText.text = PlayerInfo.PlayerName.text;
        PlayerSpeOccText.text = $"{PlayerInfo.PlayerSpecie.text} - {PlayerInfo.PlayerOccupation.text}";
        PlayerBioText.text = $"{PlayerInfo.PlayerBiotype.text} - {PlayerInfo.PlayerHeight.text}";
        
        PlayerLifeText.text = PlayerEssentials.lifePoints.ToString();
        PlayerCaosText.text = PlayerEssentials.chaosPoints.ToString();
        PlayerArmorText.text = PlayerEssentials.armorPoints.ToString();
        
        PlayerNotesText.text = $"{PlayerStory.HistoryText.text}\n------------\n{PlayerStory.NotesText.text}";

        int i = 0;
        foreach (Transform p in Potions)
        {
            p.GetChild(0).GetComponent<TMP_InputField>().text = PlayerInventory.Potions[i].ToString();
            i++;
        }
        
        i = 0;
        foreach (Transform inv in Inventory)
        {
            inv.GetComponent<TMP_InputField>().text = PlayerInventory.ItemName[i];
            inv.GetChild(0).GetComponent<TMP_InputField>().text = PlayerInventory.ItemWeight[i].ToString();
            i++;
        }

        PlayerDesignReview.PlayerPieces = PlayerView.PlayerPieces;
        PlayerDesignReview.PiecesColors = PlayerView.PiecesColors;
        PlayerDesignReview.ColorPieces();
        PlayerDesignReview.SpritePieces();

        TriquetaColor.color = PlayerInfo.Bestiary.BestirayImages[2].color;
        CircleColor.color = PlayerInfo.Bestiary.BestirayImages[2].color;
        InsideColor.color = PlayerInfo.Bestiary.BestirayImages[2].color;
    }

    public void CreatePlayerFile()
    {
        string save = $"#\n" +
                      $"# Player Info\n" +
                      $"#\n" +
                      $"name={PlayerInfo.PlayerName.text}\n" +
                      $"specie={PlayerInfo.PlayerSpecie.text.Replace("\r","")}\n" +
                      $"occupation={PlayerInfo.PlayerOccupation.text}\n" +
                      $"biotype={PlayerInfo.PlayerBiotype.text}\n" +
                      $"height={PlayerInfo.PlayerHeight.text}\n" +
                      $"#\n" +
                      $"# Player Essentials\n" +
                      $"#\n" +
                      $"life_max={PlayerEssentials.lifePoints}\n" +
                      $"life_att={PlayerEssentials.lifePoints}\n" +
                      $"caos_max={PlayerEssentials.chaosPoints}\n" +
                      $"caos_att={PlayerEssentials.chaosPoints}\n" +
                      $"armor_max={PlayerEssentials.armorPoints}\n" +
                      $"armor_att={PlayerEssentials.armorPoints}\n" +
                      $"lifearmor_max={PlayerEssentials.armorPoints*2}\n" +
                      $"lifearmor_att={PlayerEssentials.armorPoints*2}\n" +
                      $"#\n" +
                      $"# Player Stats\n" +
                      $"#\n" +
                      $"ferido=0\n" +
                      $"insano=0\n" +
                      $"inconsciente=0\n" +
                      $"#\n" +
                      $"# Player Tecnics\n" +
                      $"#\n" +
                      $"tecnics=[\n";
        
        foreach (TecnicsBar tec in PlayerTecnics.tecnics)
        {
            save += $"{tec.tecnicDiceNum}\n";
        }

        save += $"]\n" +
                $"#\n" +
                $"# Player Magic\n" +
                $"#\n" +
                $"magic=[\n";
        
        foreach (int magic in PlayerMagic.MagicDiceNum)
        {
            save += $"{magic}\n";
        }

        save += $"]\n" +
                $"#\n" +
                $"# Player Potions\n" +
                $"#\n" +
                $"potions=[\n";
        
        foreach (int pot in PlayerInventory.Potions)
        {
            save += $"{pot}\n";
        }
        
        save += $"]\n" +
                $"#\n" +
                $"# Player Inventory\n" +
                $"#\n" +
                $"inventory=[\n";
        
        int i = 0;
        foreach (string item in PlayerInventory.ItemName)
        {
            save += $"{item} - {PlayerInventory.ItemWeight[i]}\n";
            i++;
        }

        save += $"]\n" +
                $"inventory_maxweight={PlayerInventory.MaxWeight}\n" +
                $"inventory_weight={PlayerInventory.Weight}\n" +
                $"#\n" +
                $"# Player Story\n" +
                $"#\n" +
                $"notes=[\n" +
                $"{PlayerStory.HistoryText.text}\n" +
                $"------------\n" +
                $"{PlayerStory.NotesText.text}\n" +
                $"]\n" +
                $"#\n" +
                $"# Player Design\n" +
                $"#\n" +
                $"design=[\n";
        foreach (Sprite spr in PlayerView.PlayerPieces)
        {
            save += $"{spr.name}\n";
        }

        save += $"]\n" +
                $"#\n" +
                $"# Player Colors\n" +
                $"#\n" +
                $"colors=[\n";
        
        foreach (Color col in PlayerView.PiecesColors)
        {
            save += $"{ColorUtility.ToHtmlStringRGB(col)}\n";
        }
        
        save += $"]\n" +
                $"color_ex={ColorUtility.ToHtmlStringRGB(TriquetaColor.color)}\n" +
                $"#\n" +
                $"# Player File End\n" +
                $"#";
        
        PlayerPrefs.SetString($"Save{keyPref}", save);

        SceneManager.LoadScene("MenuNew");
    }
}
