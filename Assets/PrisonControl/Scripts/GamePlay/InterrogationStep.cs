using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

namespace PrisonControl
{
    public class InterrogationStep : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private GamePlayStep _mGamePlayStep;

        [SerializeField]
        private CopManager copManager;

        [SerializeField]
        public InterrogationUi interrogationUi;

        private int conversationIndex;

        [SerializeField]
        private Transform guestHolder;

        private GameObject currentGuest;

        [SerializeField]
        private CopHandler copHandler;

        [SerializeField]
        private GameObject interrogateRoom;

        [SerializeField]
        private GameObject popUp;

        [SerializeField]
        private GameObject chair;

        [SerializeField]
        private Transform copIdlePos, copInPos, copOutPos;

        [SerializeField]
        private Transform guestOutPos;

        [SerializeField]
        private AudioManager audioManager;

        [Foldout("Audio Clips")]
        public AudioClip aud_chatBubble, aud_arrest, aud_release, aud_slap, aud_tase, aud_lowblow, aud_hammerHit, aud_chickenDance, aud_spiderBucket, aud_spit;

        [SerializeField]
        private GameObject hammer, spit;


        private void Awake()
        {
            interrogationUi.conversationStarted += ConversationStarted;

            interrogationUi.step1Response += Step1Respnse;
            interrogationUi.step2Response += Step2Respnse;
            interrogationUi.step3Response += Step3Respnse;
        }

        private void Destroy()
        {
            interrogationUi.conversationStarted -= ConversationStarted;

            interrogationUi.step1Response -= Step1Respnse;
            interrogationUi.step2Response -= Step2Respnse;
            interrogationUi.step3Response -= Step3Respnse;
        }

        void OnEnable()
        {
            conversationIndex = 0;
            Setup();

            copHandler.ReturnSelectedAvatar().transform.position = copIdlePos.position;
        }

       
        void Setup()
        {

            if(currentGuest)
                Destroy(currentGuest);

            popUp.SetActive(true);
            interrogateRoom.SetActive(true);
            interrogationUi.Setup(_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo);

            currentGuest = Instantiate(_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].Guests[0].pf_character, guestHolder);
            currentGuest.GetComponent<Animator>().applyRootMotion = true;
            currentGuest.GetComponent<GuestAnimationManager>().PlayIdleSit();

