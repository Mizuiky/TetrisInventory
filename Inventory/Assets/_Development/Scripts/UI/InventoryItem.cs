using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public enum MoveDirection
{
    Left,
    Right,
    Up,
    Down
}

public enum Status
{
    Enabled,
    Disabled
}

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class InventoryItem : MonoBehaviour, IInventoryItem
{
    [SerializeField] private InventoryItemData _data;

    private RectTransform _rect;
    private Image _image;

    private TextMeshProUGUI _qtd;
    private Sprite _sprite;
    private Color _green;
    private Color _red;

    private float _horizontal;
    private float _vertical;

    private float _width;
    private float _height;
    private bool _canMove = true;
    private float _slotWidth;
    private float _slotHeight;
    private float _duration = 0.5f;
    private float _speed = 2f;
    private float _timeElapsed = 0f;
    private Vector2 _movement;
    private Vector3 _newPosition;
    private Vector3 _constant;

    public InventoryItemData Data { get { return _data; } set { _data = value; } }
    public Image Image { get { return _image; } }
    public int Qtd { get { return _data.qtd; } set { _data.qtd = value; } }
    public RectTransform Rect { get { return _rect; } set { _rect = value; } }

    public void Start()
    {

    }

    public void Init(InventoryItemData data, Sprite inventoryImage)
    {
        SetProperties();

        _data = new InventoryItemData();
        _data = data;

        _sprite = inventoryImage;
    }

    public void SetSize()
    {
        _rect.localPosition = Vector3.zero;

        _width = _image.rectTransform.sizeDelta.x;
        _height = _image.rectTransform.sizeDelta.y;
    }

    public void SetProperties()
    {
        if (_rect == null)
            _rect = GetComponent<RectTransform>();

        if (_image == null)
            _image = GetComponent<Image>();

        _image.SetNativeSize();
    }

    public void SetPosition(Transform parent, Slot slot, float slotWidth, float slotHeight)
    {
        transform.SetParent(parent);

        Debug.Log(_rect.localPosition);
        Debug.Log(slot.Position);
        Debug.Log($"{slot.Index[0]}, {slot.Index[1]}");

        _rect.localPosition = slot.Position;

        _slotWidth = slotWidth;
        _slotHeight = slotHeight;
        _constant = Vector3.zero;

        _constant = new Vector3((_width / 2 - slotWidth / 2), (-_height / 2 + slotHeight / 2));
        
        var newPosition = new Vector3(slot.Position.x + _constant.x, slot.Position.y + _constant.y, _rect.localPosition.z);

        _rect.localPosition = newPosition;
    }

    public void Rotate()
    {
        _rect.Rotate(0, 0, -90);
    }

    public void Update()
    {
        if(_canMove)
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");

            _movement = new Vector2(_horizontal, _vertical);

            if (_movement != Vector2.zero)
            {
                SetNewPosition();
                _canMove = false;            
                _timeElapsed = 0f;
            }
        }
        else
        {
            Move();
        }      

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rotate();
        }
    }

    public void UpdateQtd()
    {
        _qtd.text = Qtd.ToString();
    }

    public void OnUse()
    {

    }

    public void ChangeColor(Status status)
    {
        if (status == Status.Enabled)
            SetColor(_green);
        else
            SetColor(_red);
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }

    private void SetNewPosition()
    {
        if (_movement.x > 0)
            _newPosition = new Vector3(_rect.localPosition.x + _slotWidth, _rect.localPosition.y, _rect.localPosition.z);
        else if (_movement.x < 0)
            _newPosition = new Vector3(_rect.localPosition.x - _slotWidth, _rect.localPosition.y, _rect.localPosition.z);
        else if (_movement.y > 0)
            _newPosition = new Vector3(_rect.localPosition.x, _rect.localPosition.y + _slotHeight, _rect.localPosition.z);
        else if (_movement.y < 0)
            _newPosition = new Vector3(_rect.localPosition.x, _rect.localPosition.y - _slotHeight, _rect.localPosition.z);
    }

    private void Move()
    {
        if (_timeElapsed < _duration)
        {
            _rect.localPosition = Vector3.Lerp(_rect.localPosition, _newPosition, _timeElapsed / _duration);
            _timeElapsed += Time.deltaTime * _speed;
        }
        else
        {
            _rect.localPosition = _newPosition;
            _canMove = true;
        }
    }
}
