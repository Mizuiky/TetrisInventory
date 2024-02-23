using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemBuilder : MonoBehaviour
{
    [SerializeField] List<ItemData> _itemData;
    List<ItemBase> _itemBaseList;
    List<InventoryItem> _inventoryItemList;

    private string _directoryPath;
    private string _GameDataDirectory = "GameData";
    private string _itemDataFile = "ItemData.json";    
    private string _ResourceFolderItemPath = "Assets/_Development/Resources/Prefabs/Items";
    private string _inventoryItemFolderPath = "Assets/_Development/Resources/Prefabs/InventoryItems/";
    private string _itemPath;

    private bool _hasItem = false;

    public void Init()
    {
        _directoryPath = Path.Combine(Application.persistentDataPath, _GameDataDirectory);
        _itemDataFile = Path.Combine(_directoryPath, _itemDataFile);

        _itemData = new List<ItemData>();
        _itemBaseList = new List<ItemBase>();
        _inventoryItemList = new List<InventoryItem>();

        //Load items Data
        LoadList();

        //Load all prefabs of type ItemBase and InventoryItem from Resource folder inside Assets and update them on Item Manager
        LoadPrefabs();
    }

    public void Reset()
    {
       
    }

    public void AddItem(string name, int id, ItemType type, Sprite sprite, string itemDescription, Sprite inventoryImage, int[,] inventoryConfig, int _slotPositions)
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
                inventoryData = new InventoryItemData()
                {
                    imageName = inventoryImage.name,
                    name = name,
                    id = id,
                    description = itemDescription,
                    slotPosition = new SlotPosition[_slotPositions],
                    imageConfig = new int[inventoryConfig.GetLength(0), inventoryConfig.GetLength(1)]
                }        
            };

            data.inventoryData.imageConfig = inventoryConfig;

            _itemData.Add(data);

            CreateInventoryItemPrefab(data, inventoryImage, sprite);
        }
    }

    #region Read, Save AND Load

    private void LoadList()
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

    public void SaveData(List<ItemData> itemData)
    {    
        string jsonData = JsonHelper.Serialize(itemData);

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

    private void LoadPrefabs()
    {
        GameObject[] itemList = Resources.LoadAll<GameObject>("Prefabs/Items");
        GameObject[] inventoryItemList = Resources.LoadAll<GameObject>("Prefabs/InventoryItems");

        if (itemList.Length > 0 && inventoryItemList.Length > 0)
        {
            for(int i = 0; i < itemList.Length; i++)
            {
                ItemBase item = itemList[i].GetComponent<ItemBase>();
                item.Data = _itemData.FirstOrDefault(x=>x.id == item.ID);

                _itemBaseList.Add(item);
                InventoryItem inventoryItem = inventoryItemList[i].GetComponent<InventoryItem>();
                inventoryItem.Data = item.Data.inventoryData;

                _inventoryItemList.Add(inventoryItem);
                _itemData.Add(item.Data);
            }

            GameManager.Instance.ItemManager.FillItemList(_itemBaseList, _inventoryItemList);
        }        
    }

    #endregion

    #region Create Item and Inventory Item Prefab
    private void CreateInventoryItemPrefab(ItemData data, Sprite inventoryImage, Sprite sprite)
    {
        GameObject item = new GameObject(data.itemName);

        item.AddComponent<Image>();

        InventoryItem currentInventoryItem = item.AddComponent<InventoryItem>();

        if (currentInventoryItem != null)
        {
            currentInventoryItem.GetComponent<Image>().sprite = inventoryImage;

            currentInventoryItem.Init(data.inventoryData, inventoryImage);

            _itemPath = Path.Combine(_inventoryItemFolderPath, data.inventoryData.name + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(currentInventoryItem.gameObject, _itemPath);

            CreateItemPrefab(data, sprite, currentInventoryItem);
        }
    }

    private void CreateItemPrefab(ItemData data, Sprite sprite, InventoryItem newInventoryItem)
    {
        GameObject itemObj = new GameObject(data.itemName);

        itemObj.AddComponent<SpriteRenderer>();
        itemObj.AddComponent<BoxCollider>();

        ItemBase currentItem = itemObj.AddComponent<ItemBase>();

        if(currentItem != null) 
        {
            currentItem.Init(data, sprite);
            currentItem.InventoryData = newInventoryItem.Data;

            _itemPath = Path.Combine(_ResourceFolderItemPath, data.itemName + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(currentItem.gameObject, _itemPath);

            GameManager.Instance.ItemManager.AddNewItem(currentItem, newInventoryItem);
        }       
    }

    #endregion
}
