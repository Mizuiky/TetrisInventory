using UnityEngine;
using UnityEngine.UI;

public interface IInventoryItem
{
    public InventoryItemData Data { get; }
    public Image Image { get; }
    public void Init(InventoryItemData data, Sprite sprite);
    public void Rotate();
    public void UpdateQtd();
    public void OnUse();
}
