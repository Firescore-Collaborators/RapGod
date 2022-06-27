using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class CopManager : MonoBehaviour
    {
        public CopHandler cop;

        private Animator animator;

        [SerializeField]
        GamePlayStep gamePlayStep;

        [SerializeField]
        PlayPhasesControl _mPlayPhasesControl;

        private void Awake()
        {
            animator = cop.avatar[Progress.Instance.AvatarGender].transform.GetComponentInChildren<Animator>();
        }
        public void Catch()
        {
            animator.SetTrigger("catch");

            LerpObjectPosition2.instance.LerpObject(cop.transform, gamePlayStep.copStandPos.position, 0.5f, () =>
            {
                Debug.Log("lerping cop");
                //  callback.Invoke();

                Timer.Delay(0.5f, () =>
                {
                    ArrestOut();
                });
            });
        }

        public void ArrestOut()
        {
            StartCoroutine(Arrest());
        }

        IEnumerator Arrest()
        {
            Debug.Log("Arrest");

            gamePlayStep.ArrestGuest();
            yield return new WaitForSeconds(0);

            LerpObjectPosition2.instance.LerpObject(cop.transform, gamePlayStep.copLeavePos.position, 0.5f, () =>
            {
                //  callback.Invoke();
            });
            yield return new WaitForSeconds(0f);

         //   animator.SetTrigger("moveOut");
        }

    }
}
