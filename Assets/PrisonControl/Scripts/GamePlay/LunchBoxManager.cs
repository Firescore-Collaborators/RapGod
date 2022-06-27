using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Lean.Touch;
using TMPro;

namespace PrisonControl
{
    public class LunchBoxManager : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private GamePlayStep _mGamePlayStep;

        [SerializeField]
        CameraManager cameraManager;

        [SerializeField]
        private GameObject cloudParticle, popUp, checkPanel, bannedObjectText;

        //[SerializeField]
        //private TextMeshPro popUpText;

        [SerializeField]
        private TypewriterEffect popupText;

        private GameObject lunchBox;

        Ray ray;
        RaycastHit hit;

        [SerializeField]
        private LayerMask layer;

        [Foldout("ScriptReferences")]
        private LeanDragTranslate currentDragObject;

        [SerializeField]
        public Transform finalKnifePos;

        [SerializeField]
        public float lerpSpeed;

        [SerializeField]
        private CopManager copManager;

        // ---- UI ----
        [SerializeField]
        private Button btnArrest, btnAllow;

        private bool isCaught;

        private GameObject tutorialObj;

        [SerializeField]
        RespondMessage respondMsg;

        [SerializeField]
        private AudioManager audioManager;

        [Foldout("Audio Clips")]
        public AudioClip aud_foundObject, aud_arrest;

        void OnEnable()
        {
            AssignData();
            StartCoroutine(Steps());

            isCaught = false;

            btnArrest.GetComponent<Button>().interactable = false;
        }

        IEnumerator Steps()
        {
            yield return new WaitForSeconds(.5f);
            ShowPopUp();
        }

        void ShowPopUp()
        {
            GetComponent<AudioSource>().clip = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLunchBoxInfo[_mPlayPhasesControl.lunchBox_cursr - 1].aud_intro;
            GetComponent<AudioSource>().Play();

            popUp.SetActive(true);
            popupText.WholeText = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLunchBoxInfo[_mPlayPhasesControl.lunchBox_cursr - 1].ReturnMessage();
            popupText.ShowTextResponse(() =>
            {
                StartCoroutine(OnPopupDone());
            });
        }

        IEnumerator OnPopupDone()
        {
            cloudParticle.SetActive(true);

            lunchBox = Instantiate(_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLunchBoxInfo[_mPlayPhasesControl.lunchBox_cursr - 1].ReturnLunchBoxPrefab(), transform);

            yield return new WaitForSeconds(.65f);

            lunchBox.GetComponent<Animator>().Play("BoxOpen");

            cameraManager.ActivateCamItemCheck1();

            yield return new WaitForSeconds(1);
            checkPanel.SetActive(true);

            if (!Progress.Instance.IsLunchBoxStepTutDone && _mPlayPhasesControl.CurrentMiniLevel == 0)
            {
                tutorialObj = lunchBox.transform.GetChild(0).gameObject;
                tutorialObj.SetActive(true);

            }

            popUp.SetActive(false);

        }

        private void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

#if UNITY_EDITOR

            if (Input.GetMouseButtonUp(0))
            {
                if (currentDragObject != null)
                {
                    SetDefault();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("---- mouse click");

                if (!Progress.Instance.IsLunchBoxStepTutDone && _mPlayPhasesControl.CurrentMiniLevel == 0)
                {
                    tutorialObj.SetActive(false);
                }
                if (Physics.Raycast(ray, out hit, 100f, layer))
                {
                    if (hit.collider.TryGetComponent(out LeanDragTranslate leanDragTranslate))
                    {
                        currentDragObject = leanDragTranslate;
                        currentDragObject.enabled = true;
                    }

                    if (hit.collider.gameObject.CompareTag("fruits"))
                    {
                        Debug.Log("---- fruits selected");
                        return;
                    }

                    if (hit.collider.gameObject.CompareTag("Knife"))
                    {
                        Debug.Log("---- knife selected");
                        hit.collider.gameObject.layer = 10;
                        hit.collider.gameObject.GetComponent<Outline>().enabled = true;
                        KnifeFound(hit.collider.transform);
                    }
                }
            }
#else
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log("began");
                    if (!Progress.Instance.IsLunchBoxStepTutDone && _mPlayPhasesControl.CurrentMiniLevel == 0)
                    {
                        tutorialObj.SetActive(false);
                    }
                    if (Physics.Raycast(ray, out hit, 100f, layer))
                    {
                        if (hit.collider.TryGetComponent(out LeanDragTranslate leanDragTranslate))
                        {
                            currentDragObject = leanDragTranslate;
                            currentDragObject.enabled = true;
                        }

                        if (hit.collider.gameObject.CompareTag("fruits"))
                        {
                            Debug.Log("---- fruits selected");
                            return;
                        }
                        if (hit.collider.gameObject.CompareTag("Knife"))
                        {
                            Debug.Log("---- knife selected");
                            hit.collider.gameObject.layer = 10;

                            KnifeFound(hit.collider.transform);
                        }
                    }
                }
            }
