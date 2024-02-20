using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ItemBuilder : MonoBehaviour
{
    [SerializeField] List<ItemData> _itemData;

    private string _directoryPath;
    private string _GameDataDirectory = "GameData";
    private string _itemDataFile = "ItemDatas.json";    
    private string _itemFolderPath = "Assets/_Development/Prefabs/Items/";
    private string _inventoryItemFolderPath = "Assets/_Development/Prefabs/UI/InventoryItems/";
    private string _itemPath;

    private bool _hasItem = false;

    public void Init()
    {
        _directoryPath = Path.Combine(Application.persistentDataPath, _GameDataDirectory);
        _itemDataFile = Path.Combine(_directoryPath, _itemDataFile);

        _itemData = new List<ItemData>();

        ReadList();
    }

    public void Reset()
    {
       
    }

    public void AddItem(string name, int id, ItemType type, Sprite sprite, string itemDescription, Sprite inventoryImage)
    {
        if(_itemData.Count > 0)
            _hasItem = _itemData.Any(x => x.id == id);
        
        if(!_hasItem)
        {
            ItemData data = new ItemData()
            {
                itemName = name,
                id = id,
                type = (int)type,
                spriteName = sprite.name,       
                inventoryItemData = new InventoryItemData()
                {
                    imageName = inventoryImage.name,
                    name = name,
                    id = id,
                    description = itemDescription,
                }        
            };

            _itemData.Add(data);

            CreateInventoryItemPrefab(data, inventoryImage, sprite);

            SaveList();
        }
    }

    #region Read and Save

    private void ReadList()
    {
        if (!Directory.Exists(_directoryPath))
        {
            Directory.CreateDirectory(_directoryPath);

            File.WriteAllText(_itemDataFile, "New Item Data File");
            Debug.Log("File and directory sucessfull created");
        }         
        else
        {
            try
            {
                if (File.Exists(_itemDataFile))
                {
                    string jsonData = File.ReadAllText(_itemDataFile);

                    if (jsonData != null)
                    {
                        _itemData = JsonHelper.Deserialize<List<ItemData>>(jsonData);
                        Debug.Log("File sucessfull read");
                    }
                }
                else
                {
                    File.WriteAllText(_itemDataFile, "New Item Data File");
                    Debug.Log("File sucessfull created");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Exception catch when reading list: "+ e);

            }          
        }
    }

    private void SaveList()
    {    
        string jsonData = JsonHelper.Serialize(_itemData);

        if(jsonData != null)
        {
            try
            {
                File.WriteAllText(_itemDataFile, jsonData);
            }
            catch(Exception e)
            {
                Debug.LogWarning("Exception catch when writing list: " + e);
            }    
        }
    }

    #endregion

    #region Create Item and Inventory Item Prefab
    private void CreateItemPrefab(ItemData data, Sprite sprite, IInventoryItem inventoryItem)
    {
        GameObject itemObj = new GameObject(data.itemName);

        itemObj.AddComponent<SpriteRenderer>();
        itemObj.AddComponent<BoxCollider>();

        ItemBase currentItem = itemObj.AddComponent<ItemBase>();

        if(currentItem != null) 
        {
            currentItem.Init(data, inventoryItem);
            currentItem.SetSprite(sprite);

            _itemPath = Path.Combine(_itemFolderPath, data.itemName + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(currentItem.gameObject, _itemPath);
        }       
    }

    private void CreateInventoryItemPrefab(ItemData data, Sprite inventoryImage, Sprite sprite)
    {
        GameObject item = new GameObject(data.itemName);

        item.AddComponent<Image>();
        
        InventoryItem currentItem = item.AddComponent<InventoryItem>();

        if (currentItem != null)
        {
            currentItem.Init(data.inventoryItemData, inventoryImage);

            _itemPath = Path.Combine(_inventoryItemFolderPath, data.inventoryItemData.name + ".prefab");
            GameObject newInventoryItem = PrefabUtility.SaveAsPrefabAsset(currentItem.gameObject, _itemPath);

            IInventoryItem invItem = newInventoryItem.GetComponent<IInventoryItem>();

            if(invItem != null)
                CreateItemPrefab(data, sprite, invItem);               
        }
    }

    #endregion

    //private Sprite[] LoadSprite()
    //{
    //   //based on sprite name get them from resources load
    //}
    //
}
