using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerInfoCreate : MonoBehaviour
{
    [Header("Other Scripts")]
    public GameObject Canvas;
    public TextMeshProUGUI PlayerNameStyle;
    public TextMeshProUGUI PlayerSpecieStyle;
    public Bestiary Bestiary;
    public PlayerMagic Magics;
    public PlayerTecnics Tecnics;
    public PlayerInventory Inventory;
    
    [Header("Text")]
    public TextAsset SpeciesText;
    public TextAsset OccupationText;
    public TextAsset RandomNameText;
    public TextAsset RandomSurnameText;
    public TMP_InputField PlayerName;
    public TMP_InputField PlayerSpecie;
    public TMP_InputField PlayerOccupation;
    public TMP_InputField PlayerHeight;
    public TMP_InputField PlayerBiotype;

    [Header("List of Things")]
    public List<string> BiotypeExemples;
    private int biotypeSelect;
    
    public List<float> HeightExemples;
    private int heightSelect;
    
    private List<string> SpeciesName = new List<string>();
    private Dictionary<string,List<string>> SpeciesTecnics = new Dictionary<string, List<string>>();
    private int specieSelect;

    private List<string> occupationName = new List<string>();
    private Dictionary<string,List<string>> OccupationTecnics = new Dictionary<string, List<string>>();
    private int occupationSelect;
    private void Start()
    {
        string[] spc = SpeciesText.text.Split(new string[] {"-_-_-_-_-_-_-_-"}, StringSplitOptions.RemoveEmptyEntries);
        string[] ocp = OccupationText.text.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string spcInfo in spc)
        {
            string[] info = spcInfo.Split(new string[] {" - "}, StringSplitOptions.RemoveEmptyEntries);
            Color col;
            ColorUtility.TryParseHtmlString("#" + info[1], out col);
            SpeciesName.Add(info[0].Replace("\n",""));
            Bestiary.SpeciesColor.Add(col);
            Bestiary.SpeciesDescription.Add(info[2]);
            SpeciesTecnics.Add(info[0].Replace("\n",""), info[3].Trim(' ', '\n').Split(new string[] {"/"}, StringSplitOptions.RemoveEmptyEntries).ToList());
        }

        foreach (string ocupation in ocp)
        {
            string[] ocup = ocupation.Split(new string[] {" - "}, StringSplitOptions.RemoveEmptyEntries);
            occupationName.Add(ocup[0]);
            OccupationTecnics.Add(ocup[0],ocup[1].Trim(' ', '\n').Split(new string[] {"/"}, StringSplitOptions.RemoveEmptyEntries).ToList());
        }
        
        Tecnics.TecnicsSpeciesName = SpeciesTecnics[SpeciesName[specieSelect]];
        Tecnics.TecnicsOccupationName = OccupationTecnics[occupationName[occupationSelect]];
    }

    public void UpdateSpecies(bool next)
    {
        if (!next)
        {
            if (specieSelect < SpeciesName.Count-1)
            {
                specieSelect++;
            }
            else
            {
                specieSelect = 0;
            }
        }
        else
        {
            if (specieSelect > 0)
            {
                specieSelect--;
            }
            else
            {
                specieSelect = SpeciesName.Count-1;
            }
        }

        PlayerSpecie.text = SpeciesName[specieSelect];
        Bestiary.ChangeSpecie(specieSelect, PlayerSpecie.text);

    }
    
    public void UpdateOcupation(bool next)
    {
        if (!next)
        {
            if (occupationSelect < occupationName.Count-1)
            {
                occupationSelect++;
            }
            else
            {
                occupationSelect = 0;
            }
        }
        else
        {
            if (occupationSelect > 0)
            {
                occupationSelect--;
            }
            else
            {
                occupationSelect = occupationName.Count-1;
            }
        }
        
        PlayerOccupation.text = occupationName[occupationSelect];
    }
    
    public void UpdateBiotype(bool next)
    {
        if (!next)
        {
            if (biotypeSelect < BiotypeExemples.Count-1)
            {
                biotypeSelect++;
            }
            else
            {
                biotypeSelect = 0;
            }
        }
        else
        {
            if (biotypeSelect > 0)
            {
                biotypeSelect--;
            }
            else
            {
                biotypeSelect = BiotypeExemples.Count-1;
            }
        }
        
        PlayerBiotype.text = BiotypeExemples[biotypeSelect];
    }
    
    public void UpdateHeight(bool next)
    {
        if (!next)
        {
            if (heightSelect < HeightExemples.Count-1)
            {
                heightSelect++;
            }
            else
            {
                heightSelect = 0;
            }
        }
        else
        {
            if (heightSelect > 0)
            {
                heightSelect--;
            }
            else
            {
                heightSelect = HeightExemples.Count-1;
            }
        }

        if (heightSelect == 4)
        {
            PlayerHeight.text = "> " +HeightExemples[heightSelect]+" M";
        }else if (heightSelect == 5)
        {
            PlayerHeight.text = "< " +HeightExemples[heightSelect].ToString("#.#0")+" M";
        }
        else
        {
            PlayerHeight.text = HeightExemples[heightSelect].ToString("#.#0")+" M";
        }
    }

    public void NextPage()
    {
        if (!PlayerNick().Contains("playerName") && PlayerNick().Replace("  ", "").ToCharArray().Length > 6)
        {
            Canvas.GetComponent<Animator>().Play("StartToStyle");
            PlayerNameStyle.text = PlayerNick();
            PlayerSpecieStyle.text = SpeciesName[specieSelect];
            Magics.UpdateGrimore(specieSelect, SpeciesName[specieSelect], Bestiary.SpeciesSprite[specieSelect]);
            Tecnics.TecnicsSpeciesName = SpeciesTecnics[SpeciesName[specieSelect]];
            Tecnics.TecnicsOccupationName = OccupationTecnics[occupationName[occupationSelect]];
            Inventory.MaxWeightSet(PlayerWeightMax());
        }
    }

    public void RandomName()
    {
        string[] name = RandomNameText.text.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
        string[] surname = RandomSurnameText.text.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
        string nickname = name[Random.Range(0, name.Length)].Replace("\n","") + " " + surname[Random.Range(0, surname.Length)].Replace("\n","");
        PlayerName.text = nickname.Replace("\r", "");
    }
    
    private string PlayerNick()
    {
        return PlayerName.text.Replace("  ", "");
    }

    private int PlayerWeightMax()
    {
        float h = HeightExemples[heightSelect];
        int w = 10;
        if (h < 1.65f)
        {
            w -= 2;
        }
        else if (h > 1.8f)
        {
            w += 2;
        }

        if (biotypeSelect > 0 && biotypeSelect < 4)
        {
            w += biotypeSelect;
        }else if (biotypeSelect == 4)
        {
            w -= 2;
        }
        else
        {
            w -= 1;
        }
        
        return w;
    }
}
