using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    private IEnumerator coroutine;
        
    public TMP_Text MainText, DummyText;
    //public string Text = "My apple is green";
    public char[] arr;

    // Start is called before the first frame update
    void Start()
    {
        ChangeText();

        coroutine = WaitAndPrint(0.5f);
        StartCoroutine(coroutine);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // every 2 seconds perform the print()
    private IEnumerator WaitAndPrint(float waitTime)
    {
        string _text = "My apples are ______ coz I am a Queen";
        arr = _text.ToCharArray();

        for (int i = 0; i < arr.Length; i++)
        {
            yield return new WaitForSeconds(waitTime);
            char letter = arr[i];


            DummyText.text = (DummyText.text + letter).ToString();

            //DummyText.GetComponent<Text>().color = Color.green;

        }

        
    }
    public void ChangeText()
    {
        
    }
}
