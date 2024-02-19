using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemBuilder
{
    private string _itemDataDirectory = "GameData";
    private string _itemDataFile = "ItemData.txt";
    private string _directoryPath;

    private List<ItemData> _itemData;
    private List<Item> _items;

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
        var data = new ItemData()
        {
            itemName = name,
            id = id,
            type = (int)type,
            spriteName = sprite.name,
            itemDescription = itemDescription
        };

        _itemData.Add(data);

        var item = new Item();
        item.Init(data);
        item.SetSprite(sprite);

        _items.Add(item);

        SaveList();
    }

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
                        //_itemData = JsonHelper.Deserialize<List<ItemData>>(jsonData);
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

    //private Sprite[] GetSprite()
    //{
    //   //based on sprite name get them from resources load
    //}
}
