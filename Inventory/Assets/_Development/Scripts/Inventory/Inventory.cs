using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    private Slot[,] _inventory;
    private Slot _currentAvailableSlot;
    private Transform _inventoryItemsParent;
    private List<SlotPosition> _slotPositions;
    private List<InventoryItem> _items;
    private TextMeshProUGUI _itemDescription;

    private int _lines;
    private int _columns;
   
    private bool availableSlot = false;
    private bool _canAddItem = false;

    private float _slotWidth;
    private float _slotHeight;

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

        if (HasAddedItem(item))
            return;

         for (int i = 0; i < _lines; i++)
         {
            for(int j = 0; j < _columns; j++)
            {
                availableSlot = IsAvailableSlot(_inventory[i,j]);

                if (availableSlot)
                {
                    _canAddItem = CheckCanAddItem(item, i, j);
                    if (_canAddItem)
                        break;                   
                }
            }

            if (_canAddItem)
                break;            
        }

        _canAddItem = false;
    }

    private bool OnCheckSlotAvailability(InventoryItem item, int nextLine, int nextColumn) 
    {
        if(CanStoreItem(item.Data, nextLine, nextColumn))
        {
            RemoveItemFromSlots(item.Data.slotPosition);
            SetSlot(item);
            return true;
        }

        return false;
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

    private bool CheckCanAddItem(InventoryItem item, int line, int column)
    {     
        if (CanStoreItem(item.Data, line, column))
        {
            var currentItem = SetInventoryItem(item);

            if (currentItem != null)
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
            itemToAdd.Data.slotPosition = _slotPositions.ToArray();
            itemToAdd.SetProperties();
            itemToAdd.SetSize();
            itemToAdd.Move.OnVerifyNextSlotAvailability += OnCheckSlotAvailability;

            return itemToAdd;
        }
        
        return null;
    }

    private bool CanStoreItem(InventoryItemData data, int line, int column)
    {
        var auxLine = line;
        var auxColumn = column;

        _slotPositions.Clear();

        var linesAvailable = _inventory.GetLength(0) - line;
        var columnsAvailable = _inventory.GetLength(1) - column;

        var qtdLinesItem = data.imageConfig.GetLength(0);
        var qtdColumnsItem = data.imageConfig.GetLength(1);   

        if (qtdColumnsItem > columnsAvailable)
            return false;
        
        
        else if (qtdLinesItem > linesAvailable)
            return false;

        for (int i = 0; i < data.imageConfig.GetLength(0); i++)
        {
            for (int j = 0; j < data.imageConfig.GetLength(1); j++)
            {
                if (data.imageConfig[i, j] == 0)
                {
                    _slotPositions.Add(new SlotPosition(auxLine, auxColumn, true));

                    if (j == data.imageConfig.GetLength(1) - 1)
                        auxColumn = column;
                    else
                        auxColumn++;

                    continue;
                }  
                
                else if (data.imageConfig[i, j] == 1 && _inventory[auxLine, auxColumn].HasItem && data.id != _inventory[auxLine,auxColumn].Data.attachedItemId)
                    return false;           
                
                else
                {             
                    _slotPositions.Add(new SlotPosition(auxLine, auxColumn, false));
                    auxColumn++;                  
                }

                if (j == data.imageConfig.GetLength(1) - 1)
                    auxColumn = column;
            }

            auxLine++;            
        }

         _currentAvailableSlot = _inventory[line, column];

        return true;
    }

    private void AddItem(InventoryItem item)
    {
        item.SetPosition(_inventoryItemsParent, _currentAvailableSlot, _slotWidth, _slotHeight);

        SetSlot(item);

        item.Qtd++;

        _items.Add(item);

        GameManager.Instance.ItemManager.UpdateInventoryItemList(item.Data);
    }

    private void SetSlot(InventoryItem item)
    {
        int count = 0;
        var imageData = item.Data.imageConfig;

        var slots = _slotPositions.ToArray();

        for (int i = 0; i < imageData.GetLength(0); i++)
        {
            for (int j = 0; j < imageData.GetLength(1); j++)
            {
                var config = slots[count];
                item.Data.slotPosition[count] = config;

                if (imageData[i, j] == 0)
                {
                    item.Data.slotPosition[count].isEmpty = true;
                    count++;
                    continue;
                }

                item.Data.slotPosition[i].isEmpty = false;
                _inventory[config.line, config.column].AddItem(item.Data.id);
            }
        }
    }

    private void RemoveItemFromSlots(SlotPosition[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            Debug.Log($"Removed Item from slots: Line{slot.line} Column{slot.column}");

            if (!slot.isEmpty)
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
