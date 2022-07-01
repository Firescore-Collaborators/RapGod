using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMoveScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField]private CanvasGroup canvasGroup;
    public Vector3 StartPosition;
    public int boxCode;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>(); 
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        StartPosition = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if(rectTransform.position.x <= Notes_Manager.instance.endpoint1.position.x || rectTransform.position.y <= Notes_Manager.instance.endpoint1.position.y
            || rectTransform.position.x >= Notes_Manager.instance.endpoint2.position.x || rectTransform.position.y >= Notes_Manager.instance.endpoint2.position.y)
        {
            rectTransform.position = StartPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    
}
