using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTint : MonoBehaviour
{
    private Dictionary<int, Color> _itemColors;
    private Image _image;

    private Color _originalColor;
    private Color _selectedColor;
    private Color _cantPlaceColor;

    public void Init(Image image)
    {
        _image = image;
        _selectedColor = ColorToHex("#3EDB83");
        _cantPlaceColor = ColorToHex("#DB3D3D");
    }

    private Color ColorToHex(string colorHexadecimal)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString(colorHexadecimal, out color))
        {
            return color;
        }

        return _originalColor;
    }

    public void SetColorByType(ColorType type)
    {
        switch(type)
        {
            case ColorType.SelectedItem: _image.color = _selectedColor;
                break;
            case ColorType.ReleasedItem: _image.color = _originalColor;
                    break;
            case ColorType.CantPlaceItem: _image.color = _cantPlaceColor;
                break;
        }       
    }

    public void SetColor(Color color) 
    {
        _originalColor = color;
        _image.color= color;
    }
}
