using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Globalization;

public enum FileType
{
    ItemData,
    InventoryData
}

public class SaveManager
{
    private List<ItemData> _itemData;
    private List<InventoryItemData> _inventoryItemData;

    private string _directoryPath;
    private string _SaveDataDirectory = "SaveData";
    private string _itemDataFileName = "ItemData.json";
    private string _inventoryItemFileName = "InventoryItemData.json";
    //add one file to inventory data

    private string _itemDataPath;
    private string _inventoryItemDataPath;

    public List<ItemData> ItemData { get { return _itemData; } }
    public List<InventoryItemData> InventoryItemData { get { return _inventoryItemData; } }

    public void Init()
    {
        _directoryPath = Path.Combine(Application.persistentDataPath, _SaveDataDirectory);
        _itemDataPath = Path.Combine(_directoryPath, _itemDataFileName);
        _inventoryItemDataPath = Path.Combine(_directoryPath, _inventoryItemFileName);

        _itemData = new List<ItemData>();
        _inventoryItemData = new List<InventoryItemData>();

        //Load items Data
        LoadData();
    }

    public void LoadData()
    {
        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);      

        SetPathToRead<ItemData>(_itemDataPath, _itemDataFileName, FileType.ItemData);
        SetPathToRead<InventoryItemData>(_inventoryItemDataPath, _inventoryItemFileName, FileType.InventoryData);
    }

    private void SetPathToRead<T>(string path, string fileName, FileType type)
    {
        List<T> data = null;
        bool canReadData = false;

        canReadData = Read<T>(path, out data);

        if (!canReadData)
        {
            try
            {
                File.WriteAllText(path, fileName);
                Debug.Log($"{fileName} sucessfull created");
            }
            catch
            {
                Debug.LogWarning("Exception catch when writing list: Couldnt create new one");
            }
        }
        else
        {          
            if(data != null)
            {
                if (type == FileType.ItemData)
                    _itemData = data as List<ItemData>;
                else if (type == FileType.InventoryData)
                    _inventoryItemData = data as List<InventoryItemData>;
            }
        }         
    }

    public bool Read<T>(string path, out List<T> result)
    {
        result = null;
        
        try
        {
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);

                if (!string.IsNullOrEmpty(jsonData))
                {                   
                    result = JsonHelper.Deserialize<List<T>>(jsonData);

                    if (result != null)
                        Debug.Log("Data file sucessfull read");
                    return true;                  
                }
            }
        }
        catch
        {
            Debug.LogWarning("Exception catch when reading list: Couldn`t deserialize file");
            return true;
        }

        return false;
    }

    public void Save<T>(List<T>list, FileType type)
    {
        string jsonData = JsonHelper.Serialize(list);
        string path = "";

        if (type == FileType.ItemData)
            path = _itemDataPath;

        else if (type == FileType.InventoryData)
            path = _inventoryItemDataPath;

        if (jsonData != null)
        {
            try
            { 
                using(StreamWriter writer = new StreamWriter(path, false))
                { 
                    writer.Write(jsonData);
                }    
            }
            catch
            {
                Debug.LogWarning("Exception catch when writing list: Couldn`t serialize file");
            }
        }
    }
}
