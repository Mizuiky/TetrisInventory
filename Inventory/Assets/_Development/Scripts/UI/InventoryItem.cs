using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ColorType
{
    SelectedItem,
    ReleasedItem,
    CantPlaceItem
}

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class InventoryItem : MonoBehaviour, IInventoryItem
{
    [SerializeField] private InventoryItemData _data;
    private MovementController _move;

    private RectTransform _rect;
    private Image _image;

    private TextMeshProUGUI _qtd;
    private Sprite _sprite;
    private ColorTint _colorTint;

    private float _width;
    private float _height;

    private bool _isSelected;
    private bool _canPlaceItem;
    private bool _canBeSelected;

    public InventoryItemData Data { get { return _data; } set { _data = value; } }
    public RectTransform Rect { get { return _rect; } set { _rect = value; } }
    public MovementController Move { get { return _move; } }
    public ColorTint ColorTint { get { return _colorTint; } }
    public Image Image { get { return _image; } }
    public int Qtd { get { return _data.qtd; } set { _data.qtd = value; } }    
    public bool IsSelected { get { return _isSelected; } }   
    public bool CanBeSelected { get { return _canBeSelected; } set { _canBeSelected = value; } }

    public void Init(InventoryItemData data, Sprite inventoryImage)
    {
        _data = new InventoryItemData();
        _data = data;
        _sprite = inventoryImage;
        _canBeSelected = true;

        InitializeComponent();       
    }

    public void InitializeComponent()
    {
        _isSelected = false;
        _canPlaceItem = false;
        SetComponents();     
    }

    public void SetComponents()
    {
        if (_rect == null)
        {
            _rect = GetComponent<RectTransform>();
            _rect.localPosition = Vector3.zero;
        }
            
        if (_image == null)
            _image = GetComponent<Image>();

        _image.SetNativeSize();
        SetSize();

        if (_move == null)
        {
            _move = GetComponent<MovementController>();
            _move.Init(this, _width, _height);
        }         
        
        if(_colorTint == null)
        {
            _colorTint = GetComponent<ColorTint>();
            _colorTint.Init(_image);
        }
    }

    public void SetSize()
    {
        _width = _image.rectTransform.sizeDelta.x;
        _height = _image.rectTransform.sizeDelta.y;
    }

    public void SetCanPlaceItem(bool canPlaceItem)
    {
        _canPlaceItem = canPlaceItem;
        if (!_canPlaceItem)
            _colorTint.SetColorByType(ColorType.CantPlaceItem);
        else
            _colorTint.SetColorByType(ColorType.SelectedItem);
    }

    public void UpdateQtd()
    {
        _qtd.text = Qtd.ToString();
    }

    public void Select()
    {
        _isSelected = true;
        _canPlaceItem = true;
        _move.SetSelectedPosition(_isSelected);
        _colorTint.SetColorByType(ColorType.SelectedItem);      
    }

    public void Release()
    {
        if(_canPlaceItem)
        {
            _isSelected = false;
            _canPlaceItem = false;
            _move.SetSelectedPosition(_isSelected);
            _colorTint.SetColorByType(ColorType.ReleasedItem);
        }
    }

    public void OnUse()
    {

    }
}
