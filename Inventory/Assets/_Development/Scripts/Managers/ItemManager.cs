using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager
{
    public List<ItemBase> _items;
    public List<InventoryItem> _inventoryItems;
    public Action<GameObject, int> OnUpdateItem;

    public void Init()
    {
        _items = new List<ItemBase>();
        _inventoryItems = new List<InventoryItem>();
    }

    public void UpdateQtd(ItemBase item)
    {
        var currentItem = _items.FirstOrDefault(x => x.Data.id == item.Data.id);
        
        if(currentItem == null)
        {
            currentItem = item;
            AddItem(currentItem);
        }

        var inventoryItem = _inventoryItems.FirstOrDefault(x => x.Data.id == item.Data.id);
        inventoryItem.Qtd += 1;

        UpdateInventory(inventoryItem.gameObject, inventoryItem.Data.id);
    }

    public void UpdateInventory(GameObject item, int id)
    {
        OnUpdateItem?.Invoke(item, id);
    }

    public void FillItemList(List<ItemBase> itemBase, List<InventoryItem> inventoryItems)
    {
        _items = itemBase;
        _inventoryItems = inventoryItems;
    }

    public void AddItem(ItemBase item)
    {
        _items.Add(item);
    }

    public void AddNewItem(ItemBase item, InventoryItem newInventoryItem)
    {
        _items.Add(item);
        _inventoryItems.Add(newInventoryItem);
    }

    public ItemBase GetItemAtIndex(int index)
    {
        return _items[index];
    }

    public InventoryItemData GetInventoryDataById(int id)
    {
        return _inventoryItems.FirstOrDefault(x=>x.Data.id == id).Data;
    }

    public void UpdateInventoryItemList(InventoryItemData itemData)
    {
        var itemToUpdate = _items.FirstOrDefault(x => x.Data.id == itemData.id).Data.inventoryData;
        itemToUpdate = itemData;

        var InventoryItemToUpdate = _inventoryItems.FirstOrDefault(x => x.Data.id == itemData.id);
        InventoryItemToUpdate.Data = itemData;

        SaveItems();
    }

    public void SaveItems()
    {
        var list = _items.Select(x => x.Data).ToList();
        GameManager.Instance.ItemBuilder.SaveData(list);
    }
}
