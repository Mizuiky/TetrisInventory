using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rect;
    [SerializeField] private SlotData _slotData;

    private Vector2 _position;
    public Vector2 Position { get { return _position; } }
    public int[] Index { get { return _slotData.index; } }
    public bool HasItem { get { return _slotData.hasItem; } set { _slotData.hasItem = value; } }

    private Sprite _highlight;
    private Sprite _normal;

    public Vector3 _topLeft;
    public Vector3 _topRight;
    public Vector3 _bottomLeft;
    public Vector3 _bottomRight;

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

        SetSlotCorners(width, height);
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

    public void AttachItem(int itemId)
    {
        _slotData.attachedItemId = itemId;
    }

    public void DeattachItem()
    {
        _slotData.attachedItemId = -1;
    }

    private void SetSlotCorners(float width, float height)
    {
        _topLeft = new Vector3(_rect.localPosition.x - width / 2, _rect.localPosition.y + height / 2, 0);
        _topRight = new Vector3(_rect.localPosition.x + width / 2, _rect.localPosition.y + height / 2, 0);

        _bottomLeft = new Vector3(_rect.localPosition.x - width / 2, _rect.localPosition.y - height / 2, 0);
        _bottomRight = new Vector3(_rect.localPosition.x + width / 2, _rect.localPosition.y - height / 2, 0);
    }
}
