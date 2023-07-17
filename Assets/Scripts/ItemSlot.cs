using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TMP_InputField _itemName;
    [SerializeField] private TMP_InputField _itemWeight;

    private InventoryManager _inventoryManager;

    public ItemSlot SetValue(InventorySlot slot, InventoryManager pInv)
    {
        _inventoryManager = pInv;
        
        _itemName.text = slot.itemName;
        _itemWeight.text = slot.itemWeight.ToString();

        return this;
    }
    
    public InventorySlot GetValue()
    {
        return new InventorySlot(_itemName.text, float.Parse(_itemWeight.text));
    }

    public float GetWeight => float.Parse(_itemWeight.text);
    
    public void UpdateWeight()
    {
        _itemWeight.text = float.Parse(_itemWeight.text).ToString("0.##");
        _inventoryManager.UpdateWeight();
    }
}
