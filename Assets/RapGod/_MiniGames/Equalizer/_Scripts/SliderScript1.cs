using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SliderScript1 : MonoBehaviour
{
    public Vector3 screenPoint;
    public Vector3 offset;

    [SerializeField]
    public GameObject slider;
    public bool onObject, isMatched;
    public int Reading, slidercount;

    public AudioSource asrc;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        onObject = true;

        //if (currentIndex == Reading)
        //{
        //    //Debug.Log("playing");
        //    SoundManager.instance.Play("match");
        //}
    }
    private void OnMouseUp()
    {
        onObject = false;
        slidercount = currentIndex;

        if (currentIndex == Reading)
        {
            //Debug.Log("playing");
            SoundManager.instance.Play("match");
        }
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = Vector3.zero;
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        curPosition.y = transform.position.y;
        curPosition.x = transform.position.x;

        transform.position = curPosition;

        CheckPos();
    }
    // Start is called before the first frame update
    void Start()
    {
        asrc = GetComponent<AudioSource>();
    }

    public int currentIndex;

    // Update is called once per frame
    void Update()
    {
        if(currentIndex == Reading)
        {
            //SoundManager.instance.Play("match");
        }

        if (EqualizerManager.instance.GameOver)
            return;

        if (!onObject)
            return;

            if(transform.position.z <= EqualizerManager.instance.startLimit.position.z)
            {
                transform.position = new Vector3( transform.position.x, transform.position.y ,EqualizerManager.instance.startLimit.position.z);
            }
            if (transform.position.z >= EqualizerManager.instance.endLimit.position.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, EqualizerManager.instance.endLimit.position.z);
            }
    }

    public void CheckPos()
    {
        for (int i = 0; i < EqualizerManager.instance.Limit.Length; i++)
        {
            {
                if (transform.position.z >= EqualizerManager.instance.Limit[i].transform.position.z)
                {
                    currentIndex = i;

                }
               
            }
        }
        
    }
}