#endif
        }

        void SetDefault()
        {
            if (currentDragObject != null)
            {
                currentDragObject.enabled = false;
                currentDragObject = null;
            }
        }

        void AssignData()
        {

        }

        public void KnifeFound(Transform knife) {
            Debug.Log("knife found");

            cameraManager.ActivateCamItemCheck2();
            _mGamePlayStep.OnSuspiciousItemCheck();

            audioManager.PlayAudio(aud_foundObject);

            Timer.Delay(1, () => {
                LerpObjectPosition.instance.LerpObject(knife, finalKnifePos.position, lerpSpeed, () =>
                {
                    if (knife.TryGetComponent(out ObjectRotate rotate))
                    {
                        Debug.Log("knife lerp complete");

                        bannedObjectText.SetActive(true);

                     //   knife.GetComponent<ObjectRotate>().enabled = true;

                        lunchBox.GetComponent<Animator>().Play("BoxClose");

                        isCaught = true;
                        Timer.Delay(0.5f, () =>
                        {
                            btnArrest.GetComponent<Button>().interactable = true;
                        });
                    }
                });
                LerpObjectRotation.instance.LerpObject(knife, finalKnifePos.rotation, lerpSpeed);
            });
        }

        public void Arrest()
        {
            lunchBox.GetComponent<Animator>().Play("BoxOut");

            if (!isCaught)
            {
                cameraManager.ActivateCamItemCheck2();

                Timer.Delay(1, () =>
                {
                    copManager.Catch();

                    bannedObjectText.SetActive(false);

                    Timer.Delay(2, () =>
                    {
                        _mPlayPhasesControl._OnMiniLevelFinished2();
                    });

                    HideBox();
                });
            }
            else
            {
                audioManager.PlayAudio(aud_arrest);
                cameraManager.ActivateCamItemCheck2();
                copManager.Catch();
                _mGamePlayStep.OnArrestStarted();

                if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLunchBoxInfo[_mPlayPhasesControl.lunchBox_cursr - 1].ReturnInnocentType() == LunchBox_SO.InnocentTypes.Guilty)
                {
                    _mPlayPhasesControl.correctAnswers++;
                    respondMsg.ShowCorrectMsg();
                    Progress.Instance.IncreamentRating(3);
                }
                else
                {
                    respondMsg.ShowWrongMsg();
                    Progress.Instance.DecreamentRating(3);
                }

                if (_mPlayPhasesControl.lunchBox_cursr == 3)
                    Progress.Instance.WasBadDecision = true;

                bannedObjectText.SetActive(false);

                Timer.Delay(2, () =>
                {
                    _mPlayPhasesControl._OnMiniLevelFinished2();
                });

                HideBox();
            }
        }

        public void Allow()
        {

            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLunchBoxInfo[_mPlayPhasesControl.lunchBox_cursr - 1].ReturnInnocentType() == LunchBox_SO.InnocentTypes.Innocent)
            {
                _mPlayPhasesControl.correctAnswers++;
                respondMsg.ShowCorrectMsg();
                Progress.Instance.IncreamentRating(3);
            }
            else
            {
                respondMsg.ShowWrongMsg();
                Progress.Instance.DecreamentRating(3);

                //if (_mPlayPhasesControl.lunchBox_cursr == 3)
                //    Progress.Instance.WasBadDecision = true;

            }

            cameraManager.ActivateCamItemCheck2();

            bannedObjectText.SetActive(false);
            _mPlayPhasesControl._OnMiniLevelFinished();
            HideBox();
        }

        public void HideBox()
        {
            lunchBox.GetComponent<Animator>().Play("BoxOut");

            Timer.Delay(.5f, () =>
            {
                Destroy(lunchBox);
            });
            
            checkPanel.SetActive(false);
        }
    }
}
