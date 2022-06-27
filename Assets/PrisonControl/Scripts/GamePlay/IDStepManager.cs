using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using UnityEngine.EventSystems;

namespace PrisonControl
{
    public class IDStepManager : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private GamePlayStep _mGamePlayStep;

        [SerializeField]
        private CopManager copManager;

        [SerializeField]
        private Transform IDCardParent, IDStartPos, IDFinalPos, IDCard;

        [SerializeField]
        private GameObject cloudParticle, popUp, checkPanel;

        [SerializeField]
        private GameObject pf_IDOverlay;

        private GameObject currentIdOverlay;
      
        public float lerpSpeed;

        [Foldout("TextMeshPro")]
        public TextMeshPro nameText;
        [Foldout("TextMeshPro")]
        public TextMeshPro ageText;
        [Foldout("TextMeshPro")]
        public TextMeshPro inmateRelationText;
        [Foldout("TextMeshPro")]
        public TextMeshPro numberText;

        //[Foldout("TextMeshPro")]
        //public TextMeshPro popupText;

        [SerializeField]
        private TypewriterEffect popupText;

        [SerializeField]
        private SpriteRenderer dpHolder;

        Ray ray;
        RaycastHit hit;

        Vector2 lastPos;

        [SerializeField]
        float heldTimer;

        float heldDelay;

        [SerializeField]
        private Image loader;


        // ID Card
        [SerializeField]
        private Transform stamp, stampRaycaster;

        [SerializeField]
        private bool readyForStamp;

        [SerializeField]
        private LayerMask layer;

        [SerializeField]
        Vector3 stampHitPosition;

        [SerializeField]
        Animator stampAnim;

        [SerializeField]
        GameObject paintBall;

        private Color loaderStartColor;

        // ----- UI -------
        [SerializeField]
        Button btnSuspicious, btnArrest, btnAllow;

        [SerializeField]
        GameObject tutorialHand1, tutorialHand2, tutorialMsg, dustParticles, tutorialLoop;

        [SerializeField]
        RespondMessage respondMsg;

        public Vector2 startPos;

        [SerializeField]
        private AudioManager audioManager;

        [Foldout("Audio Clips")]
        public AudioClip aud_idCardAppear, aud_arrest, aud_stamp;

        void OnEnable()
        {
            heldDelay = 0.75f;

            loaderStartColor = loader.color;
            readyForStamp = false;

            AssignData();
            StartCoroutine(Steps());

            InputManager.MouseDragStarted += OnClick;
            InputManager.MouseDragged += OnDrag;
            InputManager.MouseDragEnded += OnClickEnd;

            stamp.GetComponent<Lean.Common.LeanConstrainToColliders>().enabled = true;
            stamp.GetComponent<Lean.Touch.LeanDragTranslate>().enabled = true;

            stamp.gameObject.SetActive(false);

            heldTimer = 0;
            loader.fillAmount = heldTimer;

            btnArrest.interactable = false;
            btnArrest.gameObject.SetActive(false);
            btnSuspicious.gameObject.SetActive(true);

            btnAllow.interactable = true;
            btnSuspicious.interactable = true;

            if (!Progress.Instance.IsIdStepTutDone && _mPlayPhasesControl.CurrentMiniLevel == 0)
            {
                btnAllow.interactable = false;
            }
            else if (!Progress.Instance.IsIdStepTutDone && _mPlayPhasesControl.CurrentMiniLevel == 1)
            {
                btnSuspicious.interactable = false;
            }



            if (currentIdOverlay)
            {
                Destroy(currentIdOverlay);
            }
            currentIdOverlay = Instantiate(pf_IDOverlay, IDCardParent);

            tutorialHand1.SetActive(false);
            tutorialMsg.SetActive(false);
            tutorialLoop.SetActive(false);
        }

        void OnClick(Vector2 pos)
        {

        }

