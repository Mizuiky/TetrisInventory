using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class MovementController : MonoBehaviour
{
    private InventoryItem _item;

    public delegate bool EventHandler(InventoryItem item, int line, int column);
    public event EventHandler OnVerifyNextSlotAvailability;

    private Vector2 _movement;
    private Vector3 _newPosition;

    private float _horizontal;
    private float _vertical;

    private float _duration = 0.5f;
    private float _speed = 2f;
    private float _timeElapsed = 0f;

    private bool _isNextPositionSet;

    private float _slotWidth;
    private float _slotHeight;

    private float _width;
    private float _height;

    private int _inventoryColumns;
    private int _inventoryLines;

    private float _selectedZposition;
    private float _selectedYPosition;
    private Vector3 _seletedPosition;

    private Vector3 _constant;

    public void Init(InventoryItem item, float imgWidth, float imgHeight)
    {
        _item = item;
        _isNextPositionSet = false;      
        _selectedZposition = -3;
        _selectedYPosition = 3;
        _width = imgWidth;
        _height = imgHeight;
    }

    public void SetInventorySize(int inventoryMaxColumns, int inventoryMaxLines)
    {
        _inventoryColumns = inventoryMaxColumns;
        _inventoryLines = inventoryMaxLines;
    }

    public void SetSlotSize(float slotWidth, float slotHeight)
    {
        _slotWidth = slotWidth;
        _slotHeight = slotHeight;
    }

    public void Update()
    {
        if (_item.IsSelected && !_isNextPositionSet)
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
            _movement = new Vector2(_horizontal, _vertical);

            if (_movement != Vector2.zero)
            {
                var hasSetPosition = MoveToPosition();
                if (hasSetPosition)
                {
                    _isNextPositionSet = true;
                    _timeElapsed = 0f;
                }
            }
        }
        else if (_item.IsSelected && _isNextPositionSet)
            MoveItem();

        if (Input.GetKeyDown(KeyCode.Space))
            Rotate();
    }

    public void Rotate()
    {
        _item.Rect.Rotate(0, 0, -90);
    }

    public void SetPosition(Transform parent, Slot slot)
    {
        transform.SetParent(parent);
        _item.Rect.localPosition = slot.Position;

        _constant = Vector3.zero;
        _constant = new Vector3((_width / 2 - _slotWidth / 2), (-_height / 2 + _slotHeight / 2));
        var newPosition = new Vector3(slot.Position.x + _constant.x, slot.Position.y + _constant.y, _item.Rect.localPosition.z);

        _item.Rect.localPosition = newPosition;
    }

    public void SetSelectedPosition(bool isSelected)
    {
        if (isSelected)
            _seletedPosition = new Vector3(_item.Rect.localPosition.x, _item.Rect.localPosition.y + _selectedYPosition, _item.Rect.localPosition.z + _selectedZposition);
        else
            _seletedPosition = new Vector3(_item.Rect.localPosition.x, _item.Rect.localPosition.y - _selectedYPosition, _item.Rect.localPosition.z - _selectedZposition);

        _item.Rect.localPosition = _seletedPosition;
    }

    private bool MoveToPosition()
    {
        int nextLine = _item.Data.slotPosition[0].line;
        int nextColumn = _item.Data.slotPosition[0].column;

        if (_movement.x > 0)
        {
            nextColumn++;
            if (nextColumn > _inventoryColumns)
                return false;

            _newPosition = new Vector3(_item.Rect.localPosition.x + _slotWidth, _item.Rect.localPosition.y, _item.Rect.localPosition.z);
        }

        else if (_movement.x < 0)
        {
            nextColumn--;
            if (nextColumn < 0)
                return false;

            _newPosition = new Vector3(_item.Rect.localPosition.x - _slotWidth, _item.Rect.localPosition.y, _item.Rect.localPosition.z);
        }

        else if (_movement.y > 0)
        {
            nextLine--;
            if (nextLine < 0)
                return false;

            _newPosition = new Vector3(_item.Rect.localPosition.x, _item.Rect.localPosition.y + _slotHeight, _item.Rect.localPosition.z);
        }

        else if (_movement.y < 0)
        {
            nextLine++;
            if (nextLine > _inventoryLines)
                return false;

            _newPosition = new Vector3(_item.Rect.localPosition.x, _item.Rect.localPosition.y - _slotHeight, _item.Rect.localPosition.z);
        }

        return CheckNextPosition(nextLine, nextColumn);
    }

    private bool CheckNextPosition(int nextLine, int nextColumn)
    {
        var hasCheckNewPositon = OnVerifyNextSlotAvailability?.Invoke(_item, nextLine, nextColumn);

        if (hasCheckNewPositon.HasValue && hasCheckNewPositon.Value)
        {
            for (int i = 0; i < _item.Data.slotPosition.GetLength(0); i++)
            {
                Debug.Log($"Added item to slots: Line{_item.Data.slotPosition[i].line} Column{_item.Data.slotPosition[i].column}");
            }

            return true;
        }
        else
            return false;
    }

    private void MoveItem()
    {
        if (_timeElapsed < _duration)
        {
            _item.Rect.localPosition = Vector3.Lerp(_item.Rect.localPosition, _newPosition, _timeElapsed / _duration);
            _timeElapsed += Time.deltaTime * _speed;
        }
        else
        {
            _item.Rect.localPosition = _newPosition;
            _isNextPositionSet = false;
        }
    }
}
