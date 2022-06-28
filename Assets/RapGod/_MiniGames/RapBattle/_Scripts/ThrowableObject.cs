using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    LTBezierPath cr;
    public GameObject target;
    public float speed = 3;
    public GameObject Prefab;
    public bool splash = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        int randomObject = Random.Range(0, gameObject.transform.childCount);
        gameObject.transform.GetChild(randomObject).gameObject.SetActive(true);

        int i = Random.Range(0, 2);
        if( i ==0)
        {
            splash = true;
        }
    }

    LTDescr descr;
    void Start()
    {
        Vector3 mid = (target.transform.position + transform.position) / 2;
        mid.y = mid.y + 1.5f;

        Vector3 targetPos = target.transform.position;
        targetPos.y = targetPos.y + Random.Range(0.3f, 1f);
        cr = new LTBezierPath(new Vector3[] { transform.position, mid, mid, targetPos });
        
        StartCoroutine("OnShoot");
    }
   
    IEnumerator OnShoot()
    {
        descr = LeanTween.move(this.gameObject, cr.pts, speed).setOrientToPath(true).setRepeat(0);
        yield return new WaitForSeconds(speed-0.1f);
        gameObject.AddComponent<Rigidbody>().AddForceAtPosition(gameObject.transform.forward*0.2f, target.transform.position, ForceMode.Impulse);
        descr.cancel(this.gameObject);


    }

    public void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.layer == 8)
        {
           if(splash)
            Instantiate(Prefab, gameObject.transform.position, Quaternion.identity);
        }
    }
}
