using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class WardenStep : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private GamePlayStep _mGamePlayStep;

        [SerializeField]
        public WardenUi wardenUi;

        [SerializeField]
        private CameraManager cameraManager;

        private Animator anim_warden, anim_prisoner;

        private GameObject currWarden, currPrisoner;

        [SerializeField]
        private GameObject pf_warden;

        [SerializeField]
        private GameObject pf_prisoner;

        [SerializeField]
        private GameObject wardenCashHolder;

        [SerializeField]
        private GameObject tool;

        [SerializeField]
        private Transform toolSpawnPos, toolUsePos;

        [SerializeField]
        private GameObject pf_lerpObject;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip aud_slap, aud_tase, aud_chickenDance, aud_lowBlow, aud_hammer, aud_spit, aud_spiderbucket;

        [SerializeField]
        private Material shock_material;

        [SerializeField]
        private WardenSnapshotTaker wardenSnapshotTaker;

        [SerializeField]
        private GameObject tinderChat, tinderBio;

        [SerializeField]
        private MessageController tinder_messageController;

        [SerializeField]
        private TinderBioController tinder_bioController;

        private Warden_SO warden_SO;

        private int lastDance, lastPose;

        [SerializeField]
        private Transform[] wardenPos;

        [SerializeField]
        private Transform cleaPos;

        [SerializeField]
        private GameObject hammer, spit;

        [SerializeField]
        private GameObject popup;

        [SerializeField]
        private Transform [] popups;

        private void Awake()
        {
            wardenUi.conversationStarted += ConversationStarted;

            wardenUi.step1Response += Step1Respnse;
            wardenUi.step2Response += Step2Respnse;
            wardenUi.step3Response += Step3Respnse;

            wardenUi.showWardenAnim += SetWardenAnim;

            wardenUi.onMugshotStarted += OnMugshotStarted;
            wardenUi.onTinderBioStarted += OnTinderBioStarted;
            wardenUi.onTinderChatStarted += OnTinderChatStarted;

            wardenUi.onMugshotInitiated += OnMugshotInitiated;
            wardenUi.onMugshotDone += OnMugshotDone;

            wardenUi.onPoseClicked += OnPoseClicked;
            wardenUi.onDanceClicked += onDanceClicked;

            

            wardenSnapshotTaker.PhotoCaptured += WardenPhotoCaptured;
            tinder_messageController.onConversationEnd += OnConversationEnd;
            tinder_bioController.onConversationEnd += OnConversationEnd;

            wardenUi.onWardenReward += OnWardenReward;


            wardenUi.spawnTool += SpawnTool;
            wardenUi.endLevel += EndStep;
        }

        private void OnDestroy()
        {
            wardenUi.conversationStarted -= ConversationStarted;
            wardenUi.step3Response -= Step3Respnse;
            wardenUi.showWardenAnim -= SetWardenAnim;

            wardenUi.onMugshotStarted -= OnMugshotStarted;
            wardenUi.onTinderBioStarted -= OnTinderBioStarted;
            wardenUi.onTinderChatStarted -= OnTinderChatStarted;

            wardenUi.onMugshotInitiated -= OnMugshotInitiated;
            wardenUi.onMugshotDone -= OnMugshotDone;

            wardenSnapshotTaker.PhotoCaptured -= WardenPhotoCaptured;
            wardenUi.onPoseClicked -= OnPoseClicked;
            wardenUi.onDanceClicked -= onDanceClicked;

            tinder_messageController.onConversationEnd -= OnConversationEnd;
            tinder_bioController.onConversationEnd += OnConversationEnd;

            wardenUi.spawnTool -= SpawnTool;
            wardenUi.endLevel -= EndStep;
        }

        void OnMugshotStarted()
        {
            cameraManager.ActivateWardenSnapshotCam();
        }
        void OnTinderBioStarted()
        {
            cameraManager.ActivateTinderChatCam();
            tinderBio.SetActive(true);
        }
        void OnTinderChatStarted()
        {
            //cam is same as tinder chat
            cameraManager.ActivateTinderChatCam();
            tinderChat.SetActive(true);
        }

        void OnConversationEnd()
        {
            EndStep(1);
        }

        void OnPoseClicked() {
            int randNo;
            do
            {
                randNo = Random.Range(1, 3);
            }
            while (lastPose == randNo);

            lastPose = randNo;
            anim_warden.Play("Pose"+ randNo);
        }

        void onDanceClicked()
        {
            int randNo;
            do
            {
                randNo = Random.Range(1, 8);
            }
            while (lastDance == randNo);

            lastDance = randNo;

            anim_warden.Play("Dance" + randNo);
        }
        void OnMugshotInitiated()
        {
            wardenSnapshotTaker.TakeSnapShot();
        }

        void OnMugshotDone()
        {
            anim_warden.Play("Idle");
            anim_warden.SetTrigger("Salute");

            EndStep(1);
        }

        void WardenPhotoCaptured(byte[] bytes)
        {
            wardenUi.ShowSnapShot(bytes);
        }

        void OnWardenReward()
        {
            Debug.Log("Step1Respnse");
            wardenCashHolder.SetActive(true);
        }
        void OnEnable()
        {
            warden_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetWardenInfo;
            cameraManager.ActivateWardenCam((int)warden_SO.wardenPos);

            popup.transform.position = popups[(int)warden_SO.wardenPos].position;
            popup.transform.rotation = popups[(int)warden_SO.wardenPos].rotation;


            currWarden = Instantiate(pf_warden, wardenPos[(int)warden_SO.wardenPos].transform);
            anim_warden = currWarden.GetComponent<Animator>();
            anim_warden.Play("" + warden_SO.positive_anim[0]);

            if (warden_SO.isFemalePrisoner)
            {
                currPrisoner = Instantiate(pf_prisoner, cleaPos);
                anim_prisoner = currPrisoner.GetComponent<Animator>();
                anim_prisoner.applyRootMotion = true;
                anim_prisoner.Play(warden_SO.prisoner_anim[0].ToString());
            }

            StartCoroutine(Setup());


        }

        IEnumerator Setup()
        {
            if (tool)
                Destroy(tool);

            wardenUi.Setup(warden_SO);

            yield return new WaitForSeconds(0.5f);

//            anim_warden.SetInteger("talking", Random.Range(1, 6));
        }

        void ConversationStarted()
        {
            //if (Random.Range(0, 2) == 0)
            //    currentGuest.GetComponent<GuestAnimationManager>().PlaySitCombo1();
            //else
            //    currentGuest.GetComponent<GuestAnimationManager>().PlaySitCombo2();
            SetWardenAnim(true);
        }


        public void Step1Respnse(bool responseType)
        {
            if (warden_SO.isFemalePrisoner)
                anim_prisoner.Play(warden_SO.prisoner_anim[1].ToString());
        }

        public void Step2Respnse(bool responseType)
        {
            if (warden_SO.isFemalePrisoner)
                anim_prisoner.Play(warden_SO.prisoner_anim[2].ToString());
        }

        public void Step3Respnse(bool responseType)
        {
            if (responseType)
            {
                EndStep(1);
                SetWardenAnim(responseType);

                Progress.Instance.WasBadDecision = false;
                Debug.Log("good decision");
            }
            else
            {
                Progress.Instance.WasBadDecision = true;
                Debug.Log("bad decision");

                if (warden_SO.punishmentType == Punishment.Taser)
                    Tase();
                else if (warden_SO.punishmentType == Punishment.Slap)
                    Slap();
                else if (warden_SO.punishmentType == Punishment.ChickenDance)
                    ChickenDance();
                else if (warden_SO.punishmentType == Punishment.LowBlow)
                    LowBlow();
                else if (warden_SO.punishmentType == Punishment.HammerHit)
                    HammerHit();
                else if (warden_SO.punishmentType == Punishment.SpiderBucket)
                    SpiderBucket();
                else if (warden_SO.punishmentType == Punishment.Spit)
                    Spit();
                else
                    EndStep(2);


            }
        }

        void SpawnTool()
        {
            if (warden_SO.isToolUnlock)
            {
                tool = Instantiate(warden_SO.tool, toolSpawnPos.position, Quaternion.identity);
            }
            else
            {

            }
        }

        void Tase()
        {
            GameObject lerpObjectPosition = Instantiate(pf_lerpObject);

            tool.GetComponent<Animator>().SetTrigger("use_tool");

            lerpObjectPosition.GetComponent<LerpObject>().Lerp(tool.transform, toolUsePos.position, 0.5f, () =>
            {
                DeactivateAnim();
                anim_warden.applyRootMotion = true;
                anim_warden.Play("Tase");
                shock_material.SetFloat("electricity_amt", 6);

                PlayAudio(aud_tase);

                if (warden_SO.isFemalePrisoner)
                {
                     anim_prisoner.Play("EletrocutedFull");
                   // anim_prisoner.Play(warden_SO.prisoner_anim[3].ToString());
                    anim_prisoner.GetComponent<PunishmentParticles>().ActivateShock();
                }

                Timer.Delay((1), () =>
                {
                    shock_material.SetFloat("electricity_amt", 0);

                    EndStep(1);
                });
            });
        }

        void Slap()
        {
            DeactivateAnim();
         //   anim_warden.SetTrigger("idle");
            anim_warden.Play("Slap");

            PlayAudio(aud_slap);
            if (warden_SO.isFemalePrisoner)
            {
                anim_prisoner.Play("Slap");
                // anim_prisoner.Play(warden_SO.prisoner_anim[3].ToString());
            }

            Timer.Delay(1, () =>
            {
                EndStep(1);
            });
        }

        void LowBlow()
        {
            DeactivateAnim();
            anim_warden.applyRootMotion = true;
            anim_warden.SetTrigger("idle");
            anim_warden.SetTrigger("LowBlow");

            PlayAudio(aud_lowBlow);
            anim_warden.gameObject.GetComponent<PunishmentParticles>().LowBlowParticles();
            Timer.Delay(2, () =>
            {
                EndStep(1);
            });
        }

        void HammerHit()
        {
            hammer.SetActive(true);
            DeactivateAnim();
            anim_warden.applyRootMotion = true;
            anim_warden.SetTrigger("idle");
            anim_warden.SetTrigger("HammerHit");

            PlayAudio(aud_hammer);
            Timer.Delay(2, () =>
            {
                hammer.SetActive(false);
                EndStep(1);
            });
        }

        void Spit()
        {
            spit.SetActive(true);
            DeactivateAnim();
            anim_warden.SetTrigger("idle");
            anim_warden.SetTrigger("Spit");

            PlayAudio(aud_spit);
            Timer.Delay(1, () =>
            {
                spit.SetActive(false);
                EndStep(1);
            });
        }

        void SpiderBucket()
        {
            DeactivateAnim();
            anim_warden.SetTrigger("idle");
            anim_warden.SetTrigger("SpiderBucket");

            PlayAudio(aud_spiderbucket);
            Timer.Delay(1, () =>
            {
                EndStep(1);
            });
        }

        void ChickenDance()
        {
            DeactivateAnim();
            anim_warden.Play("ChickenDance");

            PlayAudio(aud_chickenDance);

            Timer.Delay(3, () =>
            {
                EndStep(1);
            });
        }

        void EndStep(int extraDelay)
        {
            if(tool)
                Destroy(tool);

            Timer.Delay((1 + extraDelay), () =>
            {
                if (currPrisoner)
                    Destroy(currPrisoner);

                Destroy(currWarden);
                tinderBio.SetActive(false);
                tinderChat.SetActive(false);
                wardenCashHolder.SetActive(false);

                _mPlayPhasesControl._OnPhaseFinished();
            });
        }

        void SetWardenAnim(bool isPositive)
        {
            DeactivateAnim();
            if (isPositive)
            {
             //   anim_warden.SetTrigger(warden_SO.positive_anim[wardenUi.curr_conversation].ToString());

                anim_warden.Play("" + warden_SO.positive_anim[wardenUi.curr_conversation]);
                Debug.Log("play positive animation "+warden_SO.positive_anim[wardenUi.curr_conversation]);

                Debug.Log("wardenUi.curr_conversation "+ wardenUi.curr_conversation);
            }
            else
            {
                Debug.Log("wardenUi.curr_conversation "+ wardenUi.curr_conversation);
                Debug.Log("play negetive animation");
                anim_warden.Play(""+ warden_SO.negetive_anim[wardenUi.curr_conversation]);
                Timer.Delay(3f, () =>
                {
                    anim_warden.SetTrigger("idle");
                });
            }
           
        }

        //public void GetAnimationLength()
        //{
        //    AnimationClip[] clips = anim_warden.runtimeAnimatorController.animationClips;
        //    foreach (AnimationClip clip in clips)
        //    {
        //        switch (clip.name)
        //        {
        //            case :
        //                attackTime = clip.length;
        //                break;
        //            case "Damage":
        //                damageTime = clip.length;
        //                break;
        //            case "Dead":
        //                deathTime = clip.length;
        //                break;
        //            case "Idle":
        //                idleTime = clip.length;
        //                break;
        //        }
        //    }
        //}

        void DeactivateAnim()
        {
            //anim_warden.SetInteger("negetive", 0);
            //anim_warden.SetInteger("positive", 0);
        }

        void PlayAudio(AudioClip clip) {
            audioSource.clip = clip;
            audioSource.Play();
        }

    }
}
