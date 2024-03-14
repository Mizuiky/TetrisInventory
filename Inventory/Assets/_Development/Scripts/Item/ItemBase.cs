using UnityEngine;

public enum ItemType
{
    None,
    Material,
    Weapon,
    Ammo,
    Consumable,
    Tool,
    Equipment
}

public class ItemBase : MonoBehaviour, IItem
{
    [SerializeField] private ItemData _data;
    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;

    public ItemData Data { get { return _data; } set { _data = value; } }

    public void Init(ItemData data, Sprite sprite)
    {
        _data = new ItemData();
        _data = data;
        _data.spriteName = sprite.name;

        _sprite = sprite;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
    }

    public void UpdateBoxCollider()
    {

    }
}


