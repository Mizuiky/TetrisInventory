using UnityEngine;

public enum ItemType
{
    None,
    Material,
    Weapon,
    Ammo,
    Consumable
}

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData _data;
    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;
    private int _qtd;

    public void Init(ItemData data)
    {
        _data = data;
        _data.inventoryIndex = new int[2];
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
        _sprite = sprite;
    }

    public void SetInventoryIndex(int line, int column)
    {
        _data.inventoryIndex[0] = line;
        _data.inventoryIndex[1] = column;
    }

    private void UpdateBoxCollider()
    {

    }
}
