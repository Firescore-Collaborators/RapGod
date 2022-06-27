using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAndRun_RotateToward : MonoBehaviour
{
    [SerializeField]
    private GameObject followTarget;
    [SerializeField]
    private float movementSpeed, rotationSpeed;
    Quaternion targetRotation = Quaternion.identity;

    private void FixedUpdate()
    {
       
    }

    void LateUpdate()
    {
        Vector3 tempPos = gameObject.transform.localPosition;
        tempPos.x = followTarget.transform.localPosition.x;
       // tempPos.x  = Mathf.Clamp(tempPos.x, -2.8f, 3.9f);
       

        //if (Input.GetMouseButton(0))
        //{
        //    targetRotation = Quaternion.LookRotation(new Vector3(followTarget.transform.localPosition.x, 0, 0) - new Vector3(transform.localPosition.x, 0, 0));
           
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    targetRotation = Quaternion.identity;
        //}

        gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, tempPos, movementSpeed * Time.deltaTime);
      //  transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
    }
}
