using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Vector3 screenPoint;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

        transform.position = Camera.main.ScreenToWorldPoint
            (new Vector3(transform.position.x, Input.mousePosition.y, -Input.mousePosition.x));
    
    }
}
