using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HaircutScript : MonoBehaviour
{
    public GameObject Girl, TrimmerBlack, Chair, GirlChat, ReferenceImage, CameraFinal, Piston1, Piston2, Piston3, 
        topRing, sideRing, Eyering, ShakePanel, indicatorRing1, indicatorRing2, indicatorRing3 ,GirlHeadMesh;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("StopAnim", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        indicatorRing1.transform.Rotate(0,0,-0.5f);
        indicatorRing2.transform.Rotate(0, 0,-0.5f );
        indicatorRing3.transform.Rotate(0, 0, -0.5f);

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
        if (Input.GetKey(KeyCode.D))
        {
            Invoke("ShakeScreen", 0.5f);
            Piston3.GetComponent<Animator>().enabled = true;
            Invoke("ShowJwel3", 2);
            Invoke("ShakeOff", 4);

        }
    }

    public void ShakeScreen()
    {
        ShakePanel.SetActive(true);

        if (Piston1.GetComponent<Animator>().enabled)
        {
            GirlHeadMesh.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 100);
        }
        if (Piston2.GetComponent<Animator>().enabled)
        {
            GirlHeadMesh.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(2, 100);
        }
        if (Piston3.GetComponent<Animator>().enabled)
        {
            GirlHeadMesh.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100);
        }
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
        sideRing.SetActive(true);
        indicatorRing2.SetActive(false);
    }
    public void ShowJwel3()
    {
        Eyering.SetActive(true);
        indicatorRing3.SetActive(false);
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
