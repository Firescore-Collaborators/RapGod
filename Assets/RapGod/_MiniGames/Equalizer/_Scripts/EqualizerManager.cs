using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualizerManager : MonoBehaviour
{
    public static EqualizerManager instance;

    public GameObject[] squares;

    //public GameObject[] slider;

    public GameObject[] Limit;
    public Transform startLimit, endLimit;

    public bool GameOver;
    public GameObject WinPanel;

    int counter;

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
