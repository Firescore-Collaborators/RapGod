using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class AuditionManager : MonoBehaviour
{
    //public GameObject[] con, Resume;
    //public int ChatNo = 0, resNo = 0;
    public GameObject[] Chic;
    public int chicNo = 0, selectedChicNo = 1;
    public Vector3 startPosition;
    private GameObject CurrentChic;

    [SerializeField]
    private GameObject conversationPopUp;

    [SerializeField]
    private GameObject Resume;
    private Transform startPos;

    [SerializeField]
    private TMP_Text conversation, positiveResp, negetiveResp;

    public int conversationIndex, characterIndex;

    //public Dancers_SO [] dancers_SO;
    public DancerSOList dancerSOList;

    public void OnEnable()
    {
        characterIndex = 0;
        conversationIndex = 0;

        if (chicNo <= dancerSOList.dancersList.Count-1)
        {
            StartCoroutine(Entry());
        }
    }

    void UpdateData()
    {
        conversation.text = dancerSOList.dancersList[chicNo].Conversation[conversationIndex];
        positiveResp.text = dancerSOList.dancersList[chicNo].responsePositive[conversationIndex];
        negetiveResp.text = dancerSOList.dancersList[chicNo].responseNegetive[conversationIndex];

        conversationPopUp.GetComponent<Animator>().SetTrigger("show");

    }

    public void OnNegetiveClicked()
    {
        if (conversationIndex == 0)
            FirstNegetive();
        else
        if (conversationIndex == 1)
            SecondNegetetive();

        conversationIndex++;

        if (conversationIndex > 1)
            conversationIndex = 0;
    }
    void FirstNegetive()
    {
        //con[ChatNo].SetActive(false);
        conversationPopUp.GetComponent<Animator>().SetTrigger("hide");
        //ChatNo++;
        
        StartCoroutine(ActivateCon());        
    }
    
    void SecondNegetetive()
    {
        CurrentChic.GetComponent<Animator>().SetTrigger("SadWalk");
        StartCoroutine(Exit());
        //con[ChatNo].SetActive(false);
       // conversationPopUp.SetActive(false);
        conversationPopUp.GetComponent<Animator>().SetTrigger("hide");
       
    }

    public void OnPositiveClicked()
    {
        CurrentChic.GetComponent<Animator>().SetTrigger("Win");
        StartCoroutine(HappyExit());
        Resume.GetComponent<Animator>().SetTrigger("hide");
        //con[ChatNo].SetActive(false);
        //   conversationPopUp.SetActive(false);

        conversationPopUp.GetComponent<Animator>().SetTrigger("hide");
    }

    IEnumerator Exit()
    {
        Resume.GetComponent<Animator>().SetTrigger("hide");
        yield return new WaitForSeconds(1);
        CurrentChic.transform.DORotate(new Vector3(0, 90, 0), 1);

        yield return new WaitForSeconds(1.5f);
        CurrentChic.transform.DOMoveX(1.68f, 3);
        CurrentChic.GetComponent<Animator>().SetTrigger("Walk");

        yield return new WaitForSeconds(3);
        CurrentChic.gameObject.SetActive(false);

        chicNo++;
        //ChatNo++;

        if (chicNo <= dancerSOList.dancersList.Count-1)
        {
            StartCoroutine(Entry());
        }
        
    }
    IEnumerator HappyExit()
    {
        yield return new WaitForSeconds(1.5f);
        CurrentChic.transform.DORotate(new Vector3(0, 180, 0), 1);

        yield return new WaitForSeconds(1);
        CurrentChic.GetComponent<Animator>().SetBool("Walk1", true);

        yield return new WaitForSeconds(1);
        CurrentChic.transform.DOMove(new Vector3(-0.5f+selectedChicNo*0.3f, 0, 0), 5);

        yield return new WaitForSeconds(5);
        CurrentChic.transform.DORotate(new Vector3(0, 0, 0), 1);
        CurrentChic.GetComponent<Animator>().SetBool("Walk1", false);
        
        chicNo++;

        selectedChicNo++;

        if (chicNo <= dancerSOList.dancersList.Count-1)
        {
            StartCoroutine(Entry());
        }
 
    }

    IEnumerator Entry()
    {

        CurrentChic = Instantiate(dancerSOList.dancersList[chicNo].character, startPosition, Quaternion.Euler(0,90,0));

        yield return new WaitForSeconds(1);
        CurrentChic.transform.DOMoveX(0, 2);
        CurrentChic.GetComponent<Animator>().SetBool("Walk1", true);

        yield return new WaitForSeconds(2);
        CurrentChic.GetComponent<Animator>().SetBool("Walk1", false);
        CurrentChic.transform.DORotate(new Vector3(0, 0, 0), 0.5f);

        UpdateData();

        Resume.GetComponent<Animator>().SetTrigger("show");
        Resume.GetComponentInChildren<TMP_Text>().text = dancerSOList.dancersList[chicNo].Resume[0]+"\n"+
            dancerSOList.dancersList[chicNo].Resume[1]+"\n"+ dancerSOList.dancersList[chicNo].Resume[2];
        conversationIndex = 0;
    }

    IEnumerator ActivateCon()
    {

        yield return new WaitForSeconds(1);
        //con[ChatNo].SetActive(true);
        UpdateData();
        conversationPopUp.GetComponent<Animator>().SetTrigger("show");

    }

    private void OnDisable()
    {
        //for (int i = 0; i < dancers_SO.Length; i++)
        //{
        //    Destroy(dancers_SO[chicNo].character);
        //}

    }
}
