using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler
{
    public int SlotCode;
    public int[,] GridArray;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Ondrop");
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                = GetComponent<RectTransform>().anchoredPosition;
        }
        else if(eventData.pointerDrag == null)
        {
            Debug.Log("null");
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                eventData.pointerDrag.GetComponent<UIMoveScript>().StartPosition;
        }
    }
}
