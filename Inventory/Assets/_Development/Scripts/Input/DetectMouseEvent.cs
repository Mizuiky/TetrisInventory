using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectMouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Slot slot;
    private InventoryItem inventoryItem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"mouse is over this slot{slot.Data.index[0]},{slot.Data.index[1]}"); 
        
        if(!slot.HasItem)
            slot.HighLight(true);
        else
            slot.HighLight(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {        
        if (slot.HasItem)
        {
            inventoryItem = GameManager.Instance.UIController.GetItem(slot.Data.attachedItemId);
            if(inventoryItem != null) 
            {
                if (!inventoryItem.IsSelected)
                    inventoryItem.Select();                  

                else
                {
                    slot.HighLight(false);
                    inventoryItem.Release();                  
                }                   
            }
        }      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryItem = null;
        slot.HighLight(false);
    }   
}
