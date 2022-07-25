using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HaircutScript : MonoBehaviour
{
    public GameObject Girl, TrimmerBlack, Chair, GirlChat, ReferenceImage, CameraFinal;
    // Start is called before the first frame update
    void Start()
    {
        
        Invoke("StopAnim", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            StartCoroutine(Entry());
            ReferenceImage.SetActive(false);
            CameraFinal.SetActive(true);
        }
    }

    public void StopAnim()
    {
        Girl.GetComponent<Animator>().enabled = false;
    }

    IEnumerator Entry()
    {
        yield return new WaitForSeconds(1);
        Chair.transform.DORotate(new Vector3(0, -130, 0), 2);

        yield return new WaitForSeconds(2);
        GirlChat.SetActive(true);
        Girl.GetComponent<Animator>().enabled = true;
        Girl.GetComponent<Animator>().SetTrigger("Angry");


    }
}
