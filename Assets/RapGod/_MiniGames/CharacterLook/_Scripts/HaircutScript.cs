using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaircutScript : MonoBehaviour
{
    public GameObject Girl, TrimmerBlack;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StopAnim", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopAnim()
    {
        Girl.GetComponent<Animator>().enabled = false;
    }
}
