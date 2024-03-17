using System;
using System.Collections.Generic;
using System.Linq;

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

    public void SendItemDataToInventory(ItemBase item)
    {
        var inventoryItem = _inventoryItems.FirstOrDefault(x => x.Data.id == item.Data.id);
        if(inventoryItem != null)
            OnUpdateInventory(inventoryItem);
    }

    public void OnUpdateInventory(InventoryItem item)
    {
        OnUpdateItem?.Invoke(item);
    }

    public void FillItemList(List<ItemBase> itemBase, List<InventoryItem> inventoryItems)
    {
        _items = itemBase;
        _inventoryItems = inventoryItems;
    }

    public void AddNewItem(ItemBase item, InventoryItem newInventoryItem)
    {
        _items.Add(item);
        _inventoryItems.Add(newInventoryItem);
    }

    public ItemBase GetItemById(int id)
    {
        return _items.FirstOrDefault(x=>x.Data.id == id);
    }

    public bool CheckHasItem(int id)
    {
        if(_items.Count == 0) return false;
        return _items.FirstOrDefault(x => x.Data.id == id);
    }

    public void UpdateInventoryItemList(InventoryItemData itemData)
    {
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