        private void Update()
        {
#if !UNITY_EDITOR
            if (!readyForStamp)
                return;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id)) {
                    Debug.Log("its uiiiiiii");
                    return;
                }



                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log("began");
                    tutorialLoop.SetActive(false);
                    startPos = touch.position;
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    Debug.Log("moved "+ Vector2.Distance(startPos, touch.position));

                    if (Vector2.Distance(startPos, touch.position) > 15)
                    {
                        heldTimer = 0;
                        loader.gameObject.SetActive(false);
                        loader.fillAmount = 0;
                        loader.color = loaderStartColor;
                    }

                    startPos = touch.position;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    Debug.Log("moving");
                    heldTimer = 0;
                    loader.gameObject.SetActive(false);
                    loader.fillAmount = 0;
                    loader.color = loaderStartColor;
                }
                if (touch.phase == TouchPhase.Stationary)
                {
                    Debug.Log("stationary");
                    loader.gameObject.SetActive(true);
                    heldTimer += Time.deltaTime;
                    loader.fillAmount = heldTimer * heldDelay;

                    Debug.DrawRay(stamp.transform.position, transform.TransformDirection(stamp.transform.forward) * -10, Color.green);

                    if (heldTimer >= heldDelay)
                    {
                        heldTimer = 0;
                        readyForStamp = false;
                        stamp.GetComponent<Lean.Common.LeanConstrainToColliders>().enabled = false;
                        stamp.GetComponent<Lean.Touch.LeanDragTranslate>().enabled = false;

                        //int layerMask = 1 << 10;
                        //layerMask = ~layerMask;

                        loader.color = Color.green;

                        RaycastHit hit;
                        if (Physics.Raycast(stampRaycaster.transform.position, transform.TransformDirection(stampRaycaster.transform.forward * -10), out hit, Mathf.Infinity, layer))
                        {
                            if (hit.collider.gameObject.CompareTag("column1"))
                            {
                                if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.photo)
                                {
                                    OnCatchWrongInfo();
                                }
                            }
                            else if (hit.collider.gameObject.CompareTag("column2"))
                            {
                                if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.name)
                                {
                                    OnCatchWrongInfo();


                                }
                            }
                            else
                            if (hit.collider.gameObject.CompareTag("column3"))
                            {
                                if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.relation)
                                {
                                    OnCatchWrongInfo();
                                    Debug.Log("Relation is wrong");
                                }
                            }

                            Debug.DrawRay(stampRaycaster.transform.position, transform.TransformDirection(stampRaycaster.transform.forward) * -10, Color.red);

                            stampHitPosition = IDCard.InverseTransformPoint(hit.point);


                            stampAnim.SetTrigger("stamp");

                            Timer.Delay(0.2f, () =>
                            {
                                audioManager.PlayAudio(aud_stamp);
                                Instantiate(dustParticles, hit.point, Quaternion.identity);
                            });

                            StartCoroutine(ActivatePaintBall());
                            //LerpObjectLocalPosition.instance.LerpLocalObject(stamp, stampHitPosition, 0.2f, () =>
                            //{
                            //    Debug.Log("Stamp hit Completed");
                            //    //IDlerpComplete.Raise();
                            //});

                            Debug.Log("collided " + hit.collider.name);
                        }
                        else
                        {
                            Debug.DrawRay(stampRaycaster.transform.position, transform.TransformDirection(stampRaycaster.transform.forward) * 1000, Color.white);
                            Debug.Log("Did not Hit");
                        }

                        loader.fillAmount = 0;
                        loader.color = loaderStartColor;

                        Vector3 forward = stamp.transform.TransformDirection(stamp.transform.forward) * -10;
                        Debug.DrawRay(stamp.transform.position, forward, Color.red);
                    }
                }
            }
