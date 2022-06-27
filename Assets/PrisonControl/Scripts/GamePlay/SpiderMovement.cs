using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    public bool down, left, right,forward;
    public float Speed;
    bool dead;
    public float lifetime;
    public int no;
    GameObject Collider;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).transform.localEulerAngles = new Vector3(90, -180, 90);
        lifetime = Random.Range(1.0f, 2.5f);
        down = true;
        Invoke("Dead", lifetime);

        Destroy(gameObject, 5);
    }


    void Dead()
    {
        Debug.Log(lifetime);
        dead = true;

        if (Collider != null)
        {
            gameObject.transform.SetParent(Collider.transform);
            gameObject.GetComponent<SpiderMovement>().enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.05f) )
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
           
            down = false;
            int i = Random.Range(0, 50);
            if( no % 3 == 0)
            {
                transform.GetChild(0).transform.localEulerAngles = new Vector3(90, 90, 90);
                forward = true;
            }
            else if(no % 2 == 0)
            {
                transform.GetChild(0).transform.localEulerAngles = new Vector3(90, -90, 90);
                left = true;
            }
            else
            {
                transform.GetChild(0).transform.localEulerAngles = new Vector3(90, -180, 90);
                right = true;
            }
            Collider = hit.collider.gameObject;
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 0.05f)  && !down)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
           

            right = false;
            left = false;
            down = true;

            Collider = hit.collider.gameObject;
            

        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 0.05f) && !down)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.yellow);
          

            right = false;
            left = false;
            down = true;

            Collider = hit.collider.gameObject;
           
        }
        else
        {
            right = false;
            left = false;
            down = true;
          //  Collider = null;
        }

        if(down)
        {

            transform.position += Vector3.down * Speed * Time.deltaTime;
        }
        else if(right)
        {
            transform.position += Vector3.right *- Speed * Time.deltaTime;
        }
        else if(left)
        {
            transform.position += Vector3.forward * -Speed * Time.deltaTime;
        } else if(forward)
        {
            transform.position += Vector3.forward * Speed * Time.deltaTime;
        }

    }
}
