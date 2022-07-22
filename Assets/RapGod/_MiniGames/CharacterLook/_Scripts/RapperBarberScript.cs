using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RapperBarberScript : MonoBehaviour
{
    public GameObject Chat;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Entry());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Entry()
    {
        yield return new WaitForSeconds(1);
        //transform.GetComponent<Animator>().SetBool("Walk", true);
        transform.GetComponent<Animator>().CrossFade("Walking",0,0);
        transform.DOMoveZ(0.7f, 2);
        //transform.DOMoveX(-1f, 2);


        yield return new WaitForSeconds(1);
        transform.DORotate(new Vector3(0, 90, 0), 1f);

        yield return new WaitForSeconds(1f);
        transform.GetComponent<Animator>().SetBool("Walk", false);
        Chat.transform.GetComponent<Animator>().SetBool("Chat", true);

    }
}