#endif
        }

        void OnDrag(Vector2 pos)
        {

#if UNITY_EDITOR
            if (!readyForStamp)
                return;
            tutorialLoop.SetActive(false);

            if ((Vector2.Distance(lastPos,pos) >= 0 && Vector2.Distance(lastPos, pos) < 0.005f) || true)
            {
                loader.gameObject.SetActive(true);
                heldTimer += Time.deltaTime;
                loader.fillAmount = heldTimer / heldDelay;

                if (heldTimer >= heldDelay)
                {
                    heldTimer = 0;
                    readyForStamp = false;
                    stamp.GetComponent<Lean.Common.LeanConstrainToColliders>().enabled = false;
                    stamp.GetComponent<Lean.Touch.LeanDragTranslate>().enabled = false;

                    //int layerMask = 1 << 10;
                    //layerMask = ~layerMask;

                    loader.color = Color.green;

                    RaycastHit hit;
                    if (Physics.Raycast(stampRaycaster.transform.position, transform.TransformDirection(stampRaycaster.transform.forward * -10), out hit, Mathf.Infinity, layer))
                    {
                        if (hit.collider.gameObject.CompareTag("column1"))
                        {
                            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.photo)
                            {
                                OnCatchWrongInfo();
                            }
                        }
                        else if (hit.collider.gameObject.CompareTag("column2"))
                        {
                            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.name)
                            {
                                OnCatchWrongInfo();
                            }
                        }
                        else
                        if (hit.collider.gameObject.CompareTag("column3"))
                        {
                            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.relation)
                            {
                                OnCatchWrongInfo();
                                Debug.Log("Relation is wrong");
                            }
                        }

                        Debug.DrawRay(stampRaycaster.transform.position, transform.TransformDirection(stampRaycaster.transform.forward) * -10, Color.red);

                        stampHitPosition = IDCard.InverseTransformPoint(hit.point);

                       
                        stampAnim.SetTrigger("stamp");

                        Timer.Delay(0.2f, () =>
                        {
                            audioManager.PlayAudio(aud_stamp);
                            Instantiate(dustParticles, hit.point, Quaternion.identity);
                        });

                        StartCoroutine(ActivatePaintBall());
                        //LerpObjectLocalPosition.instance.LerpLocalObject(stamp, stampHitPosition, 0.2f, () =>
                        //{
                        //    Debug.Log("Stamp hit Completed");
                        //    //IDlerpComplete.Raise();
                        //});

                        Debug.Log("collided "+hit.collider.name);
                    }
                    else
                    {
                        Debug.DrawRay(stampRaycaster.transform.position, transform.TransformDirection(stampRaycaster.transform.forward) * 1000, Color.white);
                        Debug.Log("Did not Hit");
                    }

                    loader.fillAmount = 0;
                    loader.color = loaderStartColor;

                    Vector3 forward = stamp.transform.TransformDirection(stamp.transform.forward) * -10;
                    Debug.DrawRay(stamp.transform.position, forward, Color.red);
                }
            }
            else
            {
                heldTimer = 0;
                loader.gameObject.SetActive(false);
                loader.fillAmount = heldTimer;
                loader.color = loaderStartColor;

            }

            lastPos = pos;
