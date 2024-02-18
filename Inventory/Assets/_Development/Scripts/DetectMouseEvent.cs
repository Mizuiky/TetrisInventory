using UnityEngine;
using UnityEngine.EventSystems;

public class DetectMouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Slot slot;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"mouse is over this slot{slot.Index}");

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("mouse exit");
    }
}
