using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Spawn(GameObject objToSpawn, Transform parent)
    {
        return Instantiate(objToSpawn, parent);
    }

    public void AddItemToInventory(int id)
    {
        var item = GameManager.Instance.ItemManager.GetItemAtIndex(id);
        GameManager.Instance.ItemManager.UpdateQtd(item);
    }
}
