using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerEssentials : MonoBehaviour
{
    public int EssentialsPoints;
    public TextMeshProUGUI LifeText;
    public Vector2 LifeMinMax;
    public TextMeshProUGUI ChaosText;
    public Vector2 ChaosMinMax;
    public TextMeshProUGUI ArmorText;
    public Vector2 ArmorMinMax;

    public TextMeshProUGUI EssentialsText;
    public Animator Canvas;
    
    [HideInInspector]
    public int lifePoints;
    [HideInInspector]
    public int chaosPoints;
    [HideInInspector]
    public int armorPoints;

    private void Start()
    {
        lifePoints = (int)LifeMinMax.x;
        chaosPoints = (int)ChaosMinMax.x;
        armorPoints = (int)ArmorMinMax.x;
    }

    public void ChangeLife(bool plus)
    {
        if (plus)
        {
            if (EssentialsPoints > 0 && lifePoints < LifeMinMax.y)
            {
                lifePoints++;
                EssentialsPoints--;
            }
        }
        else if (lifePoints > LifeMinMax.x)
        {
            lifePoints--;
            EssentialsPoints++;
        }

        LifeText.text = lifePoints.ToString();
        UpdateEssentialsText();
    }
    
    public void ChangeChaos(bool plus)
    {
        if (plus)
        {
            if (EssentialsPoints > 0 && chaosPoints < ChaosMinMax.y)
            {
                chaosPoints++;
                EssentialsPoints--;
            }
        }
        else if (chaosPoints > ChaosMinMax.x)
        {
            chaosPoints--;
            EssentialsPoints++;
        }

        ChaosText.text = chaosPoints.ToString();
        UpdateEssentialsText();
    }
    
    public void ChangeArmor(bool plus)
    {
        if (plus)
        {
            if (EssentialsPoints > 0 && armorPoints < ArmorMinMax.y)
            {
                armorPoints++;
                EssentialsPoints--;
            }
        }
        else if (armorPoints > ArmorMinMax.x)
        {
            armorPoints--;
            EssentialsPoints++;
        }

        ArmorText.text = armorPoints.ToString();
        UpdateEssentialsText();
    }

    private void UpdateEssentialsText()
    {
        EssentialsText.text = $"ESSENCIAIS: {EssentialsPoints} PONTOS";
    }

    public void NextPage()
    {
        if (EssentialsPoints == 0)
        {
            Canvas.Play("EssentialsTecnics");
        }
    }
}
