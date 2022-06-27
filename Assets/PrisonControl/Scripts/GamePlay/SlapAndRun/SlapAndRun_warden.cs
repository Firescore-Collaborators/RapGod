using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAndRun_warden : MonoBehaviour
{
    [SerializeField]
    private TypewriterEffect txt_conversation;

    [SerializeField]
    private GameObject popup;

    public void ShowConversation(string conversation)
    {
        txt_conversation.WholeText = conversation;

        txt_conversation.ShowTextResponse(() =>
        {
            Timer.Delay(1, () =>
            {
                popup.SetActive(false);
            });
        });
    }

}
