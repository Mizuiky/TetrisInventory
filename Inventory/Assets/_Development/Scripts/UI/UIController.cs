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

    public void OpenInventory()
    {
        if(_inventory != null)
        {
            if (!_inventory.gameObject.activeInHierarchy)
            {
                _inventory.Open(true);
            }
            else
            {
                _inventory.Open(false);
            }
        }        
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
