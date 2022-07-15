using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualizerManager : MonoBehaviour
{
    public static EqualizerManager instance;

    public GameObject[] squares;
    public AudioSource ASRC;

    public AudioClip incrementPop, matchTing;

    //public GameObject[] slider;

    public GameObject[] Limit;
    public Transform startLimit, endLimit;

    public bool GameOver;
    public GameObject WinPanel;

    public int counter;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameOver)
        //    return;

        //counter = 0;

        //for (int i = 0; i < squares.Length; i++)
        //{
        //    if (squares[i].GetComponent<SliderScript>().isMatched)
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
        for (int i = 0; i < squares.Length; i++)
        {
            if (squares[i].GetComponent<SliderScript>().isMatched)
            {
                squares[i].GetComponent<AudioSource>().Play();
            }
        }
    }

    public void CheckStatus()
    {
        if (GameOver)
            return;

        counter = 0;

        for (int i = 0; i < squares.Length; i++)
        {
            if (squares[i].GetComponent<SliderScript>().isMatched)
            {
                counter++;
            }

            if (counter == 5)
            {
                Debug.Log("Success");
                WinPanel.SetActive(true);
                GameOver = true;
            }
        }
    }

}
