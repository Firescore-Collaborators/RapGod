using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LyricsManagerNew : MonoBehaviour
{
    public static LyricsManagerNew instance;

    public TMP_Text[] Raps;

    public TMP_Text FinalText, FinalText2;
    public int LevelNo;

    public Lyric_SO2[] LyricsSO;
    public GameObject[] Panel;
    public GameObject MasterPanel, CurrentPanel;

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
    }

    public void Retry()
    {
        LevelNo = 0;

        ClearListData();

        //CurrentPanel = Instantiate(MasterPanel, FindObjectOfType<Canvas>().transform);


        //for (int i = 0; i < LevelNo; i++)
        //{
        //    Panel[i] = CurrentPanel.transform.GetChild(i).gameObject;
        //}

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
