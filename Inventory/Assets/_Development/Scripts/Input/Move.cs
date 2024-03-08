using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Move : MonoBehaviour
{
    [SerializeField] private InventoryItem _item;

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

    public float selectedZposition;
    public float selectedYPosition;

    public void Init(InventoryItem item)
    {
        _isNextPositionSet = false;
        _item = item;
        Debug.Log("is selected" + _item.IsSelected);
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
                var hasSetPosition = SetNewPosition();
                Debug.Log($"SetPosition:{hasSetPosition}");

                if (hasSetPosition)
                {
                    _isNextPositionSet = true;
                    _timeElapsed = 0f;
                }
            }
        }
        else if (_item.IsSelected && _isNextPositionSet)
        {
            MoveItem();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rotate();
        }
    }

    public void Rotate()
    {
        _item.Rect.Rotate(0, 0, -90);
    }

    public void SetSlotSize(float width, float height)
    {
        _slotWidth = width;
        _slotHeight = height;      
    }

    private bool SetNewPosition()
    {
        int nextLine = _item.Data.slotPosition[0].line;
        int nextColumn = _item.Data.slotPosition[0].column;

        if (_movement.x > 0)
        {
            nextColumn++;
            if (nextColumn > _item.Data.imageConfig.GetLength(1))
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
            if (nextLine > _item.Data.imageConfig.GetLength(0))
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
