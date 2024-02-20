using UnityEngine;

public interface IInventoryItem
{
    public InventoryItemData Data { get; }
    public void Init(InventoryItemData data, Sprite sprite);
    public void Rotate();
    public void UpdateQtd(int value);
}
