using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [SerializeField]
        private AudioSource audioSourceBG, audioSourceSFX;

        [SerializeField]
        private HapticsManager hapticsManager;
        [SerializeField] AudioClip defaultBGMusic;

        void OnDisable()
        {
        }
        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }    

        void Start()
        {
            defaultBGMusic = audioSourceBG.clip;
        }

        public void PlayBGMusicDefault()
        {
            if(audioSourceBG.clip != defaultBGMusic)
            {
                audioSourceBG.clip = defaultBGMusic;
                print("Playing default BG music");
                audioSourceBG.Play();
            }
        }

        public void PlayBGMusic(AudioClip clip)
        {
            audioSourceBG.clip = clip;
            audioSourceBG.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            audioSourceSFX.clip = clip;
            audioSourceSFX.Play();
        }
    }
}