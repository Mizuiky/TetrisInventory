using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController
{
    private ItemSpawner _itemSpawner;
    private Inventory _inventory;

    private Slot _currentAvailableSlot;
    private List<SlotPosition> _auxSlotPositions;
    private List<int> _colorID;

    private float _slotWidth;
    private float _slotHeight;

    private int _inventoryLines;
    private int _inventoryColumns;

    private bool availableSlot = false;
    private bool _canAddItem = false;
    private bool _hasItemConflit = false;
    private Color[] _colors;
    private int _randomColorID;

    public InventoryController(ItemSpawner itemSpawner, Color[] colors)
    {
        _itemSpawner = itemSpawner;
        _colors = colors;
        _colorID = new List<int>();
    }

    public void Open()
    {
        _inventory.Open();
    }

    public void Init(Inventory inventory, float slotWidth, float slotHeight)
    {
        _inventory = inventory;
        _inventoryLines = _inventory.Lines;
        _inventoryColumns = _inventory.Columns;

        _auxSlotPositions = new List<SlotPosition>();

        _slotWidth = slotWidth;
        _slotHeight = slotHeight;
    }

    private int GetRandomColor()
    {
        var randomColorID = Random.Range(0, _colors.Length);

        while(_colorID.Contains(randomColorID))
        {        
            randomColorID = Random.Range(0, _colors.Length);
        }

        _colorID.Add(randomColorID);

        return randomColorID;
    }

    public void SetItem(InventoryItem item)
    {
        _auxSlotPositions.Clear();

        if (_inventory.HasAddedItem(item))
            return;

        for (int i = 0; i < _inventoryLines; i++)
        {
            for (int j = 0; j < _inventoryColumns; j++)
            {
                availableSlot = _inventory.IsAvailableSlot(i, j);

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


    private bool TryAddItem(InventoryItem item, int line, int column)
    {
        if (!CheckBoundsOverflow(item.Data, line, column))
            return false;

        if (!VerifyItemConfig(item.Data, line, column, false))
            return false;

        var currentItem = _itemSpawner.SpawnInventoryItem(item, _inventory.ItemsParent);

        if (currentItem != null)
        {
            currentItem.Move.SetSlotSize(_slotWidth, _slotHeight);
            currentItem.Move.SetInventorySize(_inventoryColumns - 1, _inventoryLines - 1);

            _randomColorID = GetRandomColor();

            currentItem.ColorTint.SetColor(_colors[_randomColorID]);

            _inventory.AddItem(currentItem, _currentAvailableSlot, _auxSlotPositions);
            _auxSlotPositions.Clear();
            return true;
        }

        return false;
    }

    private bool CheckBoundsOverflow(InventoryItemData data, int line, int column)
    {
        var linesAvailable = _inventoryLines - line;
        var columnsAvailable = _inventoryColumns - column;

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

                else if (data.imageConfig[i, j] == 1 && _inventory.GetSlot(auxLine, auxColumn).HasItem && data.id != _inventory.GetSlot(auxLine, auxColumn).Data.attachedItemId)
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

        _currentAvailableSlot = _inventory.GetSlot(line, column);

        if (!detectItemConflict)
            return true;

        return hasConflict;
    }

    public bool FindConflictOnNextMove(int id, int nextLine, int nextColumn)
    {
        var itemData = _inventory.GetInventoryItemById(id).Data;

        _hasItemConflit = VerifyItemConfig(itemData, nextLine, nextColumn, true);

        _inventory.RemoveItemFromSlots(itemData.slotPosition, itemData.id);
        _inventory.SetSlotPosition(itemData, _auxSlotPositions, _hasItemConflit);

        GameManager.Instance.ItemManager.UpdateInventoryItemList(itemData);

        if (_hasItemConflit)
            return true;

        return false;
    }

    public InventoryItem GetInventoryItem(int id)
    {
        return _inventory.GetInventoryItemById(id);
    }
}
