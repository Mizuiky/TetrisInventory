using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public int ItemIdToSpawn;

    public GameObject Spawn(GameObject objToSpawn, Transform parent)
    {
        return Instantiate(objToSpawn, parent);
    }

    public void AddItemToInventory()
    {
        var item = GameManager.Instance.ItemManager.GetItemById(ItemIdToSpawn);
        GameManager.Instance.ItemManager.SendItemDataToInventory(item);
    }

    public InventoryItem SpawnInventoryItem(InventoryItem item, Transform _inventoryItemsParent)
    {
        var newItem = Spawn(item.gameObject, _inventoryItemsParent);

        if (newItem != null)
        {
            var itemToAdd = newItem.GetComponent<InventoryItem>();
            itemToAdd.Data = item.Data;

            itemToAdd.InitializeComponent();
            return itemToAdd;
        }

        return null;
    }
}
