using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
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
}
