using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.1f;
    public string WholeText;

    //public string FullText
    //{
    //    set{
    //        fullText = value;
    //        StopAllCoroutines();
    //        StartCoroutine(ShowText());
    //    }
    //    get{
    //        return fullText;
    //    }
    //}

    //IEnumerator ShowText()
    //{
    //    for(int i = 0; i <= FullText.Length; i++)
    //    {
    //        currentText = FullText.Substring(0, i);
    //        textMeshPro.text = currentText;
    //        yield return new WaitForSeconds(delay);
    //    }
    //}

    public void SetCurrentText(string text)
    {
        unity_text.text = text;
    }

    public void ShowTextResponse(System.Action callBack = null)
    {
        StopAllCoroutines();
        StartCoroutine(ShowTextWithResponse(callBack));
    }

    public void ShowTextResponseDelay(System.Action callBack = null)
    {
        StopAllCoroutines();
        StartCoroutine(ShowTextWithResponseDelay(callBack));
    }

    string fullText;
    private string currentText = "";
    
    public TextMeshPro textMeshPro
    {
        get
        {
            return GetComponent<TextMeshPro>();
        }
    }

    public TextMeshProUGUI textMeshProUGUI
    {
        get
        {
            return GetComponent<TextMeshProUGUI>();
        }
    }
    
    public Text unity_text
    {
        get
        {
            return GetComponent<Text>();
        }
    }
   

    IEnumerator ShowTextWithResponse(System.Action callBack = null)
    {
        for (int i = 0; i <= WholeText.Length; i++)
        {
            currentText = WholeText.Substring(0, i);

            if (textMeshPro != null)
            {
                textMeshPro.text = currentText;
            }
            else if(unity_text != null)
            {
                unity_text.text = currentText;
            }
            else if(textMeshProUGUI != null)
            {
                textMeshProUGUI.text = currentText;
            }
            
            yield return new WaitForSeconds(delay);
        }
        callBack?.Invoke();
    }

    IEnumerator ShowTextWithResponseDelay(System.Action callBack = null)
    {
        for (int i = 0; i <= WholeText.Length; i++)
        {
            if (textMeshPro != null)
            {
                textMeshPro.text = "typing...";
            }
            else if (unity_text != null)
            {
                unity_text.text = "typing...";
            }
            else if (textMeshProUGUI != null)
            {
                textMeshProUGUI.text = "typing...";
            }
            yield return new WaitForSeconds(delay);
        }

        if (textMeshPro != null)
        {
            textMeshPro.text = WholeText;
        }
        else if (unity_text != null)
        {
            unity_text.text = WholeText;
        }
        else if (textMeshProUGUI != null)
        {
            textMeshProUGUI.text = WholeText;
        }

        callBack?.Invoke();
    }
}
