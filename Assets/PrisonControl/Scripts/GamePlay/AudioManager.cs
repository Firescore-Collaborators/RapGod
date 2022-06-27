using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource, audioSourceTap, audioSourceBG;

        [SerializeField]
        private HapticsManager hapticsManager;

        [SerializeField]
        private AudioClip bg_normal, bg_interrogation, bg_slapAndRun;

        public void PlayAudio(AudioClip audioClip)
        {
            Debug.Log("Play audio ");
            audioSource.clip = audioClip;
            audioSource.Play();
            hapticsManager.PlayLightImpact();

        }

        public void PlayTap()
        {
            audioSourceTap.Play();
            hapticsManager.PlayLightImpact();
        }

        public void SetNormalBg()
        {
            audioSourceBG.clip = bg_normal;
            audioSourceBG.Play();
            audioSourceBG.volume = 0.25f;
        }

        public void SetInterrogationBg()
        {
            audioSourceBG.clip = bg_interrogation;
            audioSourceBG.Play();
            audioSourceBG.volume = 0.13f;
        }

        public void SetSlapAndRunBg()
        {
            Debug.Log("slap and run");
            audioSourceBG.clip = bg_slapAndRun;
            audioSourceBG.Play();
            audioSourceBG.volume = 0.25f;
        }
    }
}