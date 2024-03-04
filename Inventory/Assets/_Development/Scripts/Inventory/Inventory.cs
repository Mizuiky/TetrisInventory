using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Slot[,] _inventory;
    private int _lines;
    private int _columns;
    private TextMeshProUGUI _itemDescription;
    private bool availableSlot = false;

    private Transform _inventoryItemsParent;
    private List<InventoryItem> _items;

    private float _slotWidth;
    private float _slotHeight;

    private Slot _currentAvailableSlot;
    private List<SlotPosition> _slotPositions;
    private bool _canAddItem = false;

    public void Init(Slot[,] slots, Transform itemParent, float slotWidth, float slotHeight)
    {
        _lines = slots.GetLength(0);
        _columns = slots.GetLength(1);
        _inventory = new Slot[_lines, _columns];
        _inventory = slots;
        _items = new List<InventoryItem>();
        _slotPositions = new List<SlotPosition>();
        _inventoryItemsParent = itemParent;
        _slotWidth = slotWidth;
        _slotHeight = slotHeight;
    }

    public void SetItem(InventoryItem item)
    {
        _slotPositions.Clear();

        for (int i = 0; i < _lines; i++)
        {
            for(int j = 0; j < _columns; j++)
            {
                availableSlot = IsAvailableSlot(_inventory[i,j]);

                if (availableSlot)
                {
                    _canAddItem = CheckCanAttachItem(item, i, j);
                    if (_canAddItem)
                        break;               
                }
            }

            if (_canAddItem)
                break;
        }    
    }

    private bool IsAvailableSlot(Slot slot)
    {
        if (slot.HasItem)
            return false;

        return true;
    }

    private bool CheckCanAttachItem(InventoryItem item, int line, int column)
    {
        var currentItem = SetInventoryItem(item);

        if(currentItem != null)
        {
            if (CanAddItem(currentItem, line, column))
            {
                AddItem(currentItem);
                _slotPositions.Clear();
                return true;
            }
        }
    
        return false;
    }

    private InventoryItem SetInventoryItem(InventoryItem item)
    {   
        var newItem = GameManager.Instance.Spawner.Spawn(item.gameObject, _inventoryItemsParent);
            
        if (newItem != null)
        {
            var itemToAdd = newItem.GetComponent<InventoryItem>();
            itemToAdd.Data = item.Data;
            itemToAdd.SetProperties();
            itemToAdd.SetSize();
               
            return itemToAdd;
        }
        
        return null;
    }

    private bool CanAddItem(InventoryItem Item, int line, int column)
    {
        for (int i = line; i < _inventory.GetLength(0); i++)
        {
            for (int j = column; j < _inventory.GetLength(1); j++)
            {
                if (CanStoreItem(Item.Data.imageConfig, i, j))
                    return true;
                else
                    if (j + 1 > _inventory.GetLength(1) - 1)
                        column = 0;
            }
        }
        return false;       
    }

    private bool CanStoreItem(int[,] itemConfig, int line, int column)
    {
        var auxLine = line;
        var auxColumn = column;

        _slotPositions.Clear();

        var inventoryLenght = _inventory.GetLength(1) - column;
        var itemLenght = itemConfig.GetLength(1);

        if (itemLenght > inventoryLenght)
            return false;

        for (int i = 0; i < itemConfig.GetLength(0); i++)
        {
            for (int j = 0; j < itemConfig.GetLength(1); j++)
            {
                if (itemConfig[i, j] == 0)
                {
                    if (j == itemConfig.GetLength(1) - 1)
                        auxColumn = 0;

                    continue;
                }             
                else if (itemConfig[i, j] == 1 && _inventory[auxLine, auxColumn].HasItem)
                    return false;           
                
                else
                {             
                    _slotPositions.Add(new SlotPosition(auxLine, auxColumn));
                    auxColumn++;                  
                }

                if (j == itemConfig.GetLength(1) - 1)
                    auxColumn = 0;
            }

            auxLine++;            
        }

        _currentAvailableSlot = _inventory[line, column];

        return true;
    }

    private void AddItem(InventoryItem item)
    {
        item.SetPosition(_inventoryItemsParent, _currentAvailableSlot, _slotWidth, _slotHeight);

        var slots = _slotPositions.ToArray();

        for(int i = 0; i < slots.Length; i++)
        {
            var config = slots[i];
            item.Data.slotPosition[i] = config;
            _inventory[config.line, config.column].HasItem = true;
            _inventory[config.line, config.column].AttachItem(item.Data.id);
        }

        _items.Add(item);

        GameManager.Instance.ItemManager.UpdateInventoryItemList(item.Data);
    }

    private void RemoveItem(int id)
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

    public SlotPosition(int line, int column)
    {
        this.line = line;
        this.column = column;
    }
}
