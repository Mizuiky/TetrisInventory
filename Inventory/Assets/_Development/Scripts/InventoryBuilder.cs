using UnityEngine;

public class InventoryBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private Sprite _slotSprite;

    [SerializeField] private float _startX;
    [SerializeField] private float _startY;

    [SerializeField] private int _line;
    [SerializeField] private int _column;

    private GameObject _square;

    private Vector2 _position;
    private Vector2 _index;

    private float _slotWidth;
    private float _x;
    private float _y;

    public void Start()
    {
        Init();
    }

    private void Init()
    {
        _x = _startX;
        _y = _startY;

        _slotWidth = GetSlotWidth();

        Build();
    }

    private void Build()
    {
        for(int i = 0; i < _line; i++)
        {
            for(int j = 0; j < _column; j++)
            {
                if (j == 0 && i != 0)
                {
                    _x = _startX * i;
                    _y += _slotWidth;
                }

                BuildSlot(_x, _y, i, j);

                _x += _slotWidth;
            }
        }
    }

    private void BuildSlot(float x, float y, int i, int j)
    {
        _position = new Vector2(x, y);
        _index = new Vector2(i, j);

        _square = Instantiate(_slotPrefab, _parent);
        _square.transform.localPosition = _position;

        if(_square != null)
        {
            var slot = _square.GetComponent<Slot>();
            if (slot != null)
                slot.Init(_slotSprite, _position, _index);
        }
    }

    private float GetSlotWidth()
    {
        var width = _slotPrefab.GetComponent<RectTransform>().rect.width;
        return width;
    }
}
