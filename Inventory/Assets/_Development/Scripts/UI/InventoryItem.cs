using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class InventoryItem : MonoBehaviour, IInventoryItem
{
    [SerializeField] private InventoryItemData _data;
    private Move _move;

    private RectTransform _rect;
    private Image _image;

    private TextMeshProUGUI _qtd;
    private Sprite _sprite;

    private Color _green;
    private Color _red;
    private Color _originalItemColor;

    private Vector3 _constant;

    private float _width;
    private float _height;
    private float _slotWidth;
    private float _slotHeight;
    private bool _isSelected;

    public InventoryItemData Data { get { return _data; } set { _data = value; } }
    public Image Image { get { return _image; } }
    public int Qtd { get { return _data.qtd; } set { _data.qtd = value; } }
    public RectTransform Rect { get { return _rect; } set { _rect = value; } }
    public bool IsSelected { get { return _isSelected; } }
    public Move Move { get { return _move; } }

    public void Init(InventoryItemData data, Sprite inventoryImage)
    {
        _data = new InventoryItemData();
        _data = data;

        InitializeComponent();

        _sprite = inventoryImage;      
    }

    public void InitializeComponent()
    {
        _isSelected = false;

        if (_move == null)
        {
            _move = GetComponent<Move>();
            
        }

        _move.Init(this);
        SetProperties();
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
        _originalItemColor = _image.color;

        if (_move == null)
        {
            _move = GetComponent<Move>();
        }

        _move.selectedZposition = -3;
        _move.selectedYPosition = 3;
    }

    public void SetPosition(Transform parent, Slot slot, float slotWidth, float slotHeight)
    {
        transform.SetParent(parent);
        _rect.localPosition = slot.Position;

        _slotWidth = slotWidth;
        _slotHeight = slotHeight;
        
        Move.SetSlotSize(_slotWidth, _slotHeight);

        _constant = Vector3.zero;
        _constant = new Vector3((_width / 2 - slotWidth / 2), (-_height / 2 + slotHeight / 2));
        
        var newPosition = new Vector3(slot.Position.x + _constant.x, slot.Position.y + _constant.y, _rect.localPosition.z);

        _rect.localPosition = newPosition;
    }

    public void UpdateQtd()
    {
        _qtd.text = Qtd.ToString();
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }

    public void Select()
    {
        _isSelected = true;
        _rect.localPosition = new Vector3(_rect.localPosition.x, _rect.localPosition.y + _move.selectedYPosition, _rect.localPosition.z + _move.selectedZposition);
        SetColor(Color.yellow);
    }

    public void Unselect()
    {
        _isSelected = false;
        _rect.localPosition = new Vector3(_rect.localPosition.x, _rect.localPosition.y - _move.selectedYPosition, _rect.localPosition.z - _move.selectedZposition);
        SetColor(_originalItemColor);
    }

    public void OnUse()
    {

    }
}
