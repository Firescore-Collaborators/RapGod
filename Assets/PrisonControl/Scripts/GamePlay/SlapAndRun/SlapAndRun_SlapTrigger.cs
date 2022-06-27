using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAndRun_SlapTrigger : MonoBehaviour
{
    public GameObject obj;
    public int dir;
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
           // GameObject TempObj = 
            other.transform.parent.gameObject.GetComponent<SlapAndRun_PrisionerController>().AddForce(dir,obj,transform.position);
        }
    }
}