            copHandler.ReturnSelectedAvatar().SetUniform();
        }

        void ConversationStarted()
        {
            if (Random.Range(0, 2) == 0)
                currentGuest.GetComponent<GuestAnimationManager>().PlaySitCombo1();
            else
                currentGuest.GetComponent<GuestAnimationManager>().PlaySitCombo2();
        }

        public void Step1Respnse(bool responseType)
        {
            if (responseType)
            {
            //    Allow();
            }
        }

        public void Step2Respnse(bool responseType)
        {
            if (responseType)
            {
            //    Allow();
            }
        }

        public void Step3Respnse(bool responseType)
        {
            if (responseType)
            {
                Allow();
            }
            else
            {
                currentGuest.GetComponent<Animator>().SetTrigger("scared");
              

                if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.isGuilty)
                {
                    _mPlayPhasesControl.correctAnswers++;
                    Progress.Instance.IncreamentRating(3);


                }
                else
                {
                    Progress.Instance.DecreamentRating(3);
                }

                if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.punishmentType != Punishment.None)
                {
                    currentGuest.GetComponent<Animator>().SetTrigger("Idle");
                    Timer.Delay(0.5f, () =>
                    {
                        Punish();
                    });
                }
                else
                {
                    Timer.Delay(1.5f, () =>
                    {
                        currentGuest.GetComponent<GuestAnimationManager>().PlaySayNo2();
                        Arrest();
                    });
                }
            }
        }

        void Arrest()
        {
            audioManager.PlayAudio(aud_arrest);
            Progress.Instance.WasBadDecision = true;

            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.isGuilty)
            {
                _mPlayPhasesControl.correctAnswers++;
                Progress.Instance.IncreamentRating(3);
            }
            else
            {
                Progress.Instance.DecreamentRating(3);
            }

            copHandler.ReturnSelectedAvatar().GetComponent<Animator>().SetTrigger("catch");

            LerpObjectPosition2.instance.LerpObject(copHandler.ReturnSelectedAvatar().transform, copInPos.position, 0.5f, () =>
            {
                Debug.Log("cop going in");

                Timer.Delay(0.5f, () =>
                {
                    ArrestOut();
                });
            });
        }

        void Punish()
        {
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.punishmentType == Punishment.Taser)
            {
                audioManager.PlayAudio(aud_tase);
                currentGuest.GetComponent<Animator>().applyRootMotion = true;
                currentGuest.GetComponent<Animator>().Play("Taser");
                currentGuest.GetComponent<Animator>().SetFloat("electricity_amt", 6);
                currentGuest.GetComponent<PunishmentParticles>().ActivateShock();

                Timer.Delay(1.5f, () =>
                {
                    currentGuest.GetComponent<Animator>().Play("Scared1");
                    currentGuest.GetComponent<PunishmentParticles>().DeActivateShock();
                    Arrest();
                });
            }else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.punishmentType == Punishment.Slap)
            {

                audioManager.PlayAudio(aud_slap);
                currentGuest.GetComponent<Animator>().applyRootMotion = true;
                currentGuest.GetComponent<Animator>().Play("GettingSlapped");
                currentGuest.GetComponent<PunishmentParticles>().SlapParticles();

                Timer.Delay(0.5f, () =>
                {
                    currentGuest.GetComponent<Animator>().Play("Scared1");
                    Arrest();
                });
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.punishmentType == Punishment.HammerHit)
            {
                hammer.SetActive(true);
                audioManager.PlayAudio(aud_hammerHit);
                currentGuest.GetComponent<Animator>().applyRootMotion = true;
                currentGuest.GetComponent<Animator>().Play("HammerHit");
                currentGuest.GetComponent<PunishmentParticles>().SlapParticles();

                Timer.Delay(3f, () =>
                {
                    hammer.SetActive(false);
                    _mPlayPhasesControl._OnPhaseFinished();
                });
            }else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.punishmentType == Punishment.LowBlow)
            {

                audioManager.PlayAudio(aud_lowblow);
                currentGuest.GetComponent<Animator>().applyRootMotion = true;
                currentGuest.GetComponent<Animator>().Play("LowBlow");
                currentGuest.GetComponent<PunishmentParticles>().LowBlowParticles();

                Timer.Delay(3f, () =>
                {
                    _mPlayPhasesControl._OnPhaseFinished();
                });
            }else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.punishmentType == Punishment.SpiderBucket)
            {

                audioManager.PlayAudio(aud_spiderBucket);
                currentGuest.GetComponent<Animator>().applyRootMotion = true;
                currentGuest.GetComponent<Animator>().Play("SpiderBucket");
                currentGuest.GetComponent<PunishmentParticles>().SpiderBucket();

                Timer.Delay(3f, () =>
                {
                    currentGuest.GetComponent<Animator>().Play("Scared1");
                    Arrest();
                });
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.punishmentType == Punishment.Spit)
            {
                spit.SetActive(true);
                audioManager.PlayAudio(aud_spit);
                currentGuest.GetComponent<Animator>().applyRootMotion = true;
                currentGuest.GetComponent<Animator>().Play("Spit");

                Timer.Delay(2, () =>
                {
                    spit.SetActive(false);
                    currentGuest.GetComponent<Animator>().Play("Scared1");
                    Arrest();
                });
            }


        }

        void ArrestOut()
        {
            LerpObjectPosition2.instance.LerpObject(copHandler.ReturnSelectedAvatar().transform, copOutPos.position, 0.5f, () =>
            {
                Debug.Log("cop going out");
            });

            LerpObjectPosition.instance.LerpObject(currentGuest.transform, guestOutPos.position, 0.5f, () =>
            {
                Debug.Log("guest going out");

                Timer.Delay(0.5f, () =>
                {
                    _mPlayPhasesControl._OnPhaseFinished();
                });

            });
        }

        void Allow()
        {

            Debug.Log("allowed innocent person " + _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.isGuilty);

            audioManager.PlayAudio(aud_release);

            if (!_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetInterrogationInfo.isGuilty)
            {
                _mPlayPhasesControl.correctAnswers++;
                Progress.Instance.IncreamentRating(3);
            }
            else
            {
                Progress.Instance.DecreamentRating(3);
             //   Progress.Instance.WasBadDecision = true;
            }

            currentGuest.GetComponent<GuestAnimationManager>().PlayRandomHappyAnim();

            Timer.Delay(2, () =>
            {
                _mPlayPhasesControl._OnPhaseFinished();
            });

        }
    }
}
