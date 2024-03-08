using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Color[] _colors;
    [SerializeField] ColorTint _tint;

    private Dictionary<int, Color> _itemColors;

    public void Init()
    {
        InitDictionary();
        _tint = new ColorTint();
        _tint.Init(_itemColors.Count);

        GameManager.Instance.ItemManager.OnUpdateItem += UpdateInventory;
    }

    private void InitDictionary()
    {
        _itemColors = new Dictionary<int, Color>();

        for (int i = 0; i < _colors.Length; i++)
        {
            _itemColors.Add(i, _colors[i]);
        }
    }

    public Color GetColorById(int id)
    {
        return _itemColors[id];
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
