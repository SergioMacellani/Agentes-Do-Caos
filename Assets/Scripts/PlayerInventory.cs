using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory")]
    public int MaxWeight;
    public float Weight;
    public List<string> ItemName;
    public List<float> ItemWeight;
    public TMP_InputField InventoryWeight;
    public TMP_InputField InventoryMaxWeight;
    public Transform InventoryParent;

    [Header("Potions")]
    public int MaxPotions;
    public List<int> Potions;
    public List<TMP_InputField> PotionsText = new List<TMP_InputField>();
    public TextMeshProUGUI PotionsTitle;
    public Transform PotionParent;
    private string pOld;

    [Header("Others")]
    public Animator Canvas;
    
    void Start()
    {
        foreach (Transform var in InventoryParent)
        {
            ItemName.Add(var.GetComponent<TMP_InputField>().text);
            ItemWeight.Add(float.Parse(var.GetChild(0).GetComponent<TMP_InputField>().text));
        }
        
        foreach (Transform var in PotionParent)
        {
            PotionsText.Add(var.GetChild(0).GetComponent<TMP_InputField>());
        }
    }

    public void UpdateInventoryItem()
    {
        ItemName.Clear();
        foreach (Transform var in InventoryParent)
        {
            ItemName.Add(var.GetComponent<TMP_InputField>().text);
        }
    }
    
    public void UpdateInventoryWeight()
    {
        ItemWeight.Clear();
        foreach (Transform var in InventoryParent)
        {
            ItemWeight.Add(System.Single.Parse(var.GetChild(0).GetComponent<TMP_InputField>().text.Replace(",", ".")));
        }
        
        Weight = 0;
        
        foreach (float w in ItemWeight)
        {
            Weight += w;
        }
        
        InventoryWeight.text = Weight.ToString("#.000");
    }
    
    public void UpdatePotions(string potion)
    {
        string[] i = potion.Split(new string[] { "/" }, System.StringSplitOptions.RemoveEmptyEntries);
        int p = int.Parse(i[0]);
        int pNum = int.Parse(i[1]);

        if (p>0)
        {
            if (Potions[pNum]+p <= 4 && MaxPotions-p >= 0)
            {
                Potions[pNum]+=p;
                MaxPotions-=p;
            }
        }
        else if (Potions[pNum] > 0)
        {
            if (MaxPotions-p <= 4)
            {
                Potions[pNum]+=p;
                MaxPotions-=p;
            }
        }

        PotionsText[pNum].text = Potions[pNum].ToString();
        PotionsTitle.text = $"POÇÕES: {MaxPotions}";
    }

    public void NextPage()
    {
        if (MaxPotions == 0 && Weight <= MaxWeight)
        {
            Canvas.Play("InventoryStory");
        }
    }

    public void MaxWeightSet(int w)
    {
        MaxWeight = w;
        InventoryMaxWeight.text = w.ToString();
    }
}
