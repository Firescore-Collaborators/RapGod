using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpObject : MonoBehaviour
{
    bool lerpStarted;
    float lerpSpeed;
    float lerpTime;

    Vector3 initPos;
    Vector3 finalPos;

    [SerializeField]
    Transform currentObject;

    System.Action lerpComplete1;

    void Update()
    {
        if (lerpStarted == false)
            return;

        currentObject.transform.position = Vector3.Lerp(initPos, finalPos, lerpTime);

        if (lerpTime < 1.0f)
        {
            lerpTime += Time.deltaTime / lerpSpeed;
        }
        else
        {
            lerpStarted = false;
            lerpTime = 0;
            if (lerpComplete1 != null)
            {
                lerpComplete1.Invoke();
            }
        }
    }
    public void Lerp(Transform lerpObject, Vector3 _finalPos, float speed, System.Action _lerpComplete = null)
    {
        currentObject = lerpObject;
        initPos = currentObject.transform.position;
        finalPos = _finalPos;
        lerpSpeed = speed;
        lerpTime = 0;
        if (_lerpComplete != null)
            lerpComplete1 = _lerpComplete;
        else
            lerpComplete1 = null;

        lerpStarted = true;

    }
}
