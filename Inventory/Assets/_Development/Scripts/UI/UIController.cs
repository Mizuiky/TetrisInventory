using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private ItemSpawner _spawner;

    [SerializeField] private Color[] _colors;
    [SerializeField] private TextMeshProUGUI _inventoryItemDescription;

    private InventoryController _inventoryController;

    public void Init()
    {
        GameManager.Instance.ItemManager.OnUpdateItem += UpdateInventory;
        MovementController.OnVerifyNextSlotAvailability += VerifySlotAvailability;
        DetectMouseEvent.OnSetItemDescription += SetDescription;

        _inventoryItemDescription.text = "";
        _inventoryController = new InventoryController(_spawner, _colors);
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

    public void SetDescription(string description) 
    {
        _inventoryItemDescription.text = description; 
    }
}
