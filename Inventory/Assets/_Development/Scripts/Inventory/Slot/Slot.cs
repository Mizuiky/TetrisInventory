using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rect;
    [SerializeField] private SlotData _slotData;

    private Vector2 _position;
    public Vector2 Position { get { return _position; } }
    public SlotData Data { get { return _slotData; } }
    public bool HasItem { get { return _slotData.hasItem; } set { _slotData.hasItem = value; } }

    private Sprite _highlight;
    private Sprite _normal;

    public void Awake()
    {
        if (_image == null)
            _image = GetComponent<Image>();
        if (_rect == null)
            _rect = GetComponent<RectTransform>();
    }

    public void Init(Sprite normal, Sprite highLighted, Vector2 position, int line, int column, float width, float height)
    {    
        _image.sprite = normal;
        _image.type = Image.Type.Simple;

        _normal = normal;
        _highlight = highLighted;
      
        _position = position;

        _slotData = new SlotData();

        _slotData.index = new int[2];
        _slotData.index[0] = line;
        _slotData.index[1] = column;

        _rect.sizeDelta = new Vector2(width, height);
    }

    public void Reset()
    {
        _slotData.hasItem = false;
    }

    public void HighLight(bool canHighLight)
    {
        if (canHighLight)
            _image.sprite = _highlight;
        else
            _image.sprite = _normal;
    }

    public void AddItem(int itemId)
    {
        _slotData.attachedItemId = itemId;
        _slotData.hasItem = true;
    }

    public void RemoveItem()
    {
        _slotData.attachedItemId = -1;
        _slotData.hasItem = false;
    }
}
