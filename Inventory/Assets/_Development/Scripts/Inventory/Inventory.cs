using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Slot[,] _inventory;
    private Slot _currentAvailableSlot;
    private Transform _inventoryItemsParent;
    private List<SlotPosition> _auxSlotPositions;
    private List<InventoryItem> _items;
    private TextMeshProUGUI _itemDescription;

    private int _lines;
    private int _columns;
   
    private bool availableSlot = false;
    private bool _canAddItem = false;
    private bool _hasItemConflit = false;

    private float _slotWidth;
    private float _slotHeight;

    public void Init(Slot[,] slots, Transform itemParent, float slotWidth, float slotHeight)
    {
        _lines = slots.GetLength(0);
        _columns = slots.GetLength(1);
        _inventory = new Slot[_lines, _columns];
        _inventory = slots;
        _items = new List<InventoryItem>();
        _auxSlotPositions = new List<SlotPosition>();
        _inventoryItemsParent = itemParent;
        _slotWidth = slotWidth;
        _slotHeight = slotHeight;
    }

    public void SetItem(InventoryItem item)
    {
         _auxSlotPositions.Clear();

        if (HasAddedItem(item))
            return;

         for (int i = 0; i < _lines; i++)
         {
            for(int j = 0; j < _columns; j++)
            {
                availableSlot = IsAvailableSlot(_inventory[i,j]);

                if (availableSlot)
                {
                    _canAddItem = TryAddItem(item, i, j);
                    if (_canAddItem)
                        break;                   
                }
            }

            if (_canAddItem)
                break;            
        }

        _canAddItem = false;
    }

    private bool HasAddedItem(InventoryItem item)
    {
        var inventoryItem = GetInventoryItemById(item.Data.id);

        if (inventoryItem != null)
        {
            inventoryItem.Qtd++;
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

    private bool IsAvailableSlot(Slot slot)
    {
        if (slot.HasItem)
            return false;

        return true;
    }  

    private bool TryAddItem(InventoryItem item, int line, int column)
    {
        if (!CheckBoundsOverflow(item.Data, line, column))
            return false;

        if (!VerifyItemConfig(item.Data, line, column, false))
            return false;
        
        var currentItem = SetInventoryItem(item);

        if (currentItem != null)
        {
            AddItem(currentItem);
            _auxSlotPositions.Clear();
            return true;
        }
       
        return false;
    }

    private bool CheckBoundsOverflow(InventoryItemData data, int line, int column)
    {
        var linesAvailable = _inventory.GetLength(0) - line;
        var columnsAvailable = _inventory.GetLength(1) - column;

        var qtdLinesItem = data.imageConfig.GetLength(0);
        var qtdColumnsItem = data.imageConfig.GetLength(1);

        if (qtdColumnsItem > columnsAvailable)
            return false;


        else if (qtdLinesItem > linesAvailable)
            return false;

        return true;
    }

    private bool VerifyItemConfig(InventoryItemData data, int line, int column, bool detectItemConflict)
    {
        _auxSlotPositions.Clear();

        var auxLine = line;
        var auxColumn = column;
        var hasConflict = false;

        for (int i = 0; i < data.imageConfig.GetLength(0); i++)
        {
            for (int j = 0; j < data.imageConfig.GetLength(1); j++)
            {
                if (data.imageConfig[i, j] == 0)
                {
                    _auxSlotPositions.Add(new SlotPosition(auxLine, auxColumn, true));

                    if (j == data.imageConfig.GetLength(1) - 1)
                        auxColumn = column;
                    else
                        auxColumn++;

                    continue;
                }

                else if (data.imageConfig[i, j] == 1 && _inventory[auxLine, auxColumn].HasItem && data.id != _inventory[auxLine, auxColumn].Data.attachedItemId)
                {
                    if (!detectItemConflict)
                        return false;
                    
                    _auxSlotPositions.Add(new SlotPosition(auxLine, auxColumn, false));
                    hasConflict = true;
                    auxColumn++;                   
                }
                    
                else
                {
                    _auxSlotPositions.Add(new SlotPosition(auxLine, auxColumn, false));
                    auxColumn++;
                }

                if (j == data.imageConfig.GetLength(1) - 1)
                    auxColumn = column;
            }

            auxLine++;
        }

        _currentAvailableSlot = _inventory[line, column];

        if(!detectItemConflict)
            return true;

        return hasConflict;
    }

    private InventoryItem SetInventoryItem(InventoryItem item)
    {
        var newItem = GameManager.Instance.Spawner.Spawn(item.gameObject, _inventoryItemsParent);

        if (newItem != null)
        {
            var itemToAdd = newItem.GetComponent<InventoryItem>();
            itemToAdd.Data = item.Data;

            itemToAdd.SetComponents();
            itemToAdd.Move.SetSlotSize(_slotWidth, _slotHeight);
            itemToAdd.Move.SetInventorySize(_inventory.GetLength(1) - 1, _inventory.GetLength(0) - 1);

            return itemToAdd;
        }

        return null;
    }

    private void AddItem(InventoryItem item)
    {
        item.Move.SetPosition(_inventoryItemsParent, _currentAvailableSlot);

        SetSlotPosition(item.Data, false);

        item.Qtd++;

        _items.Add(item);

        GameManager.Instance.ItemManager.UpdateInventoryItemList(item.Data);
    }

    private void SetSlotPosition(InventoryItemData data, bool hasDetectItemConflict)
    {
        int count = 0;
        var imageData = data.imageConfig;
        var slots = _auxSlotPositions.ToArray();

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
                    _inventory[config.line, config.column].AddItem(data.id);

                count++;
            }
        }
    }

    public bool FindConflictOnNextMove(int id, int nextLine, int nextColumn) 
    {
        var itemData = GetInventoryItemById(id).Data;

        _hasItemConflit = false;

        SlotPosition [] oldSlots = new SlotPosition[itemData.slotPosition.Length];
        oldSlots = itemData.slotPosition;

        _hasItemConflit = VerifyItemConfig(itemData, nextLine, nextColumn, true);
        
        RemoveItemFromSlots(oldSlots, itemData.id);
        SetSlotPosition(itemData, _hasItemConflit);

        GameManager.Instance.ItemManager.UpdateInventoryItemList(itemData);

        Debug.Log(_inventory);

        if (_hasItemConflit)
            return true;
       
        return false;
    }

    private void RemoveItemFromSlots(SlotPosition[] slots, int id)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            Debug.Log($"Removed Item from slots: Line{slot.line} Column{slot.column}");

            if (_inventory[slot.line, slot.column].HasItem && _inventory[slot.line, slot.column].Data.attachedItemId != id)
                continue;

            _inventory[slot.line, slot.column].RemoveItem();
        }
    }

    private void RemoveFromInventory(int id)
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

    private void SetDescription(string description)
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
