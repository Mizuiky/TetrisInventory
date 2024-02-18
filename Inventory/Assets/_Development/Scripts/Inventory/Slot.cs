using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image _slot;

    private ItemData _item;
    private int _itemQtd;

    private Vector2 _position;
    private Vector2 _index;

    public int itemQtd { get { return _itemQtd; } }
    public Vector2 Position { get { return _position; } }
    public Vector2 Index { get { return _index; } }

    private Sprite _highlight;
    private Sprite _normal;

    public void Start()
    {
        if (_slot == null)
            _slot = GetComponent<Image>();
    }

    public void Init(Sprite normal, Sprite highLighted, Vector2 position, Vector2 index)
    {
        _slot.sprite = normal;

        _normal = normal;
        _highlight = highLighted;

        _slot.type = Image.Type.Simple;
        _position = position;
        _index = index;
    }

    public void Reset()
    {
        _item = null;
        _itemQtd = 0;
    }

    public void UpdateQtd(int value)
    {
        _itemQtd += value;
    }

    public void AddItem(ItemData item)
    {
        _item = item;
    }

    public void HighLight(bool canHighLight)
    {
        if (canHighLight)
            _slot.sprite = _highlight;
        else
            _slot.sprite = _normal;
    }
}
