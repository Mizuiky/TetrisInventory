using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private InventoryBuilder _inventoryBuilder;
    [SerializeField] private ItemBuilder _itemBuilder;
    [SerializeField] private UIController _uiController;
    [SerializeField] private ItemSpawner _itemSpawner;
    private SaveManager _saveManager;

    private ItemManager _itemManager;
    public ItemManager ItemManager { get { return _itemManager; } }
    public ItemBuilder ItemBuilder { get { return _itemBuilder; } }
    public UIController UIController { get { return _uiController; } }
    public ItemSpawner ItemSpawner { get {  return _itemSpawner; } }
  
    public SaveManager SaveManager { get { return _saveManager; } }

    public void Start()
    {
        Init();
    }

    private void Init()
    {
        _saveManager = new SaveManager();
        _saveManager.Init();

        _itemManager = new ItemManager();
        _itemManager.Init();

        UIController.Init();

        _itemBuilder.Init();
        _inventoryBuilder.Init();
    }
}
