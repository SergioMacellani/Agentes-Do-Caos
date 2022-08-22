using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TecnicsBar : MonoBehaviour
{
    public TextMeshProUGUI TecnicsNum;
    public PlayerTecnics PlayerTecnics;

    [HideInInspector]
    public int tecnicsPhase;
    [HideInInspector]
    public int tecnicDiceNum = 5;
    
    public void Inactive(bool active)
    {
        GetComponent<Button>().interactable = active;
        foreach (Transform child in transform)
        {
            child.GetComponent<Button>().interactable = active;
        }
    }
    
    public void TecnicUpdate(int i)
    {
        if (i>0)
        {
            if (PlayerTecnics.TecnicsInfo[tecnicsPhase].y-i >= 0 && tecnicDiceNum+i <= PlayerTecnics.TecnicsInfo[tecnicsPhase].x)
            {
                tecnicDiceNum+=i;
                PlayerTecnics.TecnicsInfo[tecnicsPhase].y-=i;
            }
        }
        else if (tecnicDiceNum > 5)
        {
            if (PlayerTecnics.TecnicsInfo[tecnicsPhase].y-i <= PlayerTecnics.TecnicsInfo[tecnicsPhase].z && tecnicDiceNum-i>=0)
            {
                tecnicDiceNum+=i;
                PlayerTecnics.TecnicsInfo[tecnicsPhase].y-=i;
            }
        }

        TecnicsNum.text = tecnicDiceNum.ToString();
        PlayerTecnics.UpdateTecnics();
    }
}
