using UnityEngine;

public class InventoryBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _itemParent;

    [SerializeField] private Sprite _normalSlot;
    [SerializeField] private Sprite _highLightedSlot;

    [SerializeField] private float _startX;
    [SerializeField] private float _startY;

    [SerializeField] private int _lines;
    [SerializeField] private int _columns;

    private float _slotWidth;
    private float _slotHeight;

    private GameObject _square;

    private Vector2 _position;

    private float _x;
    private float _y;

    private Slot[,] _inventory;

    public void Init()
    {
        _x = _startX;
        _y = _startY;

        _inventory = new Slot[_lines, _columns];

        GetSlotSize();

        BuildSlots();
    }

    private void BuildSlots()
    {
        for(int i = 0; i < _lines; i++)
        {
            for(int j = 0; j < _columns; j++)
            {
                if (j == 0 && i != 0)
                {
                    _x = _startX * i;
                    _y -= _slotWidth;
                }

                CreateSlot(_x, _y, i, j);

                _x += _slotWidth;
            }
        }

        BuildInventory();
    }

    private void CreateSlot(float x, float y, int i, int j)
    {
        _position = new Vector2(x, y);

        _square = Instantiate(_slotPrefab, _parent);
        _square.transform.localPosition = _position;

        if(_square != null)
        {
            var slot = _square.GetComponent<Slot>();
            if (slot != null)
            {
                slot.Init(_normalSlot, _highLightedSlot, _position, i, j, _slotWidth, _slotHeight);
                _inventory[i, j] = slot;
            }             
        }
    }

    private void BuildInventory()
    {
        GameObject obj = new GameObject("Inventory");
        obj.AddComponent<Inventory>();

        Inventory inventory = obj.AddComponent<Inventory>();
        if (inventory != null)
        {
            inventory.Init(_inventory, _itemParent, _slotWidth, _slotHeight);
            GameManager.Instance.UIController.SetInventory(inventory);
        }        
    }

    private void GetSlotSize()
    {
        var rect = _slotPrefab.GetComponent<RectTransform>();
        _slotWidth = rect.sizeDelta.x;
        _slotHeight = rect.sizeDelta.y;
    }
}
