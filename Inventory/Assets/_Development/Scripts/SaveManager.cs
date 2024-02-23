using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public enum FileType
{
    ItemData,
    InventoryData
}

public class SaveManager
{
    [SerializeField] List<ItemData> _itemData;
    List<SlotData> _inventorySlotData;

    private string _directoryPath;
    private string _GameDataDirectory = "GameData";
    private string _itemDataFile = "ItemData.json";
    private string _inventoryFile = "InventoryData.json";

    private string _itemPath;
    private string _inventoryPath;

    public List<ItemData> ItemData { get { return _itemData; } }
    public List<SlotData> InventoryData { get { return _inventorySlotData; } }

    public void Init()
    {
        _directoryPath = Path.Combine(Application.persistentDataPath, _GameDataDirectory);
        _itemPath = Path.Combine(_directoryPath, _itemDataFile);
        _inventoryPath = Path.Combine(_directoryPath, _inventoryFile);

        _itemData = new List<ItemData>();
        _inventorySlotData = new List<SlotData>();

        //Load items Data
        LoadData();
    }

    public void LoadData()
    {
        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);      
       
        var ItemDataRead = Read<ItemData>(_itemPath, out List<ItemData> itemData);
        if(!ItemDataRead)
        {
            try
            {
                File.WriteAllText(_itemPath, "New Item Data File");
                Debug.Log("Inventory Data sucessfull created");
            }
            catch(Exception e) 
            {
                Debug.LogWarning("Exception catch when reading list: " + e);
            }              
        }
                

        var InventoryDataRead = Read<SlotData>(_inventoryPath, out List<SlotData> inventoryData);
        if (!InventoryDataRead)
        {
            try
            {
                File.WriteAllText(_inventoryPath, "New Inventory Data File");
                Debug.Log("Inventory Data sucessfull created");
            }
            catch (Exception e)
            {
                Debug.LogWarning("Exception catch when reading list: " + e);
            }
        }
    }

    public bool Read<T>(string file, out List<T> result)
    {
        result = default(List<T>);
        
        try
        {
            if (File.Exists(file))
            {
                string jsonData = File.ReadAllText(file);

                if (!string.IsNullOrEmpty(jsonData))
                {
                    result = JsonHelper.Deserialize<List<T>>(jsonData);

                   if(result != null)
                   {
                       Debug.Log("Data file sucessfull read");
                       return true;
                   }  
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Exception catch when reading list: " + e);
        }

        return false;
    }

    public void Save<T>(List<T>list, FileType type)
    {
        string jsonData = JsonHelper.Serialize(list);

        if (jsonData != null)
        {
            try
            {
                if (type == FileType.ItemData)
                    File.WriteAllText(_itemPath, jsonData);

                else if (type == FileType.InventoryData)
                    File.WriteAllText(_inventoryPath, jsonData);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Exception catch when writing list: " + e);
            }
        }
    }
}
