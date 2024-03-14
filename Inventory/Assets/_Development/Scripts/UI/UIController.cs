using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Color[] _colors;

    public void Init()
    {
        GameManager.Instance.ItemManager.OnUpdateItem += UpdateInventory;
        MovementController.OnVerifyNextSlotAvailability += VerifySlotAvailability;
    }

    public void OpenInventory()
    {
        if(_inventory != null)
        {
            if(_inventory.gameObject.activeSelf)
                _inventory.gameObject.SetActive(false);
            else
                _inventory.gameObject.SetActive(true);
        }        
    }

    private bool VerifySlotAvailability(int itemID, int line, int column)
    {
        return _inventory.FindConflictOnNextMove(itemID, line, column);
    }

    public void UpdateInventory(InventoryItem item)
    {
        _inventory.SetItem(item);
    }

    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;
    }

    public InventoryItem GetItem(int id)
    {
        return _inventory.GetInventoryItemById(id);
    }
}
