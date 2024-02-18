using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("qtd"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("slotNumber"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sprites"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));

        serializedObject.ApplyModifiedProperties();

    }

}
