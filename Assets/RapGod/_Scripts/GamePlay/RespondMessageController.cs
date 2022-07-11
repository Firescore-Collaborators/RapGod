using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;

public class RespondMessageController : MonoBehaviour
{
    public List<string> correctMessage = new List<string>();
    public List<string> correctList1 = new List<string>();
    public List<string> correctList2 = new List<string>();
    public List<string> wrongMessage = new List<string>();
    public float scaleValue = 1.2f;
    public float moveValue = 20f;
    public float scaleSpeed = 0.50f;
    public float moveSpeed = 5f;
    public Text messageText;
    public Vector3 startPos;
    DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> scaleTween;
    DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> moveTween;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    void Start()
    {
        startPos = messageText.transform.position;
    }
    public void ShowCorrectResponse()
    {
        int rand = Random.Range(0, correctMessage.Count);
        messageText.text = correctMessage[rand];
        SetTextColor(correctColor);
        MoveText();
    }

    public void ShowCorrectResponse(List<string> correctList)
    {
        int rand = Random.Range(0, correctList.Count);
        messageText.text = correctList[rand];
        SetTextColor(correctColor);
        MoveText();
    }

    public void ShowCorrectRespone(string text)
    {
        messageText.text = text;
        SetTextColor(correctColor);
        MoveText();
    }
    
    public void ShowWrongResponse(string text)
    {
        messageText.text = text;
        SetTextColor(wrongColor);
        MoveText();
    }

    public void ShowWrongResponse()
    {
        int rand = Random.Range(0, wrongMessage.Count);
        messageText.text = wrongMessage[rand];
        SetTextColor(wrongColor);
        MoveText();
    }

    void SetTextColor(Color color)
    {
        messageText.color = color;
    }

    [Button]
    void MoveText()
    {
        scaleTween.Kill();
        moveTween.Kill();
        messageText.transform.position = startPos;
        messageText.gameObject.SetActive(true);
        scaleTween = messageText.transform.DOScale(messageText.transform.localScale + (Vector3.one * scaleValue), scaleSpeed).SetLoops(2, LoopType.Yoyo);
        moveTween = messageText.transform.DOMoveY(messageText.transform.position.y + moveValue, moveSpeed).OnComplete(() =>
        {
            messageText.gameObject.SetActive(false);
        });
    }

}