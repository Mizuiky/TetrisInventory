using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Inventory _inventory;

    public void Init()
    {
        GameManager.Instance.ItemManager.OnUpdateItem += UpdateInventory;
    }

    public void OpenInventory(bool canOpen)
    {
        _inventory.Open(canOpen);
    }

    public void UpdateInventory(GameObject item, int id)
    {
        _inventory.SetItem(item, id);
    }

    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
    }
}
