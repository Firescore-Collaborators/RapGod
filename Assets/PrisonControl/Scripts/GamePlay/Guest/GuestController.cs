using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class GuestController : MonoBehaviour
    {
        public GamePlayStep gamePlayStep;
        public int GuestID;
        public float lerpSpeed;
        public float lerpPassSpeed;
        public float rotLerpSpeed;

        [SerializeField]
        private SkinnedMeshRenderer skinnedMeshRenderer;

        public GuestMood guestMood;

        public bool isCelebrity;

        public bool isFemale;

        public enum GuestMood
        {
            Happy,
            Normal,
            Angry
        }

        public void StepHead(System.Action callback)
        {
            GetComponent<Animator>().Play("Walk");

            LerpObjectPosition.instance.LerpObject(transform, gamePlayStep.guestStandPos.position, lerpSpeed, () =>
            {

                if (isCelebrity)
                {
                    GetComponent<Animator>().SetTrigger("shoot");
                }
                else
                {
                    GetComponent<Animator>().SetTrigger("Idle");
                }
                callback.Invoke();
                //   LuncBoxManager.instance.PlayerReached();
            }
            );
        }

        public void StepOut(System.Action callback)
        {
            Debug.Log("step out");
            GetComponent<Animator>().SetTrigger("nod");
            GetComponent<Animator>().Play("Smile", 2);
            GetComponent<Animator>().Play("WalkOut");

            Timer.Delay(1.5f, () => {

                LerpObjectRotation.instance.LerpObject(transform, Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y - 90.0f, 0)), rotLerpSpeed, () => {
                    LerpPassPosition(callback);
                });
            });
        }

        public void ArrestOut(System.Action callback)
        {
            Debug.Log("arrest out");

            // GetComponent<Animator>().Play("Crying");

            //Timer.Delay(0, () =>
            //{
            LerpObjectPosition.instance.LerpObject(transform, gamePlayStep.guestLeavePos.position, 0.5f, () => {

                    Debug.Log("arresting char out2");
                    callback.Invoke();
                });
            //});
        }

        public void ScaredReaction()
        {
            Debug.Log("Scaredddddd");
            GetComponent<Animator>().SetTrigger("scared");
        }

        public void SuspiciousReaction() {
            SuspiciousAnimations();
        }
        public void SuspiciousReactionItemCheck()
        {
            SuspiciousAnimationsItemCheck();
        }
        public void AngryReaction()
        {
            GetComponent<Animator>().SetTrigger("angry");

            guestMood = GuestMood.Angry;
        }

        void LerpPassPosition(System.Action callback)
        {
            if(guestMood == GuestMood.Angry)
                GetComponent<Animator>().Play("AngryWalk");
            else
                GetComponent<Animator>().Play("walk");

            LerpObjectPosition.instance.LerpObject(transform, gamePlayStep.guestLeavePos.position, lerpPassSpeed, () => {

                Debug.Log("arresting char out2");
                callback.Invoke();
            });
        }

        void SuspiciousAnimations() {

            if(isFemale)
                GetComponent<GuestAnimationManager>().PlayIDCardFemaleCombo();
            else
                GetComponent<GuestAnimationManager>().PlayIDCardMaleCombo();

        }
        void SuspiciousAnimationsItemCheck()
        {
            GetComponent<GuestAnimationManager>().PlayItemCheckCombo();
        }
    }
}
