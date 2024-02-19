using System.IO.Ports;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomItemWindow : EditorWindow
{
    private string _itemName;
    private int _id;
    private ItemType _type;
    public Sprite _sprites;
    private string _itemDescription;
    private string _helpBoxMessage = "There are still fields to be filled";
    private bool _showWarning = false;

    [MenuItem("Tools/Item Creator Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<CustomItemWindow>();
        window.titleContent = new GUIContent("Item Creator");
        window.minSize = new Vector2(400, 300);
        window.maxSize = new Vector2(600, 500);
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

        if (CheckAllFields())
        {
            if (GUILayout.Button("Create"))                              
                GameManager.Instance.ItemBuilder.AddItem(_itemName, _id, _type, _sprites, _itemDescription);
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
        if (_itemName == "" || _type == ItemType.None || _sprites == null || _itemDescription == "")
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
