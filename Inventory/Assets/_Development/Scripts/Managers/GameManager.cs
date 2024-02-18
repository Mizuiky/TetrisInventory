using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private InventoryBuilder _inventoryBuilder;

    private ItemBuilder _itemBuilder;
    public ItemBuilder ItemBuilder { get { return _itemBuilder; } }

    public void Start()
    {
        Init();
    }

    private void Init()
    {
        _itemBuilder = new ItemBuilder();
        _itemBuilder.Init();

        _inventoryBuilder.Init();
    }
}
