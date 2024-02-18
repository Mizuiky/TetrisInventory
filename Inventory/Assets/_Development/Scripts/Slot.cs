using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image _image;
    private Item _item;
    private int _itemQtd;

    private Vector2 _position;
    private Vector2 _index;

    public int itemQtd { get { return _itemQtd; } }
    public Vector2 Position { get { return _position; } }
    public Vector2 Index { get { return _index; } }


    public void Start()
    {
        if (_image == null)
            _image = GetComponent<Image>();
    }

    public void Init(Sprite slotImage, Vector2 position, Vector2 index)
    {
        _image.sprite = slotImage;
        _image.type = Image.Type.Simple;
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

    public void AddItem(Item item)
    {
        _item = item;
    }


}
