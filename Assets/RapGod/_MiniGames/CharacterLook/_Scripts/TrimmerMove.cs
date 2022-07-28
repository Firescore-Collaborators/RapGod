using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaintIn3D;

public class TrimmerMove : MonoBehaviour
{
    public Vector3 screenPoint;
    public Vector3 offset;
    public Transform startPos;
    public float Timer;
    public P3dHitNearby hit;
    public Image Loader;
    public ParticleSystem particleHair;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        particleHair.Play();

    }
    void OnMouseDrag()
    {
        Timer += Time.deltaTime;
        if (Timer >= 1)
        {
            hit.enabled = true;
        }

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = Vector3.zero;
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        //curPosition.z = curPosition.y;
        //curPosition.z = transform.position.z;
        //curPosition.y = 0.75f;
        //curPosition.y = transform.position.y;
        curPosition.x = transform.position.x;
        transform.position = curPosition;

        //particleHair.Play();

    }

    private void OnMouseUp()
    {
        Timer = 0;
       hit.enabled = false;

       particleHair.Stop();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
