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

    public void SetSkills(PlayerSkills skills)
    {
        psique.text = skills.psique.ToString();
        combate.text = skills.combate.ToString();
        ocultismo.text = skills.ocultismo.ToString();
        sentidos.text = skills.sentidos.ToString();
    }
}
