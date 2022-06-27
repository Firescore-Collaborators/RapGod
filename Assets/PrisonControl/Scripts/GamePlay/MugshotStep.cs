using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace PrisonControl
{
    public class MugshotStep : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private GamePlayStep _mGamePlayStep;

        [SerializeField]
        public MugshotUi mugshotUi;

        [SerializeField]
        private Transform standPos, prisonerHolder;

        [SerializeField]
        private int prisoner_index;

        private GameObject currentPrisoner;

        [SerializeField]
        private RenderTexture renderTexture;

        [SerializeField]
        private SnapshotTaker snapshotTaker;

        [SerializeField]
        private PaintIn3D.P3dPaintableTexture signaturePad;

        [SerializeField]
        private GameObject tutorialLoop;
        bool isRegisterPage;

        bool pos_animation_toggle, neg_animation_toggle;

        void Awake()
        {
            InputManager.MouseDragStarted += OnClick;

            mugshotUi.responseClicked += ResponseClicked;
            mugshotUi.photoCaptureInitiated += PhotoCapturedInitiated;
            mugshotUi.OnCloseSnapShot += OnCloseSnapShot;
            mugshotUi.OnCloseRegister += OnCloseRegister;

            snapshotTaker.PhotoCaptured += PhotoCaptured;
        }

        private void OnDestroy()
        {
            InputManager.MouseDragStarted += OnClick;

            mugshotUi.responseClicked -= ResponseClicked;
            mugshotUi.photoCaptureInitiated -= PhotoCapturedInitiated;
            mugshotUi.OnCloseSnapShot -= PhotoCapturedInitiated;
            mugshotUi.OnCloseRegister -= OnCloseRegister;

            snapshotTaker.PhotoCaptured -= PhotoCaptured;
        }
        private void OnEnable()
        {
            isRegisterPage = false;
            signaturePad.Clear();

            prisoner_index = 0;
            // Steps
            NextPrisoner();
        }

        void NextPrisoner()
        {
            StartCoroutine(Step());
        }

        IEnumerator Step()
        {
            //Spawn Prisoner
            prisoner_index++;
            yield return new WaitForSeconds(1);

            mugshotUi.Setup(_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetMugShotInfo, prisoner_index - 1);

            if (currentPrisoner)
                Destroy(currentPrisoner);

            currentPrisoner = Instantiate(_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetMugShotInfo.prisoners[prisoner_index - 1].pf_character, prisonerHolder);

            currentPrisoner.GetComponent<Animator>().Play("WalkIn");
            currentPrisoner.GetComponent<PrisonerMugShot>()._movedIn += MovedIn;
            currentPrisoner.GetComponent<PrisonerMugShot>()._movedOut += MovedOut;
        }

        void MovedIn()
        {
            mugshotUi.ShowUi();
        }

        void ResponseClicked(bool responseType)
        {
            if (responseType)
            {
                int nextAnim = 0; ;
                pos_animation_toggle = !pos_animation_toggle;

                if (pos_animation_toggle)
                {
                    nextAnim = (((prisoner_index - 1) * 2) + 0);
                }
                else
                {
                    nextAnim = (((prisoner_index - 1) * 2) + 1);
                }

                string animName = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetMugShotInfo.positiveAnim[nextAnim].ToString();
                currentPrisoner.GetComponent<Animator>().Play(animName);
            }
            else
            {
                int nextAnim = 0; ;
                neg_animation_toggle = !neg_animation_toggle;

                if (neg_animation_toggle)
                {
                    nextAnim = (((prisoner_index - 1) * 2) + 0);
                }
                else
                {
                    nextAnim = (((prisoner_index - 1) * 2) + 1);
                }

                string animName = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetMugShotInfo.negetiveAnim[nextAnim].ToString();
                currentPrisoner.GetComponent<Animator>().Play(animName);
            }
        }

        void MovedOut()
        {
            currentPrisoner.GetComponent<PrisonerMugShot>()._movedIn -= MovedIn;
            currentPrisoner.GetComponent<PrisonerMugShot>()._movedOut -= MovedOut;

            Destroy(currentPrisoner);
        }

        void PhotoCapturedInitiated()
        {
            snapshotTaker.TakeSnapShot();
            currentPrisoner.GetComponent<Animator>().SetTrigger("walkOut");
        }

        void PhotoCaptured(byte[] bytes)
        {
            mugshotUi.ShowSnapShot(prisoner_index, bytes);
            currentPrisoner.GetComponent<Animator>().Play("Idle");
        }

        void OnCloseSnapShot()
        {
            if (prisoner_index >= _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetMugShotInfo.totalPrisnors)
            {
                Timer.Delay(0.5f, () =>
                {
                    isRegisterPage = true;
                    tutorialLoop.SetActive(true);
                    mugshotUi.ShowRegisterPage(_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetMugShotInfo);
                });
            }
            else
            {
                NextPrisoner();
                Timer.Delay(0f, () =>
                {

                });
            }
        }

        void OnCloseRegister()
        {
            EndStep();
        }

        void EndStep()
        {
            _mPlayPhasesControl._OnPhaseFinished();
        }

        void OnClick(Vector2 pos)
        {
            if (!isRegisterPage)
                return;

            tutorialLoop.SetActive(false);
        }

        private void Update()
        {
#if !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    OnClick(Vector2.one);
                }
                //else
                //if (touch.phase == TouchPhase.Moved)
                //{
                //    OnDrag(Vector2.one);
                //}
                //else
                //if (touch.phase == TouchPhase.Ended)
                //{
                //    OnClickEnd(Vector2.one);
                //}
            }
#endif

        }

    }
}
