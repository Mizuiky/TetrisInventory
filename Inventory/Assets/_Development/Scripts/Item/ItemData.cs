using UnityEngine;

[System.Serializable]
public class ItemData : MonoBehaviour
{
    public string itemName;
    public ItemType type;
    public int qtd;
    public int slotNumber;
    public string[] sprites;
    public string itemDescription;
}
