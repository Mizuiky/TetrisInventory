using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemBuilder
{
    private string itemDataDirectory = "GameData";
    private string itemDataDirectoryPath;

    private List<ItemData> _items;

    public void Init()
    {
        itemDataDirectoryPath = Path.Combine(Application.persistentDataPath, itemDataDirectory);
        ReadList();
    }

    public void AddItem(string name, ItemType type, int qtd, int slotNumber, Sprite[] sprites, string itemDescription)
    {
        var item = new ItemData()
        {
            itemName = name,
            type = type,
            qtd = qtd,
            slotNumber = slotNumber,
            itemDescription = itemDescription
        };

        item.SetSprites(sprites);
        _items.Add(item); 
    }

    private void ReadList()
    {
        if(!Directory.Exists(itemDataDirectoryPath))
        {
            Directory.CreateDirectory(itemDataDirectoryPath);
            _items = new List<ItemData>();
        }
           
        else
        {
            try
            {
                string jsonData = File.ReadAllText(itemDataDirectoryPath);

                if (jsonData != null)
                {
                    _items = JsonHelper.Deserialize<List<ItemData>>(jsonData);
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
        string jsonData = JsonHelper.Serialize(_items);

        if(jsonData != null)
        {
            try
            {
                File.WriteAllText(itemDataDirectoryPath, jsonData);
            }
            catch(Exception e)
            {
                Debug.LogWarning("Exception catch when writing list: " + e);
            }    
        }
    }
}
