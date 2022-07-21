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
    public bool onObject, isMatched;
    public int Reading, slidercount;
    public Transform startPos;
    public AudioSource asrc;
    public GameObject Indicator1, Indicator2;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        //transform.Rotate(0,1,0);
        onObject = true;
        //EqualizerManager.instance.PlayMusic();
        //SoundManager.instance.Play("match");

        //SoundManager.instance.Play("scroll");
    }
    private void OnMouseUp()
    {
        onObject = false;
        slidercount = currentIndex;

        EqualizerManager.instance.CheckStatus();

        if (currentIndex == Reading)
        {
            //Debug.Log("playing");
            SoundManager.instance.Play("match");
        }
        else
        {
            SoundManager.instance.Play("scroll");
        }
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

        CheckEqualiser();

        //if (isMatched)
        //{
        //    //SoundManager.instance.PlayOnce("match");
        //    SoundManager.instance.Play("match");
        //}
        //else
        //{
        //    SoundManager.instance.Mute("match");
        //}

    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        startPos = transform;
        asrc = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        for (int i = 0; i < slider.transform.childCount; i++)
        {
            slider.transform.GetChild(i).GetComponent<Graphic>().color = Color.clear;
        }
    }

    public int currentIndex;

    // Update is called once per frame
    void Update()
    {
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
        

        public void CheckEqualiser()
        {
        if (EqualizerManager.instance.GameOver)
            return;

        if (!onObject)
            return;

        for (int i = 0; i < EqualizerManager.instance.Limit.Length; i++)
        {
            if (transform.position.z > EqualizerManager.instance.Limit[i].transform.position.z)
            {
                slider.transform.GetChild(i).GetComponent<Graphic>().color = Color.red;
                currentIndex = i;

                transform.Translate(0, 0, 1);

                //SoundManager.instance.Play("scroll");
                slidercount++;

                //asrc.clip = EqualizerManager.instance.incrementPop;
                //asrc.Play();

                if (currentIndex == Reading)
                {
                    Indicator1.GetComponent<MeshRenderer>().material = EqualizerManager.instance.GreenMat;
                    Indicator2.GetComponent<MeshRenderer>().material = EqualizerManager.instance.GreenMat;

                    for (int j = 0; j <= i; j++)
                    {
                        slider.transform.GetChild(j).GetComponent<Animator>().SetBool("Blip", true);
                        slider.transform.GetChild(j).GetComponent<Graphic>().color = Color.green;
                        //slider.transform.GetChild(j).GetComponent<Animator>().enabled = true;    
                    }
                    Debug.Log("matched");

                    //asrc.clip = EqualizerManager.instance.matchTing;
                    //asrc.Play();
                    isMatched = true;
                }
                //else 
                if (currentIndex != Reading)
                {
                    //SoundManager.instance.Play("scroll");
                    Indicator1.GetComponent<MeshRenderer>().material = EqualizerManager.instance.RedMat;
                    Indicator2.GetComponent<MeshRenderer>().material = EqualizerManager.instance.RedMat;

                    for (int j = 0; j <= i; j++)
                    {
                        slider.transform.GetChild(j).GetComponent<Animator>().SetBool("Blip", false);
                        slider.transform.GetChild(j).GetComponent<Graphic>().color = Color.red;

                        //asrc.clip = EqualizerManager.instance.incrementPop;
                        //asrc.PlayOneShot(asrc.clip);//slider.transform.GetChild(j).GetComponent<Animator>().enabled = false;
                    }
                    isMatched = false;
                    Debug.Log("not matched");

                    
                }
            }

            else if (transform.position.z < EqualizerManager.instance.Limit[i].transform.position.z)
            {
                slider.transform.GetChild(i).GetComponent<Graphic>().color = Color.clear;
            }

        }
    }
}
