using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image _slot;
    [SerializeField] private int[] _index;

    private IInventoryItem _inventoryItem;

    private Vector2 _position;
    public Vector2 Position { get { return _position; } }
    public int[] Index { get { return _index; } }
    public bool HasItem { get { return _inventoryItem != null; } }

    private Sprite _highlight;
    private Sprite _normal;  

    public void Start()
    {
        if (_slot == null)
            _slot = GetComponent<Image>();
    }

    public void Init(Sprite normal, Sprite highLighted, Vector2 position, int line, int column)
    {
        _slot.sprite = normal;

        _normal = normal;
        _highlight = highLighted;

        _slot.type = Image.Type.Simple;
        _position = position;

        _index = new int[2];
        _index[0] = line;
        _index[1] = column;
    }

    public void Reset()
    {
        _inventoryItem = null;
    }

    public void UpdateQtd(int value)
    {
        _inventoryItem.Data.qtd += value;
    }

    public void AddItem(IInventoryItem item)
    {
        _inventoryItem = item;
    }

    public void RemoveItem()
    {
        _inventoryItem = null;
    }

    public void HighLight(bool canHighLight)
    {
        if (canHighLight)
            _slot.sprite = _highlight;
        else
            _slot.sprite = _normal;
    }
}
