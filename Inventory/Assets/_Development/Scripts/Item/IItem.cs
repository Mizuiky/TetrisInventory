using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public ItemData Data { get; }
    public ItemBase Item { get; }
    public IInventoryItem InventoryItem { get; }
    public int ID { get; }
    public void Init(ItemData data, IInventoryItem inventoryItem);
    public void SetSprite(Sprite sprite);
    public void UpdateBoxCollider();
    public void OnUse();
}
