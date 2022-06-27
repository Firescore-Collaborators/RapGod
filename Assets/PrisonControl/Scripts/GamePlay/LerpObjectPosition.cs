using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class LerpObjectPosition : MonoBehaviour
{
    public static LerpObjectPosition instance;
    bool toLerp1, toLerp2;
    float lerpSpeed1, lerpSpeed2;
    float lerpTime1, lerpTime2;
    
    Vector3 initPos1, initPos2;
    Vector3 finalPos1, finalPos2;
    Transform currentObject1, currentObject2;

    System.Action lerpComplete1, lerpComplete2;

    int lerpIndex;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Update()
    {
        if (lerpIndex == 0)
        {
            if (toLerp1 == false)
                return;

            currentObject1.transform.position = Vector3.Lerp(initPos1, finalPos1, lerpTime1);

            if (lerpTime1 < 1.0f)
            {
                lerpTime1 += Time.deltaTime / lerpSpeed1;
            }
            else
            {
                toLerp1 = false;
                lerpTime1 = 0;
                if (lerpComplete1 != null)
                {
                    lerpComplete1.Invoke();
                }
            }
        }
        //else
        //if (lerpIndex == 2)
        //{
        //    if (toLerp2 == false)
        //        return;

        //    currentObject2.transform.position = Vector3.Lerp(initPos2, finalPos2, lerpTime2);

        //    if (lerpTime2 < 1.0f)
        //    {
        //        lerpTime2 += Time.deltaTime / lerpSpeed2;
        //    }
        //    else
        //    {
        //        toLerp2 = false;
        //        lerpTime2 = 0;
        //        if (lerpComplete2 != null)
        //        {
        //            lerpComplete2.Invoke();
        //        }
        //    }
        //}

    }
    public void LerpObject(Transform lerpObject, Vector3 _finalPos, float speed, System.Action _lerpComplete = null)
    {
        currentObject1 = lerpObject;
        initPos1 = currentObject1.transform.position;
        finalPos1 = _finalPos;
        lerpSpeed1 = speed;
        lerpTime1 = 0;
        if(_lerpComplete != null)
            lerpComplete1 = _lerpComplete;
        else
            lerpComplete1 = null;

        toLerp1 = true;
    }
}
