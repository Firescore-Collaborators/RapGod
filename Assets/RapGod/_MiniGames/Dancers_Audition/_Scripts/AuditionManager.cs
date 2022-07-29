using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using PrisonControl;
public class AuditionManager : MonoBehaviour
{
    //public GameObject[] con, Resume;
    //public int ChatNo = 0, resNo = 0;
    public PlayPhasesControl playPhasesControl;
    MultiTouchManager multiTouchManager
    {
        get
        {
            return GetComponent<MultiTouchManager>();
        }
    }
    public GameObject[] Chic;
    public int chicNo, selectedChicNo;
    public Vector3 startPosition;
    private GameObject CurrentChic;
    private bool isDisabled;

    [SerializeField]
    private GameObject conversationPopUp, DancerParent;

    [SerializeField]
    private GameObject Resume;
    private Transform startPos;

    [SerializeField]
    private TMP_Text conversation, positiveResp, negetiveResp;

    public int conversationIndex, characterIndex;

    //public Dancers_SO [] dancers_SO;
    public DancerSOList dancerSOList;
    public GameObject requirementPrefab;
    public GameObject tellMeMoreButton, tapPanel;
    List<GameObject> selectedGirls = new List<GameObject>();
    List<GameObject> spawnedGirls = new List<GameObject>();
    public void OnEnable()
    {
        InitLevelData();
        Init();
    }

    void InitLevelData()
    {
        Level_SO level = playPhasesControl.levels[Progress.Instance.CurrentLevel - 1];
        dancerSOList = level.GetAuditionSO;
        multiTouchManager.inputSequence = dancerSOList.inputSequenceSO;
        GetComponent<TouchInputs>().multiTapLimit = dancerSOList.tapSmashLimit;
    }

    void Init()
    {
        MainCameraController.instance.SetCurrentCamera("GirlAudition", 0);
        isDisabled = false;
        characterIndex = 0;
        conversationIndex = 0;
        chicNo = 0;
        selectedChicNo = 0;

        if (chicNo <= dancerSOList.dancersList.Count - 1)
        {
            StartCoroutine(Entry());
        }
        AssingRequirements();
    }
    void UpdateData()
    {
        conversation.text = dancerSOList.dancersList[chicNo].dancerSO.Conversation[conversationIndex];
        positiveResp.text = dancerSOList.dancersList[chicNo].dancerSO.responsePositive[conversationIndex];
        negetiveResp.text = dancerSOList.dancersList[chicNo].dancerSO.responseNegetive[conversationIndex];

        conversationPopUp.GetComponent<Animator>().SetTrigger("show");
    }

    void AssingRequirements()
    {
        for (int i = 0; i < dancerSOList.requirements.Count; i++)
        {
            GameObject pref = Instantiate(requirementPrefab, Resume.transform);
            pref.transform.GetChild(0).GetComponent<Text>().text = dancerSOList.requirements[i].requirementText;
            pref.transform.GetChild(1).transform.GetChild(dancerSOList.requirements[i].allowed ? 0 : 1).gameObject.SetActive(true);
        }
    }

