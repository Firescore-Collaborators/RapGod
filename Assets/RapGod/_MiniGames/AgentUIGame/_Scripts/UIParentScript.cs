using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIParentScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    Vector2 startPoint;

    [SerializeField]
    Vector2 endPoint;

    [SerializeField]
    Vector2 drag;

    public int PanelNum;
    public Transform CenterScreen;
    public bool isMoving;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Start");
        startPoint = eventData.pressPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        endPoint = eventData.position;
        drag.x = endPoint.x - startPoint.x;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isMoving = true;
        if (drag.x > 0 /*&& AgentUIManager.instance.AgentUI[0].transform.position != CenterScreen.transform.position*/)
        {
            if (AgentUIManager.instance.AgentUI[0].transform.position.x >= CenterScreen.transform.position.x)
            { return; }
            else if (AgentUIManager.instance.AgentUI[0].transform.position.x <= CenterScreen.transform.position.x)
            {
                GetComponent<RectTransform>().DOAnchorPos(new Vector2(GetComponent<RectTransform>().localPosition.x + 700, 0), 0.5f);
            }
        }
        if (drag.x < 0 /*&& AgentUIManager.instance.AgentUI[AgentUIManager.instance.AgentUI.Length-1].transform.position != CenterScreen.transform.position*/)
        {
            if (AgentUIManager.instance.AgentUI[AgentUIManager.instance.AgentUI.Length - 1].transform.position.x <= CenterScreen.transform.position.x)
            { return; }
            else if (AgentUIManager.instance.AgentUI[AgentUIManager.instance.AgentUI.Length - 1].transform.position.x >= CenterScreen.transform.position.x)
            {
                GetComponent<RectTransform>().DOAnchorPos(new Vector2(GetComponent<RectTransform>().localPosition.x - 700, 0), 0.5f);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (isMoving)
        //{
        //    if (drag.x > 0)
        //    {
        //        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x + 700, transform.position.y), 0.5f);
                
        //    }
        //    if (drag.x < 0)
        //    {
        //        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x - 700, transform.position.y), 0.5f);
                
        //    }

        //    //isMoving = false;
        //}
        //if (drag.x > 0)
        //{
        //    GetComponent<RectTransform>().DOAnchorPosX(700, 0.2f);
        //    //GetComponent<RectTransform>().localPosition = Vector2.Lerp(GetComponent<RectTransform>().localPosition,
        //    //    new Vector2(CenterScreen.transform.localPosition.x + 700, CenterScreen.transform.localPosition.y), 0.1f);

        //    //GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition,
        //    //    new Vector2(GetComponent<RectTransform>().anchoredPosition.x + 700, GetComponent<RectTransform>().anchoredPosition.y), 0.1f);
        //}
        //if (drag.x < 0)
        //{
        //    GetComponent<RectTransform>().;
        //    //GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition,
        //    //    new Vector2(GetComponent<RectTransform>().anchoredPosition.x - 700, GetComponent<RectTransform>().anchoredPosition.y), 0.1f);
        //}
    }

    
}
