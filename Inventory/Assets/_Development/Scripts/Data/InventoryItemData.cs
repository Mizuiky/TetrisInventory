[System.Serializable]
public class InventoryItemData
{
    public string imageName;
    public string name;
    public int id;
    public int qtd;
    public string description;
    public SlotPosition[] slotPosition;
    public int[,] imageConfig;
}
