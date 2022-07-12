using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class AgentUIPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    Vector2 startPoint;

    [SerializeField]
    Vector2 endPoint;

    [SerializeField]
    Vector2 drag;

    public int PanelNum;

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
        Debug.Log("Drag Ended");
        if (drag.x > 0 && PanelNum>=1 /*&& PanelNum>=1*/)
        {
            Debug.Log("Right");
            //AgentUIManager.instance.AgentUI[PanelNum-1].GetComponent<RectTransform>().localPosition = AgentUIManager.instance.CenterScreen.localPosition;
            //AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().localPosition = new Vector2(AgentUIManager.instance.CenterScreen.localPosition.x + 700, AgentUIManager.instance.CenterScreen.localPosition.x);
            //AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().localPosition = new Vector2(AgentUIManager.instance.CenterScreen.localPosition.x + 1400, AgentUIManager.instance.CenterScreen.localPosition.x);
            //if(PanelNum == 1)
            //{
            //    AgentUIManager.instance.AgentUI[PanelNum+1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(1400, 0), 0.5f);
            //    AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().DOAnchorPos(new Vector2(700, 0), 0.5f);
            //    AgentUIManager.instance.AgentUI[PanelNum - 1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            //}
            //if(PanelNum == 2)
            //{
            //    AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().DOAnchorPos(new Vector2(700, 0), 0.5f);
            //    AgentUIManager.instance.AgentUI[PanelNum - 1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            //    AgentUIManager.instance.AgentUI[PanelNum - 2].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-700, 0), 0.5f);
            //}

            //AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().DOAnchorPos(new Vector2(700, 0), 0.5f);
            //AgentUIManager.instance.AgentUI[PanelNum-1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);

        }
        if (drag.x < 0 && PanelNum<=1 /*&& PanelNum<=1*/)
        {
            
            //if (PanelNum == 1)
            //{
            //    AgentUIManager.instance.AgentUI[PanelNum - 1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-1400, 0), 0.5f);
            //    AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-700, 0), 0.5f);
            //    AgentUIManager.instance.AgentUI[PanelNum + 1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            //}
            //if(PanelNum == 0)
            //{
            //    AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-700, 0), 0.5f);
            //    AgentUIManager.instance.AgentUI[PanelNum + 1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            //    AgentUIManager.instance.AgentUI[PanelNum + 2].GetComponent<RectTransform>().DOAnchorPos(new Vector2(700, 0), 0.5f);
            //}

            //AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().DOAnchorPos(new Vector2(-700, 0), 0.5f); 
            //AgentUIManager.instance.AgentUI[PanelNum + 1].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Drag Ended");
        if (drag.x > 0 && PanelNum >= 1 /*&& PanelNum>=1*/)
        {
            Debug.Log("Right");
            AgentUIManager.instance.AgentUI[PanelNum - 1].GetComponent<RectTransform>().localPosition = 
                Vector2.Lerp(AgentUIManager.instance.AgentUI[PanelNum - 1].GetComponent<RectTransform>().localPosition, AgentUIManager.instance.CenterScreen.localPosition, 0.2f);
            //AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().localPosition = new Vector2(AgentUIManager.instance.CenterScreen.localPosition.x + 700, AgentUIManager.instance.CenterScreen.localPosition.x);
            //AgentUIManager.instance.AgentUI[PanelNum].GetComponent<RectTransform>().localPosition = new Vector2(AgentUIManager.instance.CenterScreen.localPosition.x + 1400, AgentUIManager.instance.CenterScreen.localPosition.x);
          
        }
        if (drag.x < 0 && PanelNum <= 1 /*&& PanelNum<=1*/)
        {

          
        }
    }
}
