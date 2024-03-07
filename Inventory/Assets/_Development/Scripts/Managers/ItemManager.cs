using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager
{
    public List<ItemBase> _items;
    public List<InventoryItem> _inventoryItems;
    public Action<InventoryItem> OnUpdateItem;

    public void Init()
    {
        _items = new List<ItemBase>();
        _inventoryItems = new List<InventoryItem>();
    }

    public void UpdateQtd(ItemBase item)
    {
        var currentItem = _items.FirstOrDefault(x => x.Data.id == item.Data.id);
        var inventoryItem = _inventoryItems.FirstOrDefault(x => x.Data.id == item.Data.id);

        if (currentItem == null)
        {
            currentItem = item;
            currentItem.InventoryData = inventoryItem.Data;
            AddItem(currentItem);
        }

        UpdateInventory(inventoryItem);
    }

    public void UpdateInventory(InventoryItem item)
    {
        OnUpdateItem?.Invoke(item);
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

    public ItemBase GetItemById(int id)
    {
        return _items.FirstOrDefault(x=>x.ID == id);
    }

    public void UpdateInventoryItemList(InventoryItemData itemData)
    {
        var itemToUpdate = _items.FirstOrDefault(x => x.Data.id == itemData.id).Data.inventoryData;
        itemToUpdate = itemData;

        var InventoryItemToUpdate = _inventoryItems.FirstOrDefault(x => x.Data.id == itemData.id);
        InventoryItemToUpdate.Data = itemData;

        //SaveItems();
    }

    public void SaveItems()
    {
        var list = _items.Select(x => x.Data).ToList();
        GameManager.Instance.SaveManager.Save(list, FileType.ItemData);
    }
}
