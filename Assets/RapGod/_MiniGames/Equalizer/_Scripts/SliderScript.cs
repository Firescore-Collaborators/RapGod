using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SliderScript : MonoBehaviour
{
    public Vector3 screenPoint;
    public Vector3 offset;

    [SerializeField]
    public GameObject slider;
    public bool onObject;
    public int Reading;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        //transform.Rotate(0,1,0);
        onObject = true;

    }
    private void OnMouseUp()
    {
        onObject = false;
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = Vector3.zero;
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        //curPosition.z = curPosition.y;

        //curPosition.y = 0.75f;
        curPosition.y = transform.position.y;
        curPosition.x = transform.position.x;

        transform.position = curPosition;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int currentIndex;

    // Update is called once per frame
    void Update()
    {
        if (!onObject)
            return;

        for (int i = 0; i < EqualizerManager.instance.Limit.Length; i++)
        {
            if (transform.position.z > EqualizerManager.instance.Limit[i].transform.position.z)
            {
                slider.transform.GetChild(i).GetComponent<Graphic>().color = Color.red;
                currentIndex = i;

                if(currentIndex == Reading)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        slider.transform.GetChild(j).GetComponent<Graphic>().color = Color.green;
                    }
                }
                else
                {
                    for (int j = 0; j <= i; j++)
                        slider.transform.GetChild(j).GetComponent<Graphic>().color = Color.red;
                }
            }

            else if(transform.position.z < EqualizerManager.instance.Limit[i].transform.position.z)
            {
                slider.transform.GetChild(i).GetComponent<Graphic>().color = Color.clear;
           //     currentIndex = i;
            }
            
        }

        
        
    }
}
