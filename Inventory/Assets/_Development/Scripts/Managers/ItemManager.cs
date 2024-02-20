using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<ItemBase> items;
    public Action<IInventoryItem> OnUpdateItem;

    public void Init()
    {
        items = new List<ItemBase>();
    }

    public void AddItem(ItemBase item)
    {
        var currentItem = items.FirstOrDefault(x => x.Data.id == item.Data.id);
        
        if(currentItem == null)
        {
            currentItem = item;
            items.Add(currentItem);
        }      
        
        currentItem.Data.inventoryItemData.qtd += 1;

        UpdateInventory(currentItem.InventoryItem);
    }

    public void UpdateInventory(IInventoryItem item)
    {
        OnUpdateItem?.Invoke(item);
    }
}
