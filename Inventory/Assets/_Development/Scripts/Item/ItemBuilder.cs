using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuilder : MonoBehaviour
{
    [SerializeField] GameObject _QtdPrefabField;
    private List<ItemData> _itemData;
    private List<InventoryItemData> _inventoryItemData;

    private string _ResourceFolderItemPath = "Assets/_Development/Resources/Prefabs/Items";
    private string _inventoryItemFolderPath = "Assets/_Development/Resources/Prefabs/InventoryItems/";
    private string _itemPath;

    private bool _hasItem = false;

    public void Init()
    {
        _itemData = new List<ItemData>();
        _inventoryItemData = new List<InventoryItemData>();

        LoadData();
        LoadPrefabs();
    }

    public void Reset()
    {
       
    }

    private void LoadData()
    {
        var loadItemData = GameManager.Instance.SaveManager.ItemData;
        if (loadItemData != null)
            _itemData = loadItemData;

        var loadInventoryIntemData = GameManager.Instance.SaveManager.InventoryItemData;
        if (loadInventoryIntemData != null)
            _inventoryItemData = loadInventoryIntemData;
    }

    public void AddItem(string name, int id, ItemType type, Sprite sprite, string itemDescription, Sprite inventoryImage, int[,] itemConfig)
    {
        _hasItem = false;
        var slotPositionLenght = itemConfig.GetLength(0) * itemConfig.GetLength(1);

        _hasItem = GameManager.Instance.ItemManager.CheckHasItem(id);
        
        if(_hasItem)
        {
            Debug.Log("Cant Add Item - Same ID");
            return;
        }

        ItemData data = new ItemData()
        {
            itemName = name,
            id = id,
            type = (int)type,
            spriteName = sprite.name,             
        };

        _itemData.Add(data);

        InventoryItemData inventoryItemData = new InventoryItemData()
        {
            imageName = inventoryImage.name,
            name = name,
            id = id,
            description = itemDescription,
            slotPosition = new SlotPosition[slotPositionLenght],
            imageConfig = new int[itemConfig.GetLength(0), itemConfig.GetLength(1)]
        };

        inventoryItemData.imageConfig = itemConfig;
        _inventoryItemData.Add(inventoryItemData);

        CreateInventoryItemPrefab(data, inventoryItemData, inventoryImage, sprite);

        GameManager.Instance.SaveManager.Save(_itemData, FileType.ItemData);
        GameManager.Instance.SaveManager.Save(_inventoryItemData, FileType.InventoryData);
    }

    private void LoadPrefabs()
    {
        GameObject[] itemList = Resources.LoadAll<GameObject>("Prefabs/Items");
        GameObject[] inventoryItemList = Resources.LoadAll<GameObject>("Prefabs/InventoryItems");

        List<ItemBase> tempItemBaseList = new List<ItemBase>();
        List<InventoryItem> tempInventoryItemList = new List<InventoryItem>();

        if (itemList.Length > 0 && inventoryItemList.Length > 0)
        {
            for(int i = 0; i < itemList.Length; i++)
            {
                ItemBase item = itemList[i].GetComponent<ItemBase>();
                item.Data = _itemData.FirstOrDefault(x=>x.id == item.Data.id);
                tempItemBaseList.Add(item);

                InventoryItem inventoryItem = inventoryItemList[i].GetComponent<InventoryItem>();
                inventoryItem.Data = _inventoryItemData.FirstOrDefault(x => x.id == inventoryItem.Data.id);
                inventoryItem.InitializeComponent();
                tempInventoryItemList.Add(inventoryItem);
            }

            GameManager.Instance.ItemManager.FillItemList(tempItemBaseList, tempInventoryItemList);
        }        
    }

    #region Create Item and Inventory Item Prefab
    private void CreateInventoryItemPrefab(ItemData itemData, InventoryItemData inventoryData, Sprite inventoryImage, Sprite sprite)
    {
        GameObject item = new GameObject(itemData.itemName);

        item.AddComponent<Image>();
        item.AddComponent<ColorTint>();
        item.AddComponent<MovementController>();
        InventoryItem currentInventoryItem = item.AddComponent<InventoryItem>();
        
        Image img = currentInventoryItem.GetComponent<Image>();
        img.sprite = inventoryImage;

        var height = img.sprite.rect.height;
        var width = img.sprite.rect.width;

        var bottomRightCorner = new Vector3(width/2 -32, - height/2 + 32, 0);

        _QtdPrefabField = GameManager.Instance.ItemSpawner.Spawn(_QtdPrefabField, currentInventoryItem.transform);
        _QtdPrefabField.gameObject.transform.localPosition = bottomRightCorner;

        if (currentInventoryItem != null)
        {
            currentInventoryItem.Init(inventoryData, inventoryImage);

            _itemPath = Path.Combine(_inventoryItemFolderPath, itemData.itemName + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(currentInventoryItem.gameObject, _itemPath);

            CreateItemPrefab(itemData, sprite, currentInventoryItem);
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

            _itemPath = Path.Combine(_ResourceFolderItemPath, data.itemName + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(currentItem.gameObject, _itemPath);

            GameManager.Instance.ItemManager.AddNewItem(currentItem, newInventoryItem);
        }       
    }

    #endregion
}
