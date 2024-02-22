using UnityEditor;
using UnityEngine;

public class CustomItemWindow : EditorWindow
{
    private int _id;
    private int _lines;
    private int _columns;
    private int[,] _slotConfig = new int[0, 0];
    private string _itemName;
    private string _itemDescription;
    private string _helpBoxMessage = "There are still fields to be filled";

    private ItemType _type;
    private Sprite _sprites;
    private Sprite _inventorySprite;
    private Vector2 _scrollPosition = Vector3.zero;

    [MenuItem("Tools/Item Creator Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<CustomItemWindow>();
        window.titleContent = new GUIContent("Item Creator");
        window.maxSize = new Vector2(450, 600);
        window.Show();
    }

    private void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        GUILayout.Label("Item", EditorStyles.boldLabel);

        _itemName = EditorGUILayout.TextField("Name:", _itemName);
        EditorGUILayout.Space();

        _id = EditorGUILayout.IntField("ID:", _id);
        EditorGUILayout.Space();

        _type = (ItemType)EditorGUILayout.EnumPopup("Select one type:", _type);
        EditorGUILayout.Space();

        _sprites = (Sprite)EditorGUILayout.ObjectField("Sprite:", _sprites, typeof(Sprite), false);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Description");
        _itemDescription = EditorGUILayout.TextArea(_itemDescription, GUILayout.Height(60));
        EditorGUILayout.Space(10);

        _inventorySprite = (Sprite)EditorGUILayout.ObjectField("Inventory image:", _inventorySprite, typeof(Sprite), false);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Image Config");
        _lines = EditorGUILayout.IntField("Lines", _lines);
        _columns = EditorGUILayout.IntField("Colums", _columns);
        EditorGUILayout.Space();

        if (_lines != _slotConfig.GetLength(0) || _columns != _slotConfig.GetLength(1))
            _slotConfig = new int[_lines, _columns];
              
        for(int i = 0; i < _lines; i++)
        {
            for(int j = 0; j < _columns; j++)
            {
                _slotConfig[i,j] = EditorGUILayout.IntField($"Elemento: ({i},{j})", _slotConfig[i, j]);
            }         
        }

        if (CheckAllFields())
        {
            if (GUILayout.Button("Create"))
            {
                GameManager.Instance.ItemBuilder.AddItem(_itemName, _id, _type, _sprites, _itemDescription, _inventorySprite, _slotConfig, GetFilledSlotsNumber());
            }
               
        }                     
        else
        {
            ShowWarningMessage();
            EditorGUILayout.EndVertical();
            //could not create item, same id change id
            //criar evento no item builder dizendo sew ja tem o id se tiver volta pra ca senao nao volta
        }

        EditorGUILayout.EndScrollView();
    }

    private bool CheckAllFields()
    {
        if (_itemName == "" || _type == ItemType.None || _sprites == null || _itemDescription == "" || _inventorySprite == null || _slotConfig.Length == 0)
            return false;

        return true;
    }

    private int GetFilledSlotsNumber()
    {
        var count = 0;

        for (int i = 0; i < _lines; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (_slotConfig[i,j] == 1)
                    count++;
            }
        }

        return count;
    }

    private void ShowWarningMessage()
    {
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label(_helpBoxMessage, EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);
    }
}
