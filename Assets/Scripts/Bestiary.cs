using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Bestiary : MonoBehaviour
{
    public TextMeshProUGUI SpeciesName;
    public TextMeshProUGUI SpeciesAbout;
    public List<Image> BestirayImages;
    public List<Sprite> SpeciesSprite;
    
    [HideInInspector]
    public List<Color> SpeciesColor;
    [HideInInspector]
    public List<string> SpeciesDescription;
    
    public void ChangeSpecie(int i, string specie)
    {
        BestirayImages[0].sprite = SpeciesSprite[i];
        BestirayImages[1].color = SpeciesColor[i];
        BestirayImages[2].color = SpeciesColor[i];
        BestirayImages[3].color = SpeciesColor[i];

        SpeciesName.text = specie;
        SpeciesAbout.text = SpeciesDescription[i];
    }

    /*
    private void Start()
    {

    }
    */
}
