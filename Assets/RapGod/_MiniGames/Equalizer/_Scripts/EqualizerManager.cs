using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrisonControl;
public class EqualizerManager : MonoBehaviour
{
    public static EqualizerManager instance;
    public PlayPhasesControl playPhasesControl;
    public GameObject[] slider;
    public AudioSource ASRC;

    public AudioClip incrementPop, matchTing;

    public GameObject[] Limit;
    public Transform startLimit, endLimit;

    public bool GameOver;
    public GameObject WinPanel;
    public SliderSOList sliderSOList;

    public Material RedMat, GreenMat;

    public int counter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        InitLevelData();
        Init();
    }

    void InitLevelData()
    {
        Level_SO level = playPhasesControl.levels[Progress.Instance.CurrentLevel - 1];
        sliderSOList = level.GetEqualizerSO;
    }

    void Init()
    {
        for (int i = 0; i < slider.Length; i++)
        {
            slider[i].GetComponent<SliderScript>().Reading = sliderSOList.reading[i];
            //slider[i].GetComponent<SliderScript>().startPos = slider[i].transform;
            slider[i].transform.position = new Vector3(slider[i].transform.position.x, slider[i].transform.position.y, startLimit.transform.position.z);
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < slider.Length; i++)
        {
            //slider[i].transform.position = new Vector3 (slider[i].transform.position.x, slider[i].transform.position.y, startLimit.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameOver)
        //    return;

        //counter = 0;

        //for (int i = 0; i < slider.Length; i++)
        //{
        //    if (slider[i].GetComponent<SliderScript>().isMatched)
        //    {
        //        //ASRC.clip = matchTing;
        //        //ASRC.Play();

        //        //if (!ASRC.isPlaying)
        //        //{
        //        //    ASRC.clip = matchTing;
        //        //    ASRC.Play();
        //        //}
        //        counter++;
        //    }

        //    if (counter == 5)
        //    {
        //        Debug.Log("Success");
        //        WinPanel.SetActive(true);
        //        GameOver = true;
        //    }
        //}        
    }

    public void PlayMusic()
    {
        for (int i = 0; i < slider.Length; i++)
        {
            if (slider[i].GetComponent<SliderScript>().isMatched)
            {
                slider[i].GetComponent<AudioSource>().Play();
            }
        }
    }

    public void CheckStatus()
    {
        if (GameOver)
            return;

        counter = 0;

        for (int i = 0; i < slider.Length; i++)
        {
            if (slider[i].GetComponent<SliderScript>().isMatched)
            {
                counter++;
            }

            if (counter == 5)
            {
                Debug.Log("Success");
                WinPanel.SetActive(true);
                GameOver = true;
                Timer.Delay(2f, () =>
                {
                    LevelComplete();
                });
            }
        }
    }

    void LevelComplete()
    {
        playPhasesControl._OnPhaseFinished();
        Reset();
    }

    void Reset()
    {
        WinPanel.SetActive(false);
        counter = 0;
        GameOver = false;
        WinPanel.SetActive(false);
        for (int i = 0; i < slider.Length; i++)
        {
            slider[i].GetComponent<SliderScript>().Indicator1.GetComponent<MeshRenderer>().material = RedMat;
            slider[i].GetComponent<SliderScript>().Indicator2.GetComponent<MeshRenderer>().material = RedMat;
            slider[i].GetComponent<SliderScript>().isMatched = false;
        }
    }
}
