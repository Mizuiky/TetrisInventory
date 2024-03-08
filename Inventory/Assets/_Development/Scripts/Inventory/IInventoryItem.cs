using UnityEngine;
using UnityEngine.UI;

public interface IInventoryItem
{
    public InventoryItemData Data { get; }
    public Image Image { get; }
    public int Qtd { get; set; }
    public RectTransform Rect { get; set; }
    public bool IsSelected { get; }
    public Move Move { get; }
    public void Init(InventoryItemData data, Sprite sprite);
    public void UpdateQtd();
    public void OnUse();
}
