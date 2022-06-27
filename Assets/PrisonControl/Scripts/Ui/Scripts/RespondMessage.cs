using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

namespace PrisonControl
{
    public class RespondMessage : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI correctMsg, wrongMsg;

        [SerializeField]
        private string [] correct_list, wrong_list;

        [SerializeField]
        private AudioSource audioSource;

        [Foldout("Audio Clips")]
        public AudioClip aud_correct, aud_wrong;

        public void ShowCorrectMsg()
        {
            correctMsg.gameObject.SetActive(true);
            wrongMsg.gameObject.SetActive(false);

            correctMsg.text = correct_list[Random.Range(0, correct_list.Length)];

            Timer.Delay(0.5f, () =>
            {
                PlayAudio(aud_correct);
            });

        }

        public void ShowWrongMsg()
        {
            correctMsg.gameObject.SetActive(false);
            wrongMsg.gameObject.SetActive(true);

            wrongMsg.text = wrong_list[Random.Range(0, wrong_list.Length)];

            audioSource.clip = aud_wrong;

            Timer.Delay(.5f, () =>
            {
                PlayAudio(aud_wrong);
            });
        }

        void PlayAudio(AudioClip audioClip)
        {
            audioSource.clip = audioClip;

            if (Progress.Instance.SFX_ON)
            {
                audioSource.Play();
            }
        }
    }
}
