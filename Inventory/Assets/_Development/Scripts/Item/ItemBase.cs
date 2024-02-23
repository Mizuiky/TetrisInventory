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
    [SerializeField] private ItemData _data;
    private InventoryItemData _inventoryData;
    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;

    public ItemData Data { get { return _data; } set { _data = value; } }
    public InventoryItemData InventoryData { get { return _inventoryData; } set { _inventoryData = value; } } 
    public int ID { get { return _data.id; } }

    public void Init(ItemData data, Sprite sprite)
    {
        _data = data;
        _sprite = sprite;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
    }

    public void UpdateBoxCollider()
    {

    }
}