    public void OnNegetiveClicked()
    {
        tellMeMoreButton.SetActive(false);
        if (conversationIndex == 0)
            FirstNegetive();
        else
        if (conversationIndex == 1)
            Reject();

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

    public void Reject()
    {
        if (dancerSOList.dancersList[chicNo].rightOption)
        {
            WrongChoice();
        }
        else
        {
            CurrentChic.GetComponent<Animator>().SetTrigger("SadWalk");
            StartCoroutine(Exit());
            //con[ChatNo].SetActive(false);
            // conversationPopUp.SetActive(false);
            conversationPopUp.GetComponent<Animator>().SetTrigger("hide");
        }


    }

    public void OnPositiveClicked()
    {
        if (!dancerSOList.dancersList[chicNo].rightOption)
        {
            WrongChoice();
        }
        else
        {
            CurrentChic.GetComponent<Animator>().SetTrigger("Win");
            StartCoroutine(HappyExit());
            //Resume.GetComponent<Animator>().SetTrigger("hide");
            //con[ChatNo].SetActive(false);
            //   conversationPopUp.SetActive(false);
            selectedGirls.Add(CurrentChic);
            conversationPopUp.GetComponent<Animator>().SetTrigger("hide");
        }

    }

    IEnumerator Exit()
    {
        //Resume.GetComponent<Animator>().SetTrigger("hide");
        yield return new WaitForSeconds(1);
        CurrentChic.transform.DORotate(new Vector3(0, 90, 0), 1);

        yield return new WaitForSeconds(1.5f);
        CurrentChic.transform.DOMoveX(1.68f, 1.5f);
        CurrentChic.GetComponent<Animator>().SetTrigger("Walk");

        yield return new WaitForSeconds(1.5f);
        CurrentChic.gameObject.SetActive(false);

        chicNo++;
        //ChatNo++;

        if (chicNo <= dancerSOList.dancersList.Count - 1)
        {
            StartCoroutine(Entry());
        }
        else
        {
            StartTwerk();
        }

    }
    IEnumerator HappyExit()
    {
        yield return new WaitForSeconds(1.5f);
        CurrentChic.transform.DORotate(new Vector3(0, 180, 0), 1);

        yield return new WaitForSeconds(1);
        CurrentChic.GetComponent<Animator>().SetBool("Walk1", true);

        yield return new WaitForSeconds(1);
        CurrentChic.transform.DOMove(new Vector3(-0.5f + selectedChicNo * 0.3f, 0, 0), 2.5f);

        yield return new WaitForSeconds(2.5f);
        CurrentChic.transform.DORotate(new Vector3(0, 0, 0), 1);
        CurrentChic.GetComponent<Animator>().SetBool("Walk1", false);

        chicNo++;

        selectedChicNo++;

        if (chicNo <= dancerSOList.dancersList.Count - 1)
        {
            StartCoroutine(Entry());
        }
        else
        {
            StartTwerk();
        }
    }

    IEnumerator Entry()
    {
        if (!isDisabled)
        {
            Resume.transform.parent.gameObject.SetActive(true);
            tellMeMoreButton.SetActive(true);
            CurrentChic = Instantiate(dancerSOList.dancersList[chicNo].dancerSO.character, startPosition, Quaternion.Euler(0, 90, 0), DancerParent.transform);
            spawnedGirls.Add(CurrentChic);
            yield return new WaitForSeconds(1);
            CurrentChic.transform.DOMoveX(0, 2);
            CurrentChic.GetComponent<Animator>().SetBool("Walk1", true);

            yield return new WaitForSeconds(2);
            CurrentChic.GetComponent<Animator>().SetBool("Walk1", false);
            CurrentChic.transform.DORotate(new Vector3(0, 0, 0), 0.5f);

            UpdateData();

            //Resume.GetComponent<Animator>().SetTrigger("show");
            // Resume.GetComponentInChildren<TMP_Text>().text = dancerSOList.dancersList[chicNo].Resume[0] + "\n" +
            //     dancerSOList.dancersList[chicNo].Resume[1] + "\n" + dancerSOList.dancersList[chicNo].Resume[2];
            conversationIndex = 0;
        }
    }

    IEnumerator ActivateCon()
    {
        yield return new WaitForSeconds(1);
        //con[ChatNo].SetActive(true);
        UpdateData();
        conversationPopUp.GetComponent<Animator>().SetTrigger("show");
    }

    void StartTwerk()
    {
        Resume.transform.parent.gameObject.SetActive(false);
        MainCameraController.instance.SetCurrentCamera("TwerkCamera");
        Timer.Delay(2f, () =>
        {
            TwerkInit();
        });
    }

    void TwerkInit()
    {
        tapPanel.SetActive(true);
        multiTouchManager.onMultiTaping += PlayGirlAnim;
        multiTouchManager.onInputRaised += OnTapOver;
        multiTouchManager.Init();
    }

    void PlayGirlAnim(float value, bool tapped)
    {
        if (tapped)
        {
            OnTapped();
        }
    }

    void OnTapped()
    {
        for (int i = 0; i < selectedGirls.Count; i++)
        {
            selectedGirls[i].GetComponent<Animator>().Play(dancerSOList.girlanim.ToString());
        }
    }

    void OnTapOver()
    {
        tapPanel.SetActive(false);
        Timer.Delay(8f,()=>
        {
            LevelComplete();
        });
    }

    void LevelComplete()
    {
        Reset();
        playPhasesControl._OnPhaseFinished();
    }

    void WrongChoice()
    {

    }

    private void OnDisable()
    {
        isDisabled = true;

        for (int i = 0; i < DancerParent.transform.childCount; i++)
        {
            //DancerParent.transform.GetChild(i).gameObject.SetActive(false);
            Destroy(DancerParent.transform.GetChild(i).gameObject);
        }

        //for (int i = 0; i < dancerSOList.dancersList.Count; i++)
        //{
        //    Destroy(dancerSOList.dancersList[chicNo].character);
        //}
    }

    void Reset()
    {
        for (int i = 0; i < spawnedGirls.Count; i++)
        {
            Destroy(spawnedGirls[i]);
        }
        spawnedGirls.Clear();
        selectedGirls.Clear();
        multiTouchManager.Reset();
    }

}
