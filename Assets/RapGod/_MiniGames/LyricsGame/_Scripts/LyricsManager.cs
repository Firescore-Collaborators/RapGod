using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class LyricsManager : MonoBehaviour
{
    public TMP_Text Question, FinalLyrics;
    public string Hint;
    public GameObject WinPanel, LosePanel, ParentQP, FinalShow;
    public TMP_InputField IF;
    public Lyrics_SO[] lyrics_SO;
    public TouchScreenKeyboard KB;
    public LyricListSO lyricListSO;

    [SerializeField]
    private int LyricsNo;

    // Start is called before the first frame update
    void OnEnable()
    {
        Question.text = lyricListSO.lyricList[LyricsNo].Question;
        IF.onEndEdit.AddListener(delegate { CheckInput(); });

        Invoke("ShowKB", 0.01f);
    }
    public void ShowKB()
    {
        TouchScreenKeyboard.Open("");
        IF.ActivateInputField();
        ParentQP.GetComponent<Animator>().SetTrigger("show");
    }
    public void Celebrate()
    {
        WinPanel.SetActive(true);
        ParentQP.GetComponent<Animator>().SetTrigger("hide");
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
        if (LyricsNo < lyricListSO.lyricList.Count)
        {
            Question.text = lyricListSO.lyricList[LyricsNo].Question;
            ShowKB();
        }
        else
        {
            IF.enabled = false;
            ParentQP.SetActive(false);
            FinalShow.SetActive(true);
            for (int i = 0; i < lyricListSO.lyricList.Count; i++)
            {
                lyricListSO.lyricList[i].InputLyrics =
                    lyricListSO.lyricList[i].InputLyrics.Replace(lyricListSO.lyricList[i].RightAnswer, "<#FFFFFF>" + lyricListSO.lyricList[i].RightAnswer + "</color>");

                FinalLyrics.text += lyricListSO.lyricList[i].InputLyrics;
                FinalLyrics.text += "\n";

                //FinalLyrics.text = FinalLyrics.text.Replace(lyricListSO.lyricList[i].RightAnswer, "<#FFFFFF>" + lyricListSO.lyricList[i].RightAnswer + " </color>");

            }
        }
    }
    public void OnRetryClick()
    {
        LosePanel.SetActive(false);
        ShowKB();

    }

    public void CheckInput()
    {
        if (IF.text.ToLower().Contains(lyricListSO.lyricList[LyricsNo].RightAnswer.ToLower()))
        {
            lyricListSO.lyricList[LyricsNo].InputLyrics = IF.text;
            Debug.Log("Matched");
            Invoke("Celebrate", 1);
            
        }
        else
        {
            Invoke("RetryPanel", 1);
        }
    }

    
}
