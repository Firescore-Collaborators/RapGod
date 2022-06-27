using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

namespace PrisonControl
{
    public class BribeBoxManager : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;

        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private GamePlayStep _mGamePlayStep;

        [SerializeField]
        CameraManager cameraManager;

        [SerializeField]
        private GameObject cloudParticle, popUp, checkPanel;

        [SerializeField]
        private TypewriterEffect popupText;

        private GameObject bribeBox;

        // ---- UI ----
        [SerializeField]
        private Button btnReject, btnAllow;

        [SerializeField]
        private GameObject tutorial;

        RaycastHit hit;

        private int totalBundles, bundleCntr;

        private bool isReadyToBribe;

        [SerializeField]
        RespondMessage respondMsg;

        [SerializeField]
        private AudioManager audioManager;

        [SerializeField]
        private HapticsManager hapticsManager;

        void OnEnable()
        {
            isReadyToBribe = false;

            if (bribeBox)
                Destroy(bribeBox);

            bundleCntr = 0;

            StartCoroutine(Steps());
        }

        IEnumerator Steps()
        {
            totalBundles = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetBribeBoxInfo[_mPlayPhasesControl.bribeBox_cursr - 1].ReturnTotalBundles();
            yield return new WaitForSeconds(.5f);

            cloudParticle.SetActive(true);

            bribeBox = Instantiate(_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetBribeBoxInfo[_mPlayPhasesControl.bribeBox_cursr - 1].ReturnSuitcasePrefab(), transform);

            yield return new WaitForSeconds(.65f);

            bribeBox.GetComponent<Animator>().Play("BoxOpen");

            yield return new WaitForSeconds(1);

            ShowPopUp();
        }

        void ShowPopUp()
        {
            GetComponent<AudioSource>().clip = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetBribeBoxInfo[_mPlayPhasesControl.bribeBox_cursr - 1].aud_intro;
            GetComponent<AudioSource>().Play();

            popUp.SetActive(true);
            popupText.WholeText = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetBribeBoxInfo[_mPlayPhasesControl.bribeBox_cursr - 1].ReturnMessage();
            popupText.ShowTextResponse(() =>
            {
                StartCoroutine(OnPopupDone());
            });
        }

        IEnumerator OnPopupDone()
        {
            yield return new WaitForSeconds(0);
            checkPanel.SetActive(true);
        }

        public void AcceptBribe()
        {
            Progress.Instance.DecreamentRating(3);

            _mPlayPhasesControl.correctAnswers++;

            popUp.SetActive(false);
            cameraManager.ActivateCamBribe();
            checkPanel.SetActive(false);

            Timer.Delay(1, () =>
            {
                tutorial.SetActive(true);
                isReadyToBribe = true;
            });

            Progress.Instance.WasBadDecision = true;

            //bribeBox.GetComponent<Animator>().Play("BoxClose");

            //Timer.Delay(0.5f, () =>
            //{
            //    HideBox();
            //    _mPlayPhasesControl._OnMiniLevelFinished();
            //});

            //    checkPanel.SetActive(false);

        }

        public void RejectBribe()
        {
            Progress.Instance.IncreamentRating(3);

            bribeBox.GetComponent<Animator>().Play("BoxClose");
            popUp.SetActive(false);
            checkPanel.SetActive(false);

            _mGamePlayStep.OnBrideRejected();

            respondMsg.ShowWrongMsg();

            Timer.Delay(1.5f, () =>
            {
                HideBox();
                _mPlayPhasesControl._OnMiniLevelFinished();
            });
        }

        public void HideBox()
        {
            popUp.SetActive(false);
            Destroy(bribeBox);
            checkPanel.SetActive(false);
        }

        private void Update()
        {
            if (!isReadyToBribe)
                return;

#if UNITY_EDITOR

            if (Input.GetMouseButton(0))
            {
                tutorial.SetActive(false);
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "cash")
                    {
                        hit.collider.GetComponent<Animator>().enabled = true;
                        hit.collider.GetComponent<CashBundle>().ActivateParticleEffect();
                        hit.collider.gameObject.SetActive(false);

                        bundleCntr++;

                        if (bundleCntr >= totalBundles)
                        {
                            StartCoroutine(OnCashCollected());
                        }

                    }

                }
            }
#else
         if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    tutorial.SetActive(false);
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "cash")
                        {
                            Debug.Log("pick " + hit.collider.name);
                            hit.collider.GetComponent<Animator>().enabled = true;
                            hit.collider.GetComponent<CashBundle>().ActivateParticleEffect();
                            hit.collider.gameObject.SetActive(false);

                            bundleCntr++;

                            if (bundleCntr >= totalBundles)
                            {
                                StartCoroutine(OnCashCollected());
                            }

                            hapticsManager.PlayLightImpact();
                        }

                    }
                }
            }
#endif
        }

        IEnumerator OnCashCollected()
        {
            bribeBox.GetComponent<Animator>().Play("BoxClose");

            cameraManager.ActivateCamItemCheck2();

            yield return new WaitForSeconds(0.5f);
            respondMsg.ShowCorrectMsg();

            yield return new WaitForSeconds(1f);

            HideBox();

           // respondMsg.SetActive(true);

            _mPlayPhasesControl._OnMiniLevelFinished();
           
        }
    }
}
