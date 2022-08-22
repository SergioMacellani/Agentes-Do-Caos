using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryLoad : MonoBehaviour
{
    [SerializeField]
    private GameObject inventorySlot;
    private int inventoryCount = 20;
    [HideInInspector]
    public List<string> itemName;
    [HideInInspector]
    public List<string> itemWeight;

    public WeightUpdate weightUpdate;
    
    public IEnumerator LoadInventory()
    {
        for (int i = 0; i < inventoryCount; i++)
        {
            GameObject slot = (GameObject) Instantiate(inventorySlot, transform);
            
            TMP_InputField item;
            TMP_InputField weight;
            WeightItem weightItem;
            slot.TryGetComponent(out item);
            slot.transform.GetChild(0).TryGetComponent(out weight);
            slot.transform.GetChild(0).TryGetComponent(out weightItem);
            weightItem.WeightUpdateScript = weightUpdate;
            if (i < itemName.Count)
            {
                item.text = itemName[i];
                weight.text = itemWeight[i];
            }
            else
            {
                item.text = "Slot Vazio";
                weight.text = "0,0";
            }
        }
        yield break;
    }
}
