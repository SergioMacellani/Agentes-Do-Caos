using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemWeight;

    private PlayerInventory _playerInventory;

    public ItemSlot SetValue(InventorySlot slot, PlayerInventory pInv)
    {
        _playerInventory = pInv;
        
        _itemName.text = slot.itemName;
        _itemWeight.text = slot.itemWeight.ToString();

        return this;
    }

    public float GetWeight => float.Parse(_itemWeight.text);
    
    public void UpdateWeight()
    {
        _playerInventory.UpdateWeight();
    }
}
