using System.Collections;
using UnityEngine;
using MoreMountains.NiceVibrations;

namespace PrisonControl
{
    public class HapticsManager : MonoBehaviour
    {
        //private static HapticsManager mHapticsInstace;

        //public static HapticsManager Instance
        //{
        //    get
        //    {
        //        if (mHapticsInstace == null)
        //            mHapticsInstace = new HapticsManager();

        //        return mHapticsInstace;
        //    }
        //}

        public bool isVibrating;

        [SerializeField]
        private NiceVibrationsDemoManager vibration;

        public void VibrateStart()
        {
            StopCoroutine("Vibrate");
            StartCoroutine("Vibrate");
        }

        public void LightVibrateStart()
        {
            StopCoroutine("LightVibrate");
            StartCoroutine("LightVibrate");
        }

        public void SandingLightVibrateStart()
        {
            if (isVibrating)
                return;

            StartCoroutine("SandingLightVibrate");
        }

        public void VibrateStop()
        {
            isVibrating = false;
            StopCoroutine("Vibrate");
            StopCoroutine("LightVibrate");
            StopCoroutine("SandingLightVibrate");

        }
        IEnumerator Vibrate()
        {
            if (isVibrating)
            {
                PlayLightImpact();
                yield return new WaitForSeconds(0.075f);
                isVibrating = false;
            }
        }
        IEnumerator LightVibrate()
        {
            if (isVibrating)
            {
                PlayLightImpact();
                yield return new WaitForSeconds(0.1f);
                isVibrating = false;
            }
        }

        IEnumerator SandingLightVibrate()
        {
            PlayLightImpact();
            yield return new WaitForSeconds(0.1f);
            isVibrating = false;
        }
        public void PlayLightImpact()
        {
            vibration.TriggerLightImpact();
        }

        public void PlayMediumImpact()
        {
            vibration.TriggerMediumImpact();
        }

        public void PlayHardImpact()
        {
            Debug.Log("** hard impact");
            vibration.TriggerHeavyImpact();
        }
    }
}
