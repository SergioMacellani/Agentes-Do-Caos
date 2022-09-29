using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI psique;
    [SerializeField]
    private TextMeshProUGUI combate;
    [SerializeField]
    private TextMeshProUGUI ocultismo;
    [SerializeField]
    private TextMeshProUGUI sentidos;

    public void SetSkills(int psique, int combate, int ocultismo, int sentidos)
    {
        this.psique.text = psique.ToString();
        this.combate.text = combate.ToString();
        this.ocultismo.text = ocultismo.ToString();
        this.sentidos.text = sentidos.ToString();
    }
}
