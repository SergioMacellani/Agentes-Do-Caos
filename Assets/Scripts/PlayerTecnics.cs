using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerTecnics : MonoBehaviour
{
    public Vector3[] TecnicsInfo;
    public GameObject TecnicsPrefab;
    public Transform TecnicsGroup;
    public TextAsset TecnicsNameList;
    public TextMeshProUGUI TecnicsText;
    public Animator canvas;

    [HideInInspector]
    public List<string> TecnicsSpeciesName;
    [HideInInspector]
    public List<string> TecnicsOccupationName;

    private List<TecnicsBar> TecnicsSpecies = new List<TecnicsBar>();
    private List<TecnicsBar> TecnicsOccupation = new List<TecnicsBar>();
    [HideInInspector]
    public List<TecnicsBar> tecnics = new List<TecnicsBar>();
    private List<string> tecnicsName = new List<string>();
    public int TecnicsPhase;
    private string[] phaseName = new string[3] {"", " DE ESPECIE", " DE PROFISSÃO"};
    
    void Start()
    {
        string[] split = { "\n" };
        tecnicsName = TecnicsNameList.text.Trim(' ').Split(split, StringSplitOptions.RemoveEmptyEntries).ToList();
        foreach (string name in tecnicsName)
        {
            GameObject tecnicsBar = (GameObject)Instantiate(TecnicsPrefab, TecnicsGroup);
            tecnicsBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            TecnicsBar tecnicScript = tecnicsBar.GetComponent<TecnicsBar>();
            tecnicScript.PlayerTecnics = this;
            tecnics.Add(tecnicScript);
        }
    }

    public void UpdateSpcOcc()
    {
        foreach (string i in TecnicsSpeciesName)
        {
            TecnicsSpecies.Add(tecnics[int.Parse(i)-1]);
        }
        foreach (string i in TecnicsOccupationName)
        {
            TecnicsOccupation.Add(tecnics[int.Parse(i)-1]);
        }
    }

    void UpdateBoxPhase()
    {
        if (TecnicsPhase > 0)
        {
            foreach (TecnicsBar tec in tecnics)
            {
                tec.Inactive(false);
            }

            if (TecnicsPhase == 1)
            {
                foreach (TecnicsBar spc in TecnicsSpecies)
                {
                    spc.Inactive(true);
                }
            }else
            {
                foreach (TecnicsBar occ in TecnicsOccupation)
                {
                    occ.Inactive(true);
                }
            }
        }
        else
        {
            foreach (TecnicsBar tec in tecnics)
            {
                tec.Inactive(true);
            }
        }
    }
    
    public void UpdateTecnics()
    {
        TecnicsText.text = $"TECNICAS: {TecnicsInfo[TecnicsPhase].y} PONTOS{phaseName[TecnicsPhase]}";
    }

    public void ChangePhase(bool next)
    {
        if (next)
        {
            if (TecnicsPhase < 2)
            {
                TecnicsPhase++;
            }
            else if(TecnicsInfo[0].y == 0 && TecnicsInfo[1].y == 0 && TecnicsInfo[2].y == 0)
            {
                canvas.Play("TecnicsMagic");
            }
        }
        else
        {
            if (TecnicsPhase > 0)
            {
                TecnicsPhase--;
            }
            else
            {
                canvas.Play("TecnicsEssentials");
            }
        }

        foreach (var tec in tecnics)
        {
            tec.tecnicsPhase = TecnicsPhase;
        }

        UpdateBoxPhase();
        UpdateTecnics();
    }
}
