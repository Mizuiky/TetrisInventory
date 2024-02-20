using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IInventoryItem
{
    [SerializeField] private InventoryItemData _data;
    private RectTransform _rec;
    private Image _image;

    private TextMeshProUGUI _qtd;

    public InventoryItemData Data { get { return _data; } }
    public int Qtd { get { return _data.qtd; } set { _data.qtd = value; } }

    public void Init(InventoryItemData  data, Sprite inventoryImage)
    {
        _data = new InventoryItemData();
        _data = data;
        _data.name = data.name;

        if (_image == null)
            _image = GetComponent<Image>();

        _image.sprite = inventoryImage;
    }

    public void Rotate()
    {
        _rec.Rotate(0, 0, -90);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Rotate();
        }
    }

    public void UpdateQtd(int value)
    {
        _qtd.text = value.ToString();
    }

    public void SetInventoryIndex(int line, int column)
    {
        _data.inventoryIndex[0] = line;
        _data.inventoryIndex[1] = column;
    }
}