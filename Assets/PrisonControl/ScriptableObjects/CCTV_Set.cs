using UnityEngine;
using NaughtyAttributes;

namespace PrisonControl
{
    [RequireComponent(typeof(AudioSource))]
    public class CCTV_Set : MonoBehaviour
    {
        public Transform virtual_cam;

        public CCTV_Animation[] allCharacters;

        [SerializeField] float fovOnZoom = 50.0f;
        float fovOnLoad;
        public string description;
        public Punishment punishment;

        AudioSource audioSource;

        [SerializeField]
        AudioClip aud_slap, aud_taze, aud_lowBlow, aud_chickenDance;

        public bool isGuilty;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            allCharacters = GetComponentsInChildren<CCTV_Animation>();

            fovOnLoad = virtual_cam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView;
        }

        void Start()
        {
            StartAnim();
        }

        [Button]
        public void StartAnim()
        {
            for (int i = 0; i < allCharacters.Length; i++)
            {
                allCharacters[i].PlayDefaultAnim();
            }
        }

        public void PlayAnim()
        {
            if (punishment == Punishment.Slap)
            {
                audioSource.clip = aud_slap;
                for (int i = 0; i < allCharacters.Length; i++)
                {
                    allCharacters[i].PlayAnim(punishment.ToString());
                    allCharacters[i].GetComponent<PunishmentParticles>().SlapParticles();
                }
            }
            else if (punishment == Punishment.Taser)
            {
                audioSource.clip = aud_taze;
                for (int i = 0; i < allCharacters.Length; i++)
                {
                    allCharacters[i].PlayAnim(punishment.ToString());
                    allCharacters[i].GetComponent<PunishmentParticles>().ActivateShock();
                }
            }
            else if (punishment == Punishment.LowBlow)
            {
                audioSource.clip = aud_lowBlow;
                for (int i = 0; i < allCharacters.Length; i++)
                {
                    allCharacters[i].PlayAnim(punishment.ToString());
                    allCharacters[i].GetComponent<PunishmentParticles>().LowBlowParticles();
                }
            }
            else if (punishment == Punishment.ChickenDance)
            {
                audioSource.clip = aud_chickenDance;
                for (int i = 0; i < allCharacters.Length; i++)
                {
                    allCharacters[i].PlayAnim(punishment.ToString());
                }
            }

            audioSource.Play();
            
        }

        public void ChangeFOV()
        {
            virtual_cam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = fovOnZoom;
        }

        public void ResetFOV()
        {
            virtual_cam.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = fovOnLoad;
        }
    }
}
