using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public ItemType type;
    public int qtd;
    public int slotNumber;
    public Sprite[] sprites;
    public string itemDescription;

    public void SetSprites(Sprite[] spriteList)
    {
        sprites = new Sprite[spriteList.Length];
        sprites = spriteList;
    }
}
