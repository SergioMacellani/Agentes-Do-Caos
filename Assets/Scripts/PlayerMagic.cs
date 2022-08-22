using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMagic : MonoBehaviour
{
    [Header("Grimore")]
    public Image SpecieIcon;
    public Image SpecieCircle;
    public Image GrimoireIcon;
    public TextMeshProUGUI SpecieName;
    public TextMeshProUGUI GrimoireAboutText;
    public TextAsset GrimoireAsset;
    public List<Sprite> GrimoireIcons;
    public List<Color> GrimoireColors;
    private List<string> GrimoireAbout = new List<string>();
    private List<int> GrimoireSelo = new List<int>();

    [Header("Magic")]
    public TextMeshProUGUI MagicText;

    public List<int> MagicNum;
    public List<TextMeshProUGUI> MagicNumText;
    private int MagicPoints = 100;

    [Header("Others")]
    public Animator Canvas;

    private void Start()
    {
        List<string> grimoireText = GrimoireAsset.text.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
        foreach (string grim in grimoireText)
        {
            List<string> specieGrim = grim.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            GrimoireSelo.Add(int.Parse(specieGrim[0]));
            GrimoireAbout.Add(specieGrim[1]);
        }
    }

    public void UpdateGrimore(int i, string name, Sprite icon)
    {
        SpecieIcon.sprite = icon;
        GrimoireIcon.sprite = GrimoireIcons[GrimoireSelo[i]];
        SpecieCircle.color = new Color(GrimoireColors[GrimoireSelo[i]].r, GrimoireColors[GrimoireSelo[i]].g, GrimoireColors[GrimoireSelo[i]].b, .5f);
        SpecieName.text = name;
        GrimoireAboutText.text = GrimoireAbout[i].Trim(new char[]{'\n'});
    }
    
    public void UpdateMagic(string magic)
    {
        string[] i = magic.Split(new string[] { "/" }, System.StringSplitOptions.RemoveEmptyEntries);
        int dNum = int.Parse(i[1]);
        int dplus = int.Parse(i[0]);
        
        if (dplus>0)
        {
            if (MagicNum[dNum]+dplus <= 80 && MagicPoints-dplus >= 0)
            {
                MagicNum[dNum]+=dplus;
                MagicPoints-=dplus;
            }
        }
        else if (MagicNum[dNum] > 5)
        {
            if (MagicPoints-dplus <= 100)
            {
                MagicNum[dNum]+=dplus;
                MagicPoints-=dplus;
            }
        }

        MagicNumText[dNum].text = MagicNum[dNum].ToString();
        MagicText.text = $"MAGIAS: {MagicPoints} PONTOS";
    }

    public void NextPage()
    {
        if (MagicPoints == 0)
        {
            Canvas.Play("MagicInventory");
        }
    }

    public List<int> MagicDiceNum => MagicNum;
}
