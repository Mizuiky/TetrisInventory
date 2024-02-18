using UnityEngine;

public enum ItemType
{
    Material,
    Weapon,
    Ammo,
    Consumable
}

public class Item : MonoBehaviour
{
    private ItemData _data;

    public void Init()
    {

    }
}
