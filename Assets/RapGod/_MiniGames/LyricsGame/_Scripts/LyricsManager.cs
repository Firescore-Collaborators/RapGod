using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class LyricsManager : MonoBehaviour
{
    public TMP_Text Question;
    public string Hint;
    public GameObject WinPanel, LosePanel;
    public TMP_InputField IF;
    public Lyrics_SO[] lyrics_SO;

    [SerializeField]
    private int LyricsNo;

    // Start is called before the first frame update
    void OnEnable()
    {
        Question.text = lyrics_SO[LyricsNo].Question;

        IF.onEndEdit.AddListener(delegate { CheckInput(); });
    }

    public void Celebrate()
    {
        WinPanel.SetActive(true);
    }

    public void RetryPanel()
    {
        IF.text = "";
        LosePanel.SetActive(true);
    }
    public void OnWinClick()
    {
        WinPanel.SetActive(false);
        // put next line of questions
        IF.text = "";
        LyricsNo++;
        if (LyricsNo < lyrics_SO.Length)
        {
            Question.text = lyrics_SO[LyricsNo].Question;
        }
    }
    public void OnRetryClick()
    {
        LosePanel.SetActive(false);
    }

    public void CheckInput()
    {
        if (IF.text.ToLower().Contains(lyrics_SO[LyricsNo].RightAnswer.ToLower()))
        {
            Debug.Log("Matched");
            Invoke("Celebrate", 1);
        }
        else
        {
            Invoke("RetryPanel", 1);
        }
    }
}
