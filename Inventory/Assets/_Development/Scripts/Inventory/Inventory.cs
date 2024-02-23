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
    private bool canAddItem = false;

    private Transform _inventoryItemsParent;
    private List<InventoryItem> _items;

    private float _slotWidth;
    private float _slotHeight;

    private Vector3 _topLeft;
    private Vector3 _bottomLeft;
    private Vector3 _topRight;
    private Vector3 _bottomRight;

    private InventoryItem _currentItem;
    private Slot _currentSlot;
    private Slot _attachedSlot;
    private List<SlotPosition> _slotPositions;

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

        SetInventoryCorners();
    }

    public void Open(bool canOpen)
    {
        if(canOpen)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void SetItem(GameObject item, int id)
    {
        for (int i = 0; i < _lines; i++)
        {
            for(int j = 0; j < _columns; j++)
            {
                _currentSlot = _inventory[i, j];
                availableSlot = HasItemOnSlot(id);

                if (!availableSlot)
                {
                    bool canAddItem = CheckAddItem(item);
                    if (canAddItem)
                        break;               
                }
            }

            if (canAddItem)
                break;
        }    
    }

    private bool HasItemOnSlot(int id)
    {
        if (_currentSlot.HasItem)
        {
            UpdateQtd(id);
            return true;
        }

        return false;
    }

    private bool CheckAddItem(GameObject item)
    {
        if (_currentItem == null)
        {
            var attachedItem = GameManager.Instance.Spawner.Spawn(item.gameObject, _inventoryItemsParent);

            if (attachedItem != null)
            {
                var component = attachedItem.GetComponent<InventoryItem>();
                component.SetProperties();
                component.SetSize();
                
                var data = GameManager.Instance.ItemManager.GetInventoryDataById(component.Data.id);
                if (data != null)
                    component.Data = data;

                _currentItem = component;               
            }
        }

        if (CanAttachItem())
        {
            Attach();
            _slotPositions.Clear();
            return true;
        }

        return false;
    }

    private bool CanAttachItem()
    {
        for (int i = 0; i < _inventory.GetLength(0); i++)
        {
            for (int j = 0; j < _inventory.GetLength(1); j++)
            {
                if (CanStoreItem(_currentItem.Data.imageConfig, i, j))
                    return true;
            }
        }
        return false;       
    }

    private bool CanStoreItem(int[,] itemConfig, int line, int column)
    {
        for (int i = 0; i < itemConfig.GetLength(0); i++)
        {
            for (int j = 0; j < itemConfig.GetLength(1); j++)
            {
                line += i;
                column += j;

                if (itemConfig[i, j] == 1 && _inventory[line, column].HasItem)
                {
                    _slotPositions.Clear();
                    return false;
                }
                  
                _slotPositions.Add(new SlotPosition(line, column));
            }
        }

        //_attachedSlot = 
        return true;
    }

    private void Attach()
    {
        //_currentItem.SetPosition(_inventoryItemsParent, _attachedSlot);

        var slots = _slotPositions.ToArray();

        for(int i = 0; i < slots.Length; i++)
        {
            var config = slots[i];
            _currentItem.Data.slotPosition[i] = config;
            var slot = _inventory[config.line, config.column];
            slot.HasItem = true;
            slot.AttachItem(_currentItem);
        }

        _items.Add(_currentItem);

        GameManager.Instance.ItemManager.UpdateInventoryItemList(_currentItem.Data);
        GameManager.Instance.ItemManager.SaveItems();
    }


    private void SetInventoryCorners()
    {
        _topLeft = new Vector3(_inventory[0, 0].Position.x, _inventory[0, 0].Position.y, 0);
        _topRight = new Vector3(_inventory[0, _columns - 1].Position.x, _inventory[0, _columns - 1].Position.y, 0);

        _bottomLeft = new Vector3(_inventory[_lines - 1, 0].Position.x, _inventory[_lines - 1, 0].Position.y, 0);
        _bottomRight = new Vector3(_inventory[_lines - 1, _columns - 1].Position.x, _inventory[_lines - 1, _columns - 1].Position.y, 0);

        Debug.Log("Top Left:" + _topLeft);
        Debug.Log("Bottom Left:" + _bottomLeft);
        Debug.Log("Top Right:" + _topRight);
        Debug.Log("Bottom Right:" + _bottomRight);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_topLeft, 4f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_bottomLeft, 4f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_topRight, 4f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_bottomRight, 4f);
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

    private void UpdateQtd(int id)
    {
        var item = _items.FirstOrDefault(x => x.Data.id == id);
        if (item != null)
        {
            item.UpdateQtd();
            item.Qtd++;
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
