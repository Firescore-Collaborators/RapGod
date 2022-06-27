using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class LerpObjectPosition2 : MonoBehaviour
{
    public static LerpObjectPosition2 instance;
    bool toLerp1;
    float lerpSpeed1;
    float lerpTime1;
    
    Vector3 initPos1;
    Vector3 finalPos1;
    Transform currentObject1;

    System.Action lerpComplete1;

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
