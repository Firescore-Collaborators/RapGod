using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler
{
    public int SlotCode;
 
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Ondrop");
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                = GetComponent<RectTransform>().anchoredPosition;

            Notes_Manager.instance.CubeCheck();
            if (Notes_Manager.instance.sum < Notes_Manager.instance.box.Length)
            {
                Notes_Manager.instance.sum = 1;
            }
        }
        else if(eventData.pointerDrag == null)
        {
            Debug.Log("null");
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                eventData.pointerDrag.GetComponent<UIMoveScript>().StartPosition;
        }
    }
}
