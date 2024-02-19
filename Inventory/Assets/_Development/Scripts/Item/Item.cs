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
    private ItemData _data;
    private Sprite _sprite;
    private int _qtd;

    public void Init(ItemData data)
    {
        _data = data;
        _data.inventoryIndex = new int[2];
    }

    public void SetSprite(Sprite sprite)
    {
        _sprite = sprite;
    }

    public void SetInventoryIndex(int line, int column)
    {
        _data.inventoryIndex[0] = line;
        _data.inventoryIndex[1] = column;
    }
}
