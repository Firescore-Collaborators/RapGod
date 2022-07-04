using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.IO;

public class LyricsManager : MonoBehaviour
{
    public TMP_Text InputText;
    public string Hint;
    private TouchScreenKeyboard KB;
    public GameObject WinPanel, LosePanel;
    public TMP_InputField IF;
    public Lyrics_SO[] lyrics_SO;

    // Start is called before the first frame update
    void Start()
    {
        IF.onEndEdit.AddListener(delegate { CheckInput(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            if (IF.text.ToLower().Contains(Hint.ToLower()))
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

    public void Celebrate()
    {
        WinPanel.SetActive(true);
    }

    public void RetryPanel()
    {
        IF.text = "";
        LosePanel.SetActive(true);
    }

    public void OnRetryClick()
    {
        LosePanel.SetActive(false);
    }

    public void CheckInput()
    {
        if (IF.text.ToLower().Contains(Hint.ToLower()))
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
