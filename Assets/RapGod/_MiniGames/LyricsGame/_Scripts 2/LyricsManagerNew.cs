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
    public int LevelNo, Score;
    public List<Lyric_SO2> lyricsList = new List<Lyric_SO2>();
    public GameObject[] Panel;
    public GameObject MasterPanel, CurrentPanel, OutputScreen, ParentPanel;
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
        Panel[LevelNo].GetComponent<Animator>().SetTrigger("panelON");


        ClearListData();

        //for (int i = 0; i < Raps.Length; i++)
        //{
        //    FinalText.text = FinalText.text + " " + Raps[i].text + "\n";
        //}

        for (int i = 0; i < lyricsList.Count; i++)
        {

            {
                //FinalText.text = FinalText.text + " " + lyricsList[i].Lyrics[0] + lyricsList[i].option[0]+ lyricsList[i].Lyrics[1]+ lyricsList[i].option[1]+ lyricsList[i].Lyrics[2];
                //FinalText.text = FinalText.text + " " + lyricsList[i].Lyrics[0];
            }
        }
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
        LevelNo = 0;
        Score = 0;

        FinalText.text = ""; FinalText2.text = "";
        OutputScreen.GetComponent<Animator>().SetTrigger("panelOFF");

        ClearListData();
        CurrentPanel = Instantiate(MasterPanel, ParentPanel.transform);

        //GameObject[] Panel = new GameObject[5];

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

        //for (int i = 1; i < LevelNo; i++)
        //{
        //    Panel[i].SetActive(false);
        //}
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

    void Reset()
    {
        LevelNo = 0;
        Score = 0;
        FinalText.text = ""; FinalText2.text = "";
        OutputScreen.gameObject.SetActive(false);
        OutputScreen.GetComponent<Animator>().SetTrigger("panelOFF");
        ClearListData();
        //ParentPanel.SetActive(false);
        //Panel[LevelNo].GetComponent<Animator>().SetTrigger("panelON");
    }
}
