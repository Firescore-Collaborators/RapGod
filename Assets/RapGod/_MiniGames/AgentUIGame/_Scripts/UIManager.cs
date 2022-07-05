using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    Vector2 startPoint;

    [SerializeField]
    Vector2 endPoint;

    [SerializeField]
    Vector2 drag;

    public int seq;
    public GameObject panel1, panel2, panel3, panel4, panel5;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        startPoint = eventData.pressPosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(seq);
        endPoint = eventData.position;
        drag.x = - startPoint.x + endPoint.x;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (drag.x > 0 && seq < 4)
        {
            seq++;
            if (seq == 1)
            {
                panel1.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
            }
            if (seq == 2)
            {
                panel2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
            }
            if (seq == 3)
            {
                panel3.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
            }
            if (seq == 4)
            {
                panel4.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
            }
            if (seq == 5)
            {
                panel5.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
            }

        }
        if (drag.x < 0 && seq > 0)
        {
            seq--;

            if (seq == 0)
            {
                panel1.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            }
            if (seq == 1)
            {
                panel2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            }
            if (seq == 2)
            {
                panel3.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            }
            if (seq == 3)
            {
                panel4.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            }
            if (seq == 4)
            {
                panel5.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Button1()
    {
        seq = 0;
        panel1.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel3.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel4.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel5.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
    }
    public void Button2()
    {
        seq = 1;
        panel1.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel3.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel4.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel5.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
    }
    public void Button3()
    {
        seq = 2;
        panel1.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel3.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel4.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel5.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
    }
    public void Button4()
    {
        seq = 3;
        panel1.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel3.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel4.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
        panel5.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
    }
    public void Button5()
    {
        seq = 4;
        panel1.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel3.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel4.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1200, 0), 0.5f);
        panel5.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
    }
}
