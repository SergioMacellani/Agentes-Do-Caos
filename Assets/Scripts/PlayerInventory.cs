using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform _inventoryContent;
    [SerializeField] private ItemSlot _slotPrefab;
    
    [SerializeField] private TextMeshProUGUI _maxWeight;
    [SerializeField] private TextMeshProUGUI _currentWeight;

    [SerializeField] private List<ItemSlot> _itemsSlots;
    private int maxSlots = 20;

    public void SetValue(PlayerSheetData pSheet)
    {
        _maxWeight.text = pSheet.inventory.inventoryWeight.max.ToString();
        _currentWeight.text = pSheet.inventory.inventoryWeight.current.ToString();
        CreateSlots(pSheet.inventory.inventorySlots);
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

        _maxWeight.text = current.ToString();
    }
}
