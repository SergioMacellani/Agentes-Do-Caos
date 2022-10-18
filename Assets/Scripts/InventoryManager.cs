using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform _inventoryContent;
    [SerializeField] private ItemSlot _slotPrefab;
    
    [SerializeField] private TMP_InputField _maxWeight;
    [SerializeField] private TMP_InputField _currentWeight;

    [SerializeField] private List<ItemSlot> _itemsSlots;
    private int maxSlots = 20;

    public void SetValue(PlayerSheetData pSheet)
    {
        _maxWeight.text = pSheet.inventory.inventoryWeight.max.ToString();
        _currentWeight.text = pSheet.inventory.inventoryWeight.current.ToString();
        CreateSlots(pSheet.inventory.inventorySlots);
        UpdateWeight();
    }
    
    public PlayerInv GetValue()
    {
        PlayerInv pInv = new PlayerInv(){};
        
        pInv.inventoryWeight.max = int.Parse(_maxWeight.text);
        pInv.inventoryWeight.current = float.Parse(_currentWeight.text);
        
        var slots = _itemsSlots.Select(slot => slot.GetValue()).ToList();
        pInv.inventorySlots = slots;
        
        return pInv;
    }

    private void CreateSlots(List<InventorySlot> slots)
    {
        foreach (var s in slots)
        {
            _itemsSlots.Add(Instantiate(_slotPrefab,_inventoryContent).SetValue(s,this));
        }
    }

    public void UpdateWeight()
    {
        float current = 0;
        foreach (var item in _itemsSlots)
        {
            current += item.GetWeight;
        }

        _currentWeight.text = current.ToString("0.##");
    }
}
