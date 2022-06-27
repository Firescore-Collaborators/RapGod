using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace PrisonControl
{
    public class InterrogationUi : MonoBehaviour
    {
        [SerializeField]
        private GameObject step1, step2, step3;

        public Action conversationStarted;
        public Action<bool> step1Response;
        public Action<bool> step2Response;
        public Action<bool> step3Response;

        [SerializeField]
        public TypewriterEffect[] txt_conversation;

        [SerializeField]
        private Text[] txt_positiveBtn;

        [SerializeField]
        private Text[] txt_negetiveBtn;

        [SerializeField]
        private TypewriterEffect txt_intro;

        [SerializeField]
        private Text txt_reason;

        private Interrogation_SO interrogationInfo;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Setup(Interrogation_SO _interrogationInfo)
        {
            interrogationInfo = _interrogationInfo;

            txt_reason.text = interrogationInfo.reasonText;
            txt_intro.transform.parent.gameObject.SetActive(true);

            Timer.Delay(0.2f, () =>
            {
                ShowPopUp(txt_intro, interrogationInfo.intro, txt_reason.transform.parent.gameObject, interrogationInfo.aud_intro);
            });

            for (int i = 0; i < 3; i++)
            {
                txt_positiveBtn[i].text = interrogationInfo.positiveResponse[i];
                txt_negetiveBtn[i].text = interrogationInfo.negetiveResponse[i];
            }
        }

        public void ReasonBtn()
        {
            txt_reason.transform.parent.gameObject.SetActive(false);
            txt_intro.transform.parent.gameObject.SetActive(false);
            conversationStarted?.Invoke();

            audioSource.clip = interrogationInfo.aud_reason;
            audioSource.Play();

            Timer.Delay(audioSource.clip.length + 0.5f, () =>
            {
                ShowPopUp(txt_conversation[0], interrogationInfo.conversation[0], step1, interrogationInfo.aud_conversation[0]);
            });

        }

        // Conversation Response1
        public void Step1Btn1(bool isPositive)
        {
            step1.SetActive(false);
            step1Response?.Invoke(isPositive);
            txt_conversation[0].transform.parent.gameObject.SetActive(false);

            if (isPositive)
                audioSource.clip = interrogationInfo.aud_positiveResponse[0];
            else
                audioSource.clip = interrogationInfo.aud_negetiveResponse[0];

            audioSource.Play();

            Timer.Delay(audioSource.clip.length + 0.5f, () =>
            {
                ShowPopUp(txt_conversation[1], interrogationInfo.conversation[1], step2, interrogationInfo.aud_conversation[1]);
            });
        }

        // Conversation Response2
        public void Step2Btn1(bool isPositive)
        {
            step2.SetActive(false);
            step2Response?.Invoke(isPositive);
            txt_conversation[1].transform.parent.gameObject.SetActive(false);

            if (isPositive)
                audioSource.clip = interrogationInfo.aud_positiveResponse[1];
            else
                audioSource.clip = interrogationInfo.aud_negetiveResponse[1];
            audioSource.Play();

            Timer.Delay(audioSource.clip.length + 0.5f, () =>
            {
                ShowPopUp(txt_conversation[2], interrogationInfo.conversation[2], step3, interrogationInfo.aud_conversation[2]);
            });
        }


        // Conversation Response3
        public void Step3Btn1(bool isPositive)
        {
            step3.SetActive(false);
            txt_conversation[2].transform.parent.gameObject.SetActive(false);
            if (isPositive)
                audioSource.clip = interrogationInfo.aud_positiveResponse[2];
            else
                audioSource.clip = interrogationInfo.aud_negetiveResponse[2];
            audioSource.Play();

            Timer.Delay(audioSource.clip.length + 0.5f, () =>
            {
                step3.SetActive(false);
                step3Response?.Invoke(isPositive);
            });
        }

        void ShowPopUp(TypewriterEffect typeText, string text, GameObject panel, AudioClip clip)
        {
            typeText.transform.parent.gameObject.SetActive(true);
            typeText.WholeText = text;
            typeText.ShowTextResponse(() =>
            {
                panel.SetActive(true);
            });
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
