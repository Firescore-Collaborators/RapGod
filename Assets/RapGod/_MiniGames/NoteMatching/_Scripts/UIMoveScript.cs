using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMoveScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField]private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>(); 
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    
}
