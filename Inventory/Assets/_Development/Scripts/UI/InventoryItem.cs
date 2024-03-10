using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class InventoryItem : MonoBehaviour, IInventoryItem
{
    [SerializeField] private InventoryItemData _data;
    private MovementController _move;

    private RectTransform _rect;
    private Image _image;

    private TextMeshProUGUI _qtd;
    private Sprite _sprite;

    private Color _green;
    private Color _red;
    private Color _originalItemColor;
    private float _width;
    private float _height;

    private bool _isSelected;

    public InventoryItemData Data { get { return _data; } set { _data = value; } }
    public Image Image { get { return _image; } }
    public int Qtd { get { return _data.qtd; } set { _data.qtd = value; } }
    public RectTransform Rect { get { return _rect; } set { _rect = value; } }
    public bool IsSelected { get { return _isSelected; } }
    public MovementController Move { get { return _move; } }

    public void Init(InventoryItemData data, Sprite inventoryImage)
    {
        _data = new InventoryItemData();
        _data = data;
        _sprite = inventoryImage;

        InitializeComponent();       
    }

    public void InitializeComponent()
    {
        _isSelected = false;
        SetProperties();     
    }

    public void SetProperties()
    {
        if (_rect == null)
            _rect = GetComponent<RectTransform>();

        if (_image == null)
            _image = GetComponent<Image>();

        _image.SetNativeSize();
        SetSize();

        _originalItemColor = _image.color;

        if (_move == null)
        {
            _move = GetComponent<MovementController>();
            _move.Init(this, _width, _height);
        }         

        _rect.localPosition = Vector3.zero;
    }

    public void SetSize()
    {
        _width = _image.rectTransform.sizeDelta.x;
        _height = _image.rectTransform.sizeDelta.y;
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
        _move.SetSelectedPosition(_isSelected);
        SetColor(Color.yellow);
    }

    public void Unselect()
    {
        _isSelected = false;
        _move.SetSelectedPosition(_isSelected);
        SetColor(_originalItemColor);
    }

    public void OnUse()
    {

    }
}