#endif
        }

        IEnumerator ActivatePaintBall() {
            yield return new WaitForSeconds(0.5f);
            paintBall.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            paintBall.SetActive(false);
            readyForStamp = true;
            stamp.GetComponent<Lean.Common.LeanConstrainToColliders>().enabled = true;
            stamp.GetComponent<Lean.Touch.LeanDragTranslate>().enabled = true;
        }


        void OnClickEnd(Vector2 pos)
        {
            heldTimer = 0;
            loader.gameObject.SetActive(false);
            loader.fillAmount = heldTimer;

        }
        //private void Update()
        //{
        //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hit, 100f))
        //    {
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            if (hit.collider.gameObject.CompareTag("column1"))
        //            {
        //                Debug.Log("_mPlayPhasesControl.idCheck_cursr "+ _mPlayPhasesControl.idCheck_cursr);

        //                if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.photo)
        //                {
        //                    OnCatchWrongInfo();
        //                }
        //            }
        //            if (hit.collider.gameObject.CompareTag("column2"))
        //            {
        //                if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.name)
        //                {
        //                    OnCatchWrongInfo();
        //                }
        //            }
        //            if (hit.collider.gameObject.CompareTag("column3"))
        //            {
        //                if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.relation)
        //                {
        //                    OnCatchWrongInfo();
        //                }
        //            }
                   
        //        }
        //    }
        //}

        void OnCatchWrongInfo()
        {
            if (!Progress.Instance.IsIdStepTutDone && _mPlayPhasesControl.CurrentMiniLevel == 0)
                tutorialHand1.SetActive(true);

            respondMsg.ShowCorrectMsg();
            tutorialMsg.SetActive(false);

            btnArrest.interactable = true;
        }

        public void HideId()
        {
            popUp.SetActive(false);
            IDCardParent.gameObject.SetActive(false);
            checkPanel.SetActive(false);

            IDCardParent.position = IDStartPos.position;
            IDCardParent.transform.rotation = IDStartPos.rotation;
        }

        IEnumerator Steps()
        {
            yield return new WaitForSeconds(.5f);
            ShowPopUp();
        }

        void ShowPopUp()
        {
            GetComponent<AudioSource>().clip = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].audio_intro;
            GetComponent<AudioSource>().Play();

            popUp.SetActive(true);
            popupText.WholeText = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].popUpText;
            popupText.ShowTextResponse(() =>
            {
                StartCoroutine(ShowId());
            });
        }

        IEnumerator ShowId()
        {
            cloudParticle.SetActive(true);
            IDCardParent.gameObject.SetActive(true);
            yield return new WaitForSeconds(.65f);

            LerpID();
            yield return new WaitForSeconds(1);
            checkPanel.SetActive(true);

            if (!Progress.Instance.IsIdStepTutDone && _mPlayPhasesControl.CurrentMiniLevel == 0)
            {
                tutorialHand1.SetActive(true);
            }
            else
            if (!Progress.Instance.IsIdStepTutDone && _mPlayPhasesControl.CurrentMiniLevel == 1)
            {
                tutorialHand2.SetActive(true);
                Progress.Instance.IsIdStepTutDone = true;
            }
        }

        public void LerpID()
        {
            audioManager.PlayAudio(aud_idCardAppear);

            LerpObjectPosition.instance.LerpObject(IDCardParent, IDFinalPos.position, lerpSpeed, () =>
            {
                Debug.Log("Lerp Completed");
                //IDlerpComplete.Raise();

            });
            LerpObjectRotation.instance.LerpObject(IDCardParent, IDFinalPos.rotation, lerpSpeed);
        }

        public void Suspicious()
        {
            btnArrest.gameObject.SetActive(true);
            btnSuspicious.gameObject.SetActive(false);

            stamp.gameObject.SetActive(true);

            if (!Progress.Instance.IsIdStepTutDone || _mPlayPhasesControl.CurrentMiniLevel == 0)
            {
                tutorialHand1.SetActive(false);
                tutorialLoop.SetActive(true);
            }

            tutorialMsg.SetActive(true);

            _mGamePlayStep.OnSuspicious();

            readyForStamp = true;
        }

        public void Arrest()
        {
            copManager.Catch();
            _mGamePlayStep.OnArrestStarted();

            if (_mPlayPhasesControl.CurrentMiniLevel == 0)
                tutorialHand1.SetActive(false);

            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo != IDCheck_SO.WrongInfo.none)
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

            if (_mPlayPhasesControl.idCheck_cursr == 3)
            {
                Progress.Instance.WasBadDecision = true;
            }
            Timer.Delay(2, () =>
            {
                _mPlayPhasesControl._OnMiniLevelFinished2();
            });
            HideId();

            audioManager.PlayAudio(aud_arrest);
        }

        public void Allow()
        {

            if (_mPlayPhasesControl.CurrentMiniLevel == 1)
            {
                tutorialHand2.SetActive(false);
                tutorialMsg.SetActive(true);
            }

            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].wrongInfo == IDCheck_SO.WrongInfo.none)
            {
                _mPlayPhasesControl.correctAnswers++;
                respondMsg.ShowCorrectMsg();
                Progress.Instance.IncreamentRating(3);
            }
            else
            {
                respondMsg.ShowWrongMsg();
                Progress.Instance.DecreamentRating(3);
                //if (_mPlayPhasesControl.idCheck_cursr == 3)
                //{
                //    Progress.Instance.WasBadDecision = true;
                //}
            }

            _mPlayPhasesControl._OnMiniLevelFinished();
            HideId();
          
        }

        void AssignData()
        {
            // Display Popup data
            //  popupText.text = "" + _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].popUpText;

            // Display ID Data
            nameText.text = ""+ _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].ID_name;
            ageText.text = ""+ _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].ID_age;
            inmateRelationText.text = "" + _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr - 1].relation;

            //   inmateRelationText.text = "" + _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetIDCheckInfo[_mPlayPhasesControl.idCheck_cursr];

            //Debug.Log("character name "+ _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].Guests[_mPlayPhasesControl.CurrentMiniLevel].DP);
            dpHolder.sprite = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].Guests[_mPlayPhasesControl.CurrentMiniLevel].DP;
        }
    }
}
