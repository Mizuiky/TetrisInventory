using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private ItemSpawner _spawner;

    [SerializeField] private Color[] _colors;

    private InventoryController _inventoryController;

    public void Init()
    {
        GameManager.Instance.ItemManager.OnUpdateItem += UpdateInventory;
        MovementController.OnVerifyNextSlotAvailability += VerifySlotAvailability;
        _inventoryController = new InventoryController(_spawner);
    }

    public void OpenInventory()
    {
        if(_inventoryController != null)
            _inventoryController.Open();
    }

    private bool VerifySlotAvailability(int itemID, int line, int column)
    {
        return _inventoryController.FindConflictOnNextMove(itemID, line, column);
    }

    public void UpdateInventory(InventoryItem item)
    {
        _inventoryController.SetItem(item);
    }

    public void SetInventory(Inventory inventory, float slotWidth, float slotHeight)
    {
        _inventoryController.Init(inventory, slotWidth, slotHeight);
    }

    public InventoryItem GetItem(int id)
    {
        return _inventoryController.GetInventoryItem(id);
    }
}
