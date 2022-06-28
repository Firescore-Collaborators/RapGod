using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using PrisonControl;

public class MessageController : MonoBehaviour
{
    public TinderConversationSO conversation;
    bool left = true;
    protected RectTransform contentPanel;
    public ScrollRect scrollRect;
    [Foldout("Prefabs")]
    public GameObject chatRight;
    [Foldout("Prefabs")]
    public GameObject chatLeft;

    [Foldout("ScriptableObjects")]
    public TinderDataSO tinderData;

    [Foldout("UI")]
    public Text userID;

    [Foldout("UI")]
    public Text recipientID;

    [Foldout("UI")]
    public Image userDP;

    [Foldout("UI")]
    public Image recipientDP;

    [Foldout("ChoicePanel")]
    public Transform choicePanel;

    [Foldout("ChoicePanel")]
    public Text choice1Text;

    [Foldout("ChoicePanel")]
    public Text choice2Text;

    public float recepientMessageDelay = 0.5f;

    public TypewriterEffect typewriter;

    public int currentIndex;

    public System.Action onConversationEnd;
    bool setToLastText = false;

    [SerializeField]
    AudioClip aud_type, aud_notification, aud_sendMsg;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnEnable()
    {
        InitData();
        contentPanel = scrollRect.content;
        StartConv();
    }

    private void OnDisable() {
        Reset();
    }

    void Reset()
    {
        for(int i = 0; i < contentPanel.childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
    }

    private void Update() {
        if(setToLastText)
        {
            scrollRect.verticalNormalizedPosition = 0;
        }
    }
    void InitData()
    {
        userID.text = "@" + tinderData.userId;
        recipientID.text = "@" + tinderData.recipientId;
        userDP.sprite = Sprite.Create(tinderData.userDP, new Rect(0, 0, tinderData.userDP.width, tinderData.userDP.height), new Vector2(0.5f, 0.5f));
        recipientDP.sprite = Sprite.Create(tinderData.recipientDP, new Rect(0, 0, tinderData.recipientDP.width, tinderData.recipientDP.height), new Vector2(0.5f, 0.5f));
    }

    void AddButton(string message, bool _left)
    {
        left = _left;
        ChatData chat = Instantiate(left == true ? chatLeft : chatRight, contentPanel).GetComponent<ChatData>();
        chat.message.text = message;
        chat.DP.sprite = left == false ? userDP.sprite : recipientDP.sprite;
        scrollRect.normalizedPosition = new Vector2(0, 1);
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
        audioSource.clip = aud_sendMsg;
        audioSource.Play();
        //SnapTo(chat.GetComponent<RectTransform>());
    }

    void AddButtonTypewriter(string message, bool _left, System.Action callback = null)
    {
        left = _left;
        ChatData chat = Instantiate(left == true ? chatLeft : chatRight, contentPanel).GetComponent<ChatData>();
        chat.message.GetComponent<TypewriterEffect>().WholeText = message;
        setToLastText = true;
        chat.message.GetComponent<TypewriterEffect>().ShowTextResponseDelay(() =>
        {
            callback();
            setToLastText = false;
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
            audioSource.clip = aud_notification;
            audioSource.Play();
        });
        chat.DP.sprite = left == false ? userDP.sprite : recipientDP.sprite;
        scrollRect.normalizedPosition = new Vector2(0, 1);
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
        //SnapTo(chat.GetComponent<RectTransform>());
    }

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        contentPanel.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    void StartConv()
    {
        AddRecipientMessage(conversation.recipientNeutralMessage[0], () =>
        {
            OnChoicePanel();
        });
    }

    public void AddRecipientMessage(string _recipientMessage, System.Action callback = null)
    {
        AddButtonTypewriter(_recipientMessage, true, callback);
    }

    public void AddUserMessage(string _userMessage)
    {
        AddButton(_userMessage, false);
    }

    void OnChoicePanel()
    {
        if (currentIndex >= conversation.userPositiveMessage.Count)
        {
            print("End of conversation");

            onConversationEnd?.Invoke();
            return;
        }


        choice1Text.text = conversation.userPositiveMessage[currentIndex];
        choice2Text.text = conversation.userNegativeMessage[currentIndex];
        Timer.Delay(0.3f, () =>
        {
            choicePanel.gameObject.SetActive(true);
        });
    }

    [Button]
    public void Choice1()
    {
        Progress.Instance.IncreamentRating(1);
        SendReply(choice1Text.text, true);
    }

    [Button]
    public void Choice2()
    {
        Progress.Instance.IncreamentRating(1);
        SendReply(choice2Text.text, false);
    }

    void SendReply(string message, bool positive)
    {
        Debug.Log("play audio typing");

        choicePanel.gameObject.SetActive(false);
        typewriter.WholeText = message;

        audioSource.clip = aud_type;
        audioSource.Play();
        typewriter.ShowTextResponse(() =>
        {
            audioSource.Stop();
            Timer.Delay(0.5f, () =>
            {
                AddUserMessage(typewriter.WholeText);
                typewriter.textMeshProUGUI.text = "Your reply here...";
                Timer.Delay(recepientMessageDelay, () =>
                {
                    if (currentIndex >= conversation.recipientNeutralMessage.Count - 1)
                    {
                        print("End of conversation");

                        onConversationEnd?.Invoke();

                        return;
                    }
                    SendRecipientReply(positive);
                });
            });
        });
    }

    void SendRecipientReply(bool positive)
    {
        string neutral = conversation.recipientNeutralMessage[currentIndex + 1];

        if (positive)
        {
            if (conversation.recipientPositiveReply.Count > currentIndex)
                if (conversation.recipientPositiveReply[currentIndex] != string.Empty)
                {
                    // AddRecipientMessage(conversation.recipientPositiveReply[currentIndex],neutral == string.Empty ? ()=>
                    // {
                    //     OnChoicePanel();
                    // } : null);
                    if (neutral == string.Empty)
                    {
                        AddRecipientMessage(conversation.recipientPositiveReply[currentIndex], () =>
                        {
                            currentIndex++;
                            OnChoicePanel();
                        });
                    }
                    else
                    {
                        AddRecipientMessage(conversation.recipientPositiveReply[currentIndex], () =>
                        {
                            SendNeutralReply(neutral);
                        });

                    }
                }
                else
                {
                    SendNeutralReply(neutral);
                }

        }
        else
        {
            if (conversation.recipientNegativeReply.Count > currentIndex)
                if (conversation.recipientNegativeReply[currentIndex] != string.Empty)
                {
                    // AddRecipientMessage(conversation.recipientNegativeReply[currentIndex],neutral == string.Empty ? ()=>
                    // {
                    //     OnChoicePanel();
                    // } : null);

                    if (neutral == string.Empty)
                    {
                        AddRecipientMessage(conversation.recipientNegativeReply[currentIndex], () =>
                        {
                            currentIndex++;
                            OnChoicePanel();
                        });
                    }
                    else
                    {
                        AddRecipientMessage(conversation.recipientNegativeReply[currentIndex], () =>
                        {
                            SendNeutralReply(neutral);
                        });

                    }
                }
                else
                {
                    SendNeutralReply(neutral);
                }
        }

    }

    void SendNeutralReply(string neutral)
    {
        Timer.Delay(recepientMessageDelay, () =>
        {
            AddRecipientMessage(neutral, () =>
            {
                OnChoicePanel();
            });
            currentIndex++;
        });
    }
}
