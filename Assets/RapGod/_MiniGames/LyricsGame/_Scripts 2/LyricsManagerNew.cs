using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LyricsManagerNew : MonoBehaviour
{
    public static LyricsManagerNew instance;

    public TMP_Text[] Raps;

    public TMP_Text FinalText;
    public int LevelNo;

    public Lyric_SO2[] LyricsSO;
    public GameObject[] Panel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        //for (int i = 0; i < Raps.Length; i++)
        //{
        //    FinalText.text = FinalText.text + " " + Raps[i].text + "\n";
        //}

        for (int i = 0; i <= LyricsSO.Length; i++)
        {

            {
                //FinalText.text = FinalText.text + " " + LyricsSO[i].Lyrics[0] + LyricsSO[i].option[0]+ LyricsSO[i].Lyrics[1]+ LyricsSO[i].option[1]+ LyricsSO[i].Lyrics[2];
            }
        }
    }

    void Update()
    {
        
    }
}
