using UnityEditor;
using UnityEngine;

public class CustomItemWindow : EditorWindow
{
    private string _itemName;
    private int _id;
    private ItemType _type;
    private Sprite _sprites;
    private Sprite _inventorySprite;
    private string _itemDescription;
    private string _helpBoxMessage = "There are still fields to be filled";

    [MenuItem("Tools/Item Creator Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<CustomItemWindow>();
        window.titleContent = new GUIContent("Item Creator");
        window.maxSize = new Vector2(400, 500);
        window.Show();
    }

    private void OnGUI()
    {
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

        if (CheckAllFields())
        {
            if (GUILayout.Button("Create"))                              
                GameManager.Instance.ItemBuilder.AddItem(_itemName, _id, _type, _sprites, _itemDescription, _inventorySprite);
        }                     
        else
        {
            ShowWarningMessage();
            EditorGUILayout.EndVertical();
            //could not create item, same id change id
            //criar evento no item builder dizendo sew ja tem o id se tiver volta pra ca senao nao volta
        }
    }

    private bool CheckAllFields()
    {
        if (_itemName == "" || _type == ItemType.None || _sprites == null || _itemDescription == "" || _inventorySprite == null)
            return false;

        return true;
    }

    private void ShowWarningMessage()
    {
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label(_helpBoxMessage, EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);
    }
}
