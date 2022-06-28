using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectTouchFollow : MonoBehaviour
{
    [SerializeField]float offset;

    [SerializeField] bool UI; 
    public bool follow;
    //public float distance;

    private void Start()
    {
        CalculateOffset();
        
    }

    private void Update()
    {
        if (follow)
        {
            if (UI)
            {
                transform.position = Input.mousePosition;
            }
            else
            {
                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, offset));
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            offset = 2.65f;
        }
    }

    void CalculateOffset()
    {
        //offset = Camera.main.transform.position.z - transform.position.z;
        //offset = 4.6f;//Vector3.Distance(Camera.main.transform.position, transform.position) ;
    }

}
