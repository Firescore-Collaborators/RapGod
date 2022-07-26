using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HaircutScript : MonoBehaviour
{
    public GameObject Girl, TrimmerBlack, Chair, GirlChat, ReferenceImage, CameraFinal, Piston1, Piston2, 
        topRing, sideRing, Eyering, ShakePanel, indicatorRing1, indicatorRing2;

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

        if (Input.GetKey(KeyCode.A))
        {
            Piston1.GetComponent<Animator>().enabled = true;
            Invoke("ShakeScreen", 0.5f);
            Invoke("ShakeOff", 4);
            Invoke("ShowJwel1", 2);

        }
        if (Input.GetKey(KeyCode.S))
        {
            Invoke("ShakeScreen", 0.5f);
            Piston2.GetComponent<Animator>().enabled = true;
            Invoke("ShowJwel2", 2);
            Invoke("ShakeOff", 4);

        }
    }

    public void ShakeScreen()
    {
        ShakePanel.SetActive(true);
    }
    public void ShakeOff()
    {
        ShakePanel.SetActive(false);
    }

    public void ShowJwel1()
    {
        topRing.SetActive(true);
        indicatorRing1.SetActive(false);
    }
    public void ShowJwel2()
    {
        indicatorRing2.SetActive(false);
        sideRing.SetActive(true);
    }
    public void ShowJwel3()
    {
        Eyering.SetActive(true);
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
