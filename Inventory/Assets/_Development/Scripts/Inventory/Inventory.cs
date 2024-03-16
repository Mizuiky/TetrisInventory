using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Inventory: MonoBehaviour
{
    private Slot[,] _slots;
    private List<InventoryItem> _items;
    private Transform _inventoryItemsParent;
    private TextMeshProUGUI _itemDescription;

    private int _lines;
    private int _columns;
    public int Lines { get { return _lines; } }
    public int Columns { get { return _columns; } }
    public Transform ItemsParent { get { return _inventoryItemsParent; } }
    public List<InventoryItem> InventoryItems { get { return _items; } }

    public void Init(Slot[,] slots, Transform itemParent)
    {
        _slots = slots;

        _lines = slots.GetLength(0);
        _columns = slots.GetLength(1);

        _items = new List<InventoryItem>();
        _inventoryItemsParent = itemParent;
        transform.localScale = new Vector3(0.8f, 0.8f, 0f);
    }

    public void Open()
    {
        if (this != null)
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
    }
   

    public bool HasAddedItem(InventoryItem item)
    {
        var inventoryItem = GetInventoryItemById(item.Data.id);

        if (inventoryItem != null)
        {
            inventoryItem.Qtd++;
            inventoryItem.UpdateQtd();

            GameManager.Instance.ItemManager.UpdateInventoryItemList(item.Data);
            return true;
        }

        return false;
    }

    public InventoryItem GetInventoryItemById(int id)
    {
        var inventoryItem = _items.FirstOrDefault(x => x.Data.id == id);
        return inventoryItem;
    }

    public bool IsAvailableSlot(int line, int column)
    {
        if (_slots[line, column].HasItem)
            return false;

        return true;
    }  

    public Slot GetSlot(int line, int column)
    {
        return _slots[line, column];
    }

    public void AddItem(InventoryItem item, Slot currentAvailableSlot, List<SlotPosition> _slotPositions)
    {
        item.Move.SetPosition(_inventoryItemsParent, currentAvailableSlot);

        SetSlotPosition(item.Data, _slotPositions, false);

        item.Qtd++;
        item.UpdateQtd();

        _items.Add(item);

        GameManager.Instance.ItemManager.UpdateInventoryItemList(item.Data);
    }

    public void SetSlotPosition(InventoryItemData data, List<SlotPosition>_slotPositions, bool hasDetectItemConflict)
    {
        int count = 0;
        var imageData = data.imageConfig;
        var slots = _slotPositions.ToArray();

        for (int i = 0; i < imageData.GetLength(0); i++)
        {
            for (int j = 0; j < imageData.GetLength(1); j++)
            {
                var config = slots[count];
                data.slotPosition[count] = config;

                if (imageData[i, j] == 0)
                {
                    data.slotPosition[count].isEmpty = true;
                    count++;
                    continue;
                }

                data.slotPosition[i].isEmpty = false;

                if(!hasDetectItemConflict)
                    _slots[config.line, config.column].AddItem(data.id);

                count++;
            }
        }
    }

    public void RemoveItemFromSlots(SlotPosition[] slots, int id)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];

            if (_slots[slot.line, slot.column].HasItem && _slots[slot.line, slot.column].Data.attachedItemId != id)
                continue;

            _slots[slot.line, slot.column].RemoveItem();
            Debug.Log($"Removed Item from slots: Line{slot.line} Column{slot.column}");
        }
    }

    public void DeleteFromInventory(int id)
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            InventoryItem item = transform.GetChild(i).gameObject.GetComponent<InventoryItem>();
            if (item != null && item.Data.id == id)
            {
                _items.Remove(item);
                Destroy(item.gameObject);
                break;
            }            
        }
    }

    public void SetDescription(string description)
    {
        _itemDescription.text = description;
    }
}

[System.Serializable]
public class SlotPosition
{
    public int line;
    public int column;
    public bool isEmpty;

    public SlotPosition(int line, int column, bool isEmpty)
    {
        this.line = line;
        this.column = column;
        this.isEmpty = isEmpty;
    }
}
