using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreamScript : MonoBehaviour
{
    public GameObject Chair, Girl, Chat;
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
        Chair.transform.DORotate(new Vector3(0, -130, 0), 2);

        yield return new WaitForSeconds(2);
        Chat.SetActive(true);
        Girl.GetComponent<Animator>().SetTrigger("Angry");


    }
}
