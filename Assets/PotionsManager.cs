using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PotionsManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _regenPotion;
    [SerializeField] private TMP_InputField _strenghtPotion;
    [SerializeField] private TMP_InputField _poisonPotion;
    [SerializeField] private TMP_InputField _invisibilityPotion;
    [SerializeField] private TMP_InputField _truthPotion;
    [SerializeField] private TMP_InputField _resistencePotion;

    public void SetValue(PlayerPotions potions)
    {
        _regenPotion.text = potions.Potions["regeneration"].ToString();
        _strenghtPotion.text = potions.Potions["strength"].ToString();
        _poisonPotion.text = potions.Potions["poison"].ToString();
        _invisibilityPotion.text = potions.Potions["invisibility"].ToString();
        _truthPotion.text = potions.Potions["truth"].ToString();
        _resistencePotion.text = potions.Potions["resistance"].ToString();
    }
    
    public PlayerPotions GetValue()
    {
        PlayerPotions potions = new PlayerPotions
        {
            Potions =
            {
                ["regeneration"] = int.Parse(_regenPotion.text),
                ["strength"] = int.Parse(_strenghtPotion.text),
                ["poison"] = int.Parse(_poisonPotion.text),
                ["invisibility"] = int.Parse(_invisibilityPotion.text),
                ["truth"] = int.Parse(_truthPotion.text),
                ["resistance"] = int.Parse(_resistencePotion.text)
            }
        };

        return potions;
    }
}
