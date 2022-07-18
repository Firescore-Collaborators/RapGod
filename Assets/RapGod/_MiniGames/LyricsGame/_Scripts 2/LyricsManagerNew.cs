using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PrisonControl;

public class LyricsManagerNew : MonoBehaviour
{
    public static LyricsManagerNew instance;
    public PlayPhasesControl playPhasesControl;
    public TMP_Text[] Raps;

    public TMP_Text FinalText, FinalText2, ScoreText;
    public int LevelNo, Score, SlideDelay;
    public List<Lyric_SO2> lyricsList = new List<Lyric_SO2>();
    public GameObject[] Panel;
    public GameObject MasterPanel, CurrentPanel, OutputScreen, ParentPanel, PermanentHeader;
    [SerializeField] LyricDataSO lyricDataSO;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void OnEnable()
    {
        InitLevelData();
        Init();
 
    }

    void OnDisable()
    {
        Reset();
        
    }

    void InitLevelData()
    {
        if (playPhasesControl == null)
        {
            AssingLyrics(lyricDataSO);
        }
        else
        {
            Level_SO level = playPhasesControl.levels[Progress.Instance.CurrentLevel - 1];
            lyricDataSO = level.GetLyricList;
        }
    }

    void AssingLyrics(LyricDataSO dataSO)
    {
        lyricsList.Clear();
        for (int i = 0; i < dataSO.lyricsList.Count; i++)
        {
            lyricsList.Add(dataSO.lyricsList[i]);
        }
    }

    void Init()
    {
        //Panel[LevelNo].GetComponent<Animator>().SetTrigger("panelON");
        //ClearListData();
        //OutputScreen.GetComponent<Animator>().Play("PanelOff", 0, 0.9f);

        LevelNo = 0;
        Score = 0;

        FinalText.text = ""; FinalText2.text = "";

        ClearListData();
        CurrentPanel = Instantiate(MasterPanel, ParentPanel.transform);
        PermanentHeader.SetActive(true);
        //OutputScreen.gameObject.SetActive(true);

        for (int i = 0; i < Panel.Length; i++)
        {
            if (i == 4)
            {
                Panel[i] = CurrentPanel.transform.GetChild(i + 1).gameObject;
            }
            else
            {
                Panel[i] = CurrentPanel.transform.GetChild(i).gameObject;
            }
        }

        Panel[LevelNo].GetComponent<Animator>().SetTrigger("panelON");
    }


    public void FillLyrics()
    {
        FinalText.text = FinalText.text + " " + lyricsList[LevelNo].Lyrics[0] + " " + "<mark=#FFFF66>" + lyricsList[LevelNo].option[0] + "</mark>"
           + " " + lyricsList[LevelNo].Lyrics[1] + " " + "<mark=#FFFF66>" + lyricsList[LevelNo].option[1] + "</mark>" + " " + lyricsList[LevelNo].Lyrics[2] + "\n";

        FinalText2.text = FinalText2.text + " " + lyricsList[LevelNo].Lyrics[0] + " " + lyricsList[LevelNo].option[0]
           + " " + lyricsList[LevelNo].Lyrics[1] + " " + lyricsList[LevelNo].option[1] + " " + lyricsList[LevelNo].Lyrics[2] + "\n";

        if (lyricsList[LevelNo].option[0] == lyricsList[LevelNo].option1)
        {
            Score++;
        }
        if (lyricsList[LevelNo].option[1] == lyricsList[LevelNo].option2)
        {
            Score++;
        }


        if (Score < 1) { ScoreText.text = "Dumb rapper!"; }
        else if (Score < 4 && Score > 1) { ScoreText.text = "Good"; }
        else if (Score >= 4) { ScoreText.text = "Rap GOD!!"; }

        //= "SCORE: " + Score.ToString();
    }

    public void Retry()
    {
        Init();
        OutputScreen.GetComponent<Animator>().SetTrigger("panelOFF");
        //LevelNo = 0;
        //Score = 0;

        //FinalText.text = ""; FinalText2.text = "";
        //OutputScreen.GetComponent<Animator>().SetTrigger("panelOFF");

        //ClearListData();
        //CurrentPanel = Instantiate(MasterPanel, ParentPanel.transform);
        //PermanentHeader.SetActive(true);
        //OutputScreen.gameObject.SetActive(true);

        ////GameObject[] Panel = new GameObject[5];

        //for (int i = 0; i < Panel.Length; i++)
        //{
        //    if (i == 4)
        //    {
        //        Panel[i] = CurrentPanel.transform.GetChild(i + 1).gameObject;
        //    }
        //    else
        //    {
        //        Panel[i] = CurrentPanel.transform.GetChild(i).gameObject;
        //    }
        //}

        //Panel[LevelNo].GetComponent<Animator>().SetTrigger("panelON");
    }

    public void ClearListData()
    {
        Debug.Log("Option Cleared");
        for (int i = 0; i < LyricsManagerNew.instance.lyricsList.Count; i++)
        {
            LyricsManagerNew.instance.lyricsList[i].option.Clear();
        }
    }

    public void OnLevelComplete()
    {
        LevelEnd();
    }

    void LevelEnd()
    {
        playPhasesControl._OnPhaseFinished();
        Reset();
    }

    public void Reset()
    {
        LevelNo = 0;
        Score = 0;
        FinalText.text = ""; FinalText2.text = "";
        OutputScreen.GetComponent<Animator>().SetTrigger("panelOFF");
        OutputScreen.gameObject.SetActive(false);
        ClearListData();
        
        if (ParentPanel.transform.childCount > 0)
        {
            Destroy(ParentPanel.transform.GetChild(0).gameObject);
        }

        PermanentHeader.SetActive(false);
        //ParentPanel.SetActive(false);
        //Panel[LevelNo].GetComponent<Animator>().SetTrigger("panelON");
    }
}
