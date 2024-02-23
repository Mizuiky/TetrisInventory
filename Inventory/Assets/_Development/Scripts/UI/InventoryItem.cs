using System;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.UI;

public enum MoveDirection
{
    Left,
    Right,
    Up,
    Down
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

    public void SetPosition(Transform parent, Slot slot)
    {
        transform.SetParent(parent);
        _rect.localPosition = Vector3.zero;

        _rect.localPosition = slot.GetComponent<RectTransform>().localPosition;
    }

    public void Rotate()
    {
        _rect.Rotate(0, 0, -90);
    }

    public void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rotate();
        }
    }

    public void UpdateQtd()
    {
        _qtd.text = Qtd.ToString();
    }

    public void SetInventoryIndex()
    {

    }

    public void OnUse()
    {

    }

    private void Move()
    {

    }
}