using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using PrisonControl;

public class TinderBioController : MonoBehaviour
{
    public TinderDataSO tinderData;
    public QADataSO qaData;

    [Foldout("UI")]
    public Image dp;
    [Foldout("UI")]
    public Text handle;

    [Foldout("UI")]
    public Text answer1;
    [Foldout("UI")]
    public Text answer2;

    [Foldout("UI")]
    public Transform answerPanel;
    public RespondMessage respondMessage;
    public TypewriterEffect question;
    public TypewriterEffect answer;

    int currentQuestion = 0;

    public System.Action onConversationEnd;

    AudioSource audioSource;

    [SerializeField]
    AudioClip aud_typing, aud_success;

    [Foldout("Match Screen")]
    public Text matchText;
    [Foldout("Match Screen")]
    public Image leftImage;
    [Foldout("Match Screen")]
    public Image rightImage;
    [Foldout("Match Screen")]
    public GameObject matchScreen;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        handle.text = "@" + tinderData.userId;
        dp.sprite = Sprite.Create(tinderData.userDP, new Rect(0, 0, tinderData.userDP.width, tinderData.userDP.height), new Vector2(0.5f, 0.5f));

        UpdateQuestion();
        UpdateMatchScreen();
    }

    void OnDisable()
    {
        Reset();
    }

    void Reset()
    {

    }

    void UpdateMatchScreen()
    {
        leftImage.sprite = Sprite.Create(tinderData.userDP, new Rect(0, 0, tinderData.userDP.width, tinderData.userDP.height), new Vector2(0.5f, 0.5f));
        rightImage.sprite = Sprite.Create(tinderData.recipientDP, new Rect(0, 0, tinderData.recipientDP.width, tinderData.recipientDP.height), new Vector2(0.5f, 0.5f));
        matchText.text = tinderData.userId + " and " + tinderData.recipientId + " liked each other";
    }
    void UpdateQuestion()
    {
        // question.WholeText = qaData.qaList[currentQuestion].question;
        question.SetCurrentText(qaData.qaList[currentQuestion].question);

        //question.ShowTextResponse(() =>
        //{
        //    AssingAnswers();
        //});

        AssigningAnswers();
        answer.textMeshProUGUI.text = "Your reply here...";
    }

    void AssigningAnswers()
    {
        answer1.text = qaData.qaList[currentQuestion].answer1;
        answer2.text = qaData.qaList[currentQuestion].answer2;

        Timer.Delay(1, () =>
        {
            answerPanel.gameObject.SetActive(true);
        });
    }

    public void Answer1Click()
    {
        Progress.Instance.IncreamentRating(1);
        AnswerClicked(answer1.text);
    }

    public void Answer2Click()
    {
        Progress.Instance.DecreamentRating(1);
        AnswerClicked(answer2.text);
    }

    void AnswerClicked(string _answer)
    {
        answerPanel.gameObject.SetActive(false);
        answer.WholeText = _answer;

        Debug.Log("play audio typing");
        audioSource.clip = aud_typing;
        audioSource.Play();

        answer.ShowTextResponse(() =>
        {
            Debug.Log("stop audio typing");
            audioSource.Stop();
            currentQuestion++;
            if (currentQuestion < qaData.qaList.Count)
            {
                Timer.Delay(0.5f, () =>
                {
                    //    respondMessage.ShowCorrectMsg();
                    Timer.Delay(1, () =>
                    {
                        UpdateQuestion();
                    });
                });

            }
            else
            {
                Timer.Delay(1, () =>
                {
                    audioSource.clip = aud_success;
                    audioSource.Play();
                    matchScreen.SetActive(true);

                    Timer.Delay(3, () =>
                    {
                        MatchClicked();
                    });


                    //question.WholeText = " Your Profile is all set!";
                    //answer.textMeshProUGUI.text = "";
                    /*question.ShowTextResponse(() =>
                    {
                        onConversationEnd?.Invoke();
                        Debug.Log("End of Level");

                        //answer.WholeText = "Have fun with Frndr!";
                        //answer.ShowTextResponse(() =>
                        //{

                        //});
                    });*/
                });
            }
        });
        qaData.qaList[currentQuestion].chosenAsnwer = _answer;
    }

    public void MatchClicked()
    {
       // matchScreen.SetActive(false);
        //question.ShowTextResponse(() =>
        //{
            onConversationEnd?.Invoke();
            Debug.Log("End of Level");

            //answer.WholeText = "Have fun with Frndr!";
            //answer.ShowTextResponse(() =>
            //{

            //});
        //});
    }


}