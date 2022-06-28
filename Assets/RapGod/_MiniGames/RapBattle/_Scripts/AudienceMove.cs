using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceMove : MonoBehaviour
{
    public Vector3 targetPos,previousPos;
    // Start is called before the first frame update
    void Start()
    {
      //  targetPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos != previousPos)
        {
            LeanTween.move(this.gameObject, targetPos, 1.5f);
            previousPos = targetPos;
        }
    }
}
