using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace PrisonControl
{
    [RequireComponent(typeof(GameFlow))]
    public class AvatarSelectionState : State
    {
        [SerializeField]
        AudioManager audioManager;

        [Foldout("Audio Clips")]
        public AudioClip aud_avatarClicked, aud_avatarSelected;

        private void Awake()
        {
            mGameFlow = GetComponent<GameFlow>();
            mAvatarUi.gameObject.SetActive(false);

            mAvatarUi._OnGenderSelectionDone += OnGenderSelectionDone;
            mAvatarUi._OnAvatarSelectionDone += OnAvatarSelectionDone;

            mAvatarUi.OnBackClicked += GoBack;
        }

        private void OnDestroy()
        {

            mAvatarUi._OnGenderSelectionDone -= OnGenderSelectionDone;
            mAvatarUi._OnAvatarSelectionDone -= OnAvatarSelectionDone;

            mAvatarUi.OnBackClicked -= GoBack;
        }

        public override void OnEnter()
        {
            step.SetActive(true);
            cameraManager.ActivateCamAvatarSelection();
            mAvatarUi.gameObject.SetActive(true);

            lightingManager.ActivateAvatarLights();
        }

        public override void OnExit()
        {
            mAvatarUi.gameObject.SetActive(false);

            lightingManager.ActivateGameLights();
            step.SetActive(false);
        }

        public override bool OnBackButtonPressed()
        {
            GoBack();
            return true;
        }

        private void GoBack()
        {
            mGameFlow._SwitchState(GameState.Hub);
        }

        private void OnGenderSelectionDone()
        {
            Debug.Log("Progress.Instance.AvatarGender "+ Progress.Instance.AvatarGender);

            audioManager.PlayAudio(aud_avatarSelected);

            if (Progress.Instance.AvatarGender == 0)
            {
                cameraManager.ActivateCamAvatar1();
            }
            //else if (Progress.Instance.AvatarGender == 1)
            else
            {
                cameraManager.ActivateCamAvatar2();
            }

            mAvatarUi.ActivateUniformSelection();

            avatarSelection.OnAvatarSelected(Progress.Instance.AvatarGender);
        }

        private void OnAvatarSelectionDone()
        {
            audioManager.PlayAudio(aud_avatarSelected);

            mGameFlow.fadeIn.SetActive(true);
            mGameFlow._SwitchState(GameState.Play);
        }

        public void OnAvatarClicked(int avatarGender)
        {
            Debug.Log("avatar clicked");

            audioManager.PlayAudio(aud_avatarClicked);

            mAvatarUi.ActivateGenderSelection();
            Progress.Instance.AvatarGender = avatarGender;
            avatarSelection.SelectAvatar(avatarGender);
        }

       

        private GameFlow mGameFlow;

        [SerializeField]
        AvatarSelectionUi mAvatarUi;

        [SerializeField]
        AvatarSelection avatarSelection;

        [SerializeField]
        CameraManager cameraManager;

        [SerializeField]
        private LightingManager lightingManager;

        [SerializeField]
        private GameObject step;
    }
}
