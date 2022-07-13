using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LyricsManagerNew : MonoBehaviour
{
    public static LyricsManagerNew instance;

    public TMP_Text[] Raps;

    public TMP_Text FinalText, FinalText2;
    public int LevelNo, Score;

    public Lyric_SO2[] LyricsSO;
    public GameObject[] Panel;
    public GameObject MasterPanel, CurrentPanel, OutputScreen;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        Panel[LevelNo].GetComponent<Animator>().SetTrigger("panelON");

        ClearListData();

        //for (int i = 0; i < Raps.Length; i++)
        //{
        //    FinalText.text = FinalText.text + " " + Raps[i].text + "\n";
        //}

        for (int i = 0; i < LyricsSO.Length; i++)
        {

            {
                //FinalText.text = FinalText.text + " " + LyricsSO[i].Lyrics[0] + LyricsSO[i].option[0]+ LyricsSO[i].Lyrics[1]+ LyricsSO[i].option[1]+ LyricsSO[i].Lyrics[2];
                //FinalText.text = FinalText.text + " " + LyricsSO[i].Lyrics[0];
            }
        }
    }

    void Update()
    {
        
    }

    public void FillLyrics()
    {
        FinalText.text = FinalText.text + " "+ LyricsSO[LevelNo].Lyrics[0] +" " + "<mark=#FFFFFF>" + LyricsSO[LevelNo].option[0]+ "</mark>"
           + " " + LyricsSO[LevelNo].Lyrics[1] +" "+ "<mark=#FFFFFF>" + LyricsSO[LevelNo].option[1] +"</mark>" + " " + LyricsSO[LevelNo].Lyrics[2] + "\n";

        FinalText2.text = FinalText2.text + " " + LyricsSO[LevelNo].Lyrics[0] + " " + LyricsSO[LevelNo].option[0]
           + " " + LyricsSO[LevelNo].Lyrics[1] + " " + LyricsSO[LevelNo].option[1] + " " + LyricsSO[LevelNo].Lyrics[2] + "\n";

        if (LyricsSO[LevelNo].option[0] == LyricsSO[LevelNo].option1)
        {
            Score++;
        }
        if (LyricsSO[LevelNo].option[1] == LyricsSO[LevelNo].option2)
        {
            Score++;
        }
    }

    public void Retry()
    {
        LevelNo = 0;

        FinalText.text = ""; FinalText2.text = "";
        OutputScreen.GetComponent<Animator>().SetTrigger("panelOFF");

        ClearListData();
        CurrentPanel = Instantiate(MasterPanel, FindObjectOfType<Canvas>().transform.GetChild(0));

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
        for (int i = 0; i < LyricsManagerNew.instance.LyricsSO.Length; i++)
        {
            LyricsManagerNew.instance.LyricsSO[i].option.Clear();
        }
    }
}
