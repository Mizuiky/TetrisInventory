using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemBuilder : MonoBehaviour
{
    private string _itemDataDirectory = "GameData";
    private string _itemDataFile = "ItemsData.json";
    private string _directoryPath;
    private string _itemFolderPath = "Assets/_Development/Prefabs/Items/";
    private string _itemPath;

    [SerializeField] List<ItemData> _itemData;
    List<Item> _items;
    private bool _hasItem = false;

    public void Init()
    {
        _directoryPath = Path.Combine(Application.persistentDataPath, _itemDataDirectory);
        _itemDataFile = Path.Combine(_directoryPath, _itemDataFile);

        _itemData = new List<ItemData>();
        _items = new List<Item>();

        ReadList();
    }

    public void Reset()
    {
       
    }

    public void AddItem(string name, int id, ItemType type, Sprite sprite, string itemDescription)
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
                itemDescription = itemDescription
            };

            _itemData.Add(data);

            CreatePrefab(data, name, sprite);

            SaveList();
        }
    }

    #region Read and Save

    private void ReadList()
    {
        if (!Directory.Exists(_directoryPath))
        {
            Directory.CreateDirectory(_directoryPath);
            _itemData = new List<ItemData>();

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

    private void CreatePrefab(ItemData data, string itemName, Sprite sprite)
    {
        GameObject itemObj = new GameObject(itemName);

        itemObj.AddComponent<SpriteRenderer>();
        itemObj.AddComponent<BoxCollider>();

        Item currentItem = itemObj.AddComponent<Item>();

        if(currentItem != null) 
        {
            currentItem.Init(data);
            currentItem.SetSprite(sprite);

            _items.Add(currentItem);

            _itemPath = Path.Combine(_itemFolderPath, itemName + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(currentItem.gameObject, _itemPath);
        }       
    }

    //private Sprite[] GetSprite()
    //{
    //   //based on sprite name get them from resources load
    //}
}
