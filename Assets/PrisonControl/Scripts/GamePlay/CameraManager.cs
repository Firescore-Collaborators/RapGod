using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace PrisonControl
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject camAvatarSelection, camAvatar1, camAvatar2;

        [SerializeField]
        private GameObject hubCam, defaultCam, camWardenSnapshot, camTinderChat, camIDCheckIn, camItemCheck1, camItemCheck2, camBribeStep, camInterrogation, camJailEntry1, camJailEntry2, camJailEntry3,
            camJailEntry_metalDetector, camMugShot, camFoodPlating1, camFoodPlating2, camPrisonYardEnter, camPrisonYard, camPrisonYard1, camPrisonYard2, camPrisonYardPunishment, camCCTV1, camCCTV2,
            camCellCheckStart;

        [SerializeField]
        private GameObject [] camWarden;

        public CinemachineBrain cinemachineBrainMain, cinemachineBrainCCTV;

        private float defaultPitch;
        private float defaultYaw;

        public void SetBlendVal(float _value)
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = _value;
        }

        private void Awake()
        {
            defaultPitch = camPrisonYard.GetComponent<Lean.Common.LeanPitchYaw>().Pitch;
            defaultYaw = camPrisonYard.GetComponent<Lean.Common.LeanPitchYaw>().Yaw;

        }

        public void ActivateCamHub()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 0f;
            DeactivateAllCam();
            hubCam.SetActive(true);
        }

        public void ActivateDefaultCam()
        {
            Debug.Log("----- default cam");
            cinemachineBrainMain.m_DefaultBlend.m_Time = 0;
            DeactivateAllCam();
            defaultCam.SetActive(true);
        }

        public void ActivateWardenCam(int index)
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 0;
            DeactivateAllCam();
            camWarden[index].SetActive(true);
        }

        public void ActivateWardenSnapshotCam()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 1;
            DeactivateAllCam();
            camWardenSnapshot.SetActive(true);
        }

        public void ActivateTinderChatCam()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 1;
            DeactivateAllCam();
            camTinderChat.SetActive(true);
        }

        public void ActivateCamAvatarSelection()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 0;
            DeactivateAllCam();
            camAvatarSelection.SetActive(true);
        }

        public void ActivateCamAvatar1()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = .5f;

            DeactivateAllCam();
            camAvatar1.SetActive(true);
        }

        public void ActivateCamAvatar2()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = .5f;
            DeactivateAllCam();
            camAvatar2.SetActive(true);
        }

        public void ActivateCamIDCheckIn()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 1f;

            DeactivateAllCam();
            camIDCheckIn.SetActive(true);
        }

        public void ActivateCamItemCheck1()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 1f;
            DeactivateAllCam();
            camItemCheck1.SetActive(true);
        }

        public void ActivateCamItemCheck2()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 1f;
            DeactivateAllCam();
            camItemCheck2.SetActive(true);
        }

        public void ActivateCamBribe()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 1f;
            DeactivateAllCam();
            camBribeStep.SetActive(true);
        }

        public void ActivateCamInterrogation()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 0f;
            DeactivateAllCam();
            camInterrogation.SetActive(true);
        }

        public void ActivateCamJailEntry1()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 0f;
            DeactivateAllCam();
            camJailEntry1.SetActive(true);
        }

        public void ActivateCamJailEntry2()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 1f;
            DeactivateAllCam();
            camJailEntry2.SetActive(true);
        }

        public void ActivateCamJailEntry3()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 1f;
            DeactivateAllCam();
            camJailEntry3.SetActive(true);
        }

        public void ActivateCamMetalDetector()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 1f;
            DeactivateAllCam();
            camJailEntry_metalDetector.SetActive(true);
        }
        public void ActivateCamMugshot()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 0f;
            DeactivateAllCam();
            camMugShot.SetActive(true);
        }

        public void ActivateCamFoodPlating1(float blendVal)
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = blendVal;
            DeactivateAllCam();
            camFoodPlating1.SetActive(true);
        }

        public void ActivateCamFoodPlating2()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = .5f;
            DeactivateAllCam();
            camFoodPlating2.SetActive(true);
        }

        public void ActivateCamPrisonYardEnter()
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = 0f;
            DeactivateAllCam();
            camPrisonYardEnter.SetActive(true);
        }

        public void ActivateCamPrisonYard(float blendVal)
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = blendVal;
            DeactivateAllCam();

            camPrisonYard.GetComponent<Lean.Common.LeanPitchYaw>().SetPitch(defaultPitch);
            camPrisonYard.GetComponent<Lean.Common.LeanPitchYaw>().SetYaw(defaultYaw);

            camPrisonYard.SetActive(true);
            camPrisonYard1.SetActive(true);
        }

        public void DeActivateCamPrisonYard()
        {
            camPrisonYard.SetActive(false);
            camPrisonYard1.SetActive(false);
            DeactivateAllCam();

            Debug.Log("deactivate the yard cam");
        }

        
        public void ActivateCam1PrisonYard(float blendVal)
        {
            //if (camPrisonYard1.activeSelf)
            //    return;

            cinemachineBrainMain.m_DefaultBlend.m_Time = blendVal;
            camPrisonYardPunishment.SetActive(false);
            camPrisonYard2.SetActive(false);
            camPrisonYard1.SetActive(true);
            camPrisonYard.SetActive(true);
        }

        public void ActivateCam2PrisonYard(float blendVal)
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = blendVal;
            camPrisonYard2.SetActive(true);
            camPrisonYard1.SetActive(false);
        }

        public void ActivateCamPrisonYardPunishment(float blendVal)
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = blendVal;
            DeactivateAllCam();
            camPrisonYardPunishment.SetActive(true);
        }

        public void ActivateCamCCTV1(float blendVal)
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = blendVal;
            DeactivateAllCam();
            camCCTV1.SetActive(true);
        }

        public void ActivateCamCCTV2(float blendVal)
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = blendVal;
            DeactivateAllCam();
            camCCTV2.SetActive(true);
        }

        public void ActivateCamCellCheck(float blendVal)
        {
            cinemachineBrainMain.m_DefaultBlend.m_Time = blendVal;
            DeactivateAllCam();
            camCellCheckStart.SetActive(true);
        }

        public void DeactivateAllCam()
        {
            Debug.Log("deactivate all cam");

            defaultCam.SetActive(false);
            camAvatarSelection.SetActive(false);
            camAvatar1.SetActive(false);
            camAvatar2.SetActive(false);

            camIDCheckIn.SetActive(false);
            camItemCheck1.SetActive(false);
            camItemCheck2.SetActive(false);
            camBribeStep.SetActive(false);
            camInterrogation.SetActive(false);
            hubCam.SetActive(false);

            for (int i = 0; i < camWarden.Length; i++)
            {
                camWarden[i].SetActive(false);
            }
            camWardenSnapshot.SetActive(false);

            camJailEntry1.SetActive(false);
            camJailEntry2.SetActive(false);
            camJailEntry3.SetActive(false);
            camJailEntry_metalDetector.SetActive(false);

            camMugShot.SetActive(false);
            camFoodPlating1.SetActive(false);
            camFoodPlating2.SetActive(false);

            camPrisonYard.SetActive(false);
            camPrisonYard1.SetActive(false);
            camPrisonYard2.SetActive(false);
            camPrisonYardPunishment.SetActive(false);
            camCCTV1.SetActive(false);
            camCCTV2.SetActive(false);
            camTinderChat.SetActive(false);

            camPrisonYardEnter.SetActive(false);
        }
    }
}
