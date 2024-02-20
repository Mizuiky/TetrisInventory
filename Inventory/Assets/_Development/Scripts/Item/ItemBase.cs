using UnityEngine;

public enum ItemType
{
    None,
    Material,
    Weapon,
    Ammo,
    Consumable
}

public class ItemBase : MonoBehaviour, IItem
{
    private ItemData _data;
    private ItemBase _item;
    private IInventoryItem _inventoryItem;
    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;

    public ItemData Data { get { return _data; } }
    public ItemBase Item { get { return _item; } }
    public IInventoryItem InventoryItem { get { return _inventoryItem; } }  
    public int ID { get { return _data.id; } }

    public void Init(ItemData data, IInventoryItem inventoryItem)
    {
        _data = data;
        _inventoryItem = inventoryItem;
        _item = this;
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
        _sprite = sprite;
    }

    public void UpdateBoxCollider()
    {

    }

    public virtual void OnUse()
    {

    }
}


