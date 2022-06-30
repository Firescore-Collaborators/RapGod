using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilMoveScript : MonoBehaviour
{
    public Vector3 screenPoint;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        //transform.Rotate(0,1,0);

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = Vector3.zero;
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        //curPosition.z = curPosition.y;

        //curPosition.y = 0.75f;
        curPosition.y = transform.position.y;

        transform.position = curPosition;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("sign_cp"))
        {
            other.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}