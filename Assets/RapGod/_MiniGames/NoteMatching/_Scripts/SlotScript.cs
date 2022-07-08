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
            if(Notes_Manager.instance.sum == Notes_Manager.instance.box.Length)
            {
                Notes_Manager.instance.showPanel();
            }

            if(SlotCode == eventData.pointerDrag.transform.GetComponent<UIMoveScript>().boxCode)
            {
                Debug.Log("shine");
                eventData.pointerDrag.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }

            if (eventData.pointerDrag.transform.GetComponent<UIMoveScript>().boxCode == 3)
            {
                Notes_Manager.instance.SRC.clip = Notes_Manager.instance.clip1;
                Notes_Manager.instance.SRC.Play();
            }
            if (eventData.pointerDrag.transform.GetComponent<UIMoveScript>().boxCode == 2)
            {
                Notes_Manager.instance.SRC.clip = Notes_Manager.instance.clip2;
                Notes_Manager.instance.SRC.Play();
            }
            if (eventData.pointerDrag.transform.GetComponent<UIMoveScript>().boxCode == 1)
            {
                Notes_Manager.instance.SRC.clip = Notes_Manager.instance.clip3;
                Notes_Manager.instance.SRC.Play();
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
