using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NaughtyAttributes;


namespace PrisonControl
{
    public class JailEntryStep : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private CameraManager cameraManager;

        [SerializeField]
        private Transform cam3;

        [SerializeField]
        private GameObject pf_lerpObject;

      
        private float lerpSpeed, rotLerpSpeed;

        [SerializeField]
        private GameObject metalDetector;

        private List<GameObject> prisoners;

        [SerializeField]
        private int currentPrisoner;

        [SerializeField]
        private JailEntryUi jailEntryUi;

        [SerializeField]
        private Transform[] hidePos;
        private List<GameObject> hiddenObjects;
        private List<int> hiddenObjectIndexes;

        // Raycaster
        [SerializeField]
        private Transform raycaster;
        public Vector2 startPos;
        Ray ray;
        RaycastHit hit;

        Vector2 lastPos;

        [SerializeField]
        float heldTimer;

        float heldDelay;

        [SerializeField]
        private Image loader;
        [SerializeField]
        private LayerMask layer;

        private int numberOfObjectsFound;

        // Smooth Cam follow
        public Vector3 Offset;
        private Vector3 velocity = Vector3.zero;
        private bool moveCamWithDetector;
        private Vector3 cam3DefaultPos;

        private bool isDetectorStep;

        [SerializeField]
        private GameObject scanPreview, scanParticle;

        public GameObject popUpEntry, popUpExit;
        public TypewriterEffect txt_popUpEntry, txt_popUpExit;

        [Foldout("Transform")]
        [SerializeField]
        private Transform scannerPos, handCheckPos, stepOutPos, detetcorInPos, detetcorOutPos, copPosIn, copPosOut;

        [SerializeField]
        private Transform[] queuePos;

        private bool offsetRecorded;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip aud_scanner, aud_taser, aud_chickenDance, aud_slap, aud_arrest, aud_spit, aud_hammerHit, aud_lowBlow, aud_spiderBucket;

        [SerializeField]
        RespondMessage respondMsg;

        [SerializeField]
        private Avatar cop;

        [SerializeField]
        private Color32 glowColorRed, glowColorGreen;

        [SerializeField]
        private MeshRenderer metalDetectorGlow;

        [SerializeField]
        private GameObject cryEmoji, hammer, spit, spiderBucket;

        private void Awake()
        {
            jailEntryUi._OnPassClicked += OnPassClicked;
            jailEntryUi._OnArrestClicked += OnArrestClicked;

            InputManager.MouseDragStarted += OnClick;
            InputManager.MouseDragged += OnDrag;
            InputManager.MouseDragEnded += OnClickEnd;

            prisoners = new List<GameObject>();
            hiddenObjects = new List<GameObject>();
            hiddenObjectIndexes = new List<int>();

            lerpSpeed = 1f;
            rotLerpSpeed = .3f;
        }

        private void OnDestroy()
        {
            jailEntryUi._OnPassClicked -= OnPassClicked;
            jailEntryUi._OnArrestClicked -= OnArrestClicked;

            InputManager.MouseDragStarted -= OnClick;
            InputManager.MouseDragged -= OnDrag;
            InputManager.MouseDragEnded -= OnClickEnd;

        }
        void OnEnable()
        {
            numberOfObjectsFound = 0;
            currentPrisoner = 0;
            hiddenObjectIndexes.Clear();
            StartCoroutine(Entry());
        }

        void OnPassClicked(bool responseType)
        {
            
            popUpExit.SetActive(false);
            ShowResponseMsg(true);

            EndStep(responseType, 1);
        }

        void ShowResponseMsg(bool responseType)
        {
            JailEntry_SO jailEntry_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetJailEntryInfo[currentPrisoner - 1];

            if (responseType)
            {
                if (jailEntry_SO.isGuilty)
                {
                    respondMsg.ShowWrongMsg();
                    prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("relieved");
                    Progress.Instance.DecreamentRating(2);
                }
                else
                {
                    respondMsg.ShowCorrectMsg();
                    prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("gratification");
                    Progress.Instance.IncreamentRating(2);
                }
            }
            else
            {
                if (jailEntry_SO.isGuilty)
                {
                    respondMsg.ShowCorrectMsg();
                    Progress.Instance.IncreamentRating(2);
                }
                else
                {
                    respondMsg.ShowWrongMsg();
                    Progress.Instance.DecreamentRating(2);
                }
            }

           
        }

      

      

        void OnStepAheadClicked()
        {
            cameraManager.ActivateCamJailEntry3();
            StepInHandCheck(() =>
            {
                OnCharacterReachedHandCheck();
            });
        }

        IEnumerator Entry()
        {
            metalDetector.GetComponent<Lean.Common.LeanConstrainToColliders>().enabled = false;
            metalDetector.transform.position = detetcorOutPos.position;
            cam3DefaultPos = cam3.transform.position;
            SpawnPrisoner();

            yield return new WaitForSeconds(0.2f);

            cameraManager.ActivateCamJailEntry2();

            yield return new WaitForSeconds(0.5f);
            popUpEntry.SetActive(false);

            currentPrisoner++;
            StepInScanning(() => {
                OnCharacterReachedPlatform();
            });
        }
       
        void NextPrisoner(bool isMoveOut)
        {
            Debug.Log("move character in ");
            scanPreview.SetActive(false);
            scanParticle.SetActive(false);

            numberOfObjectsFound = 0;
            hiddenObjectIndexes.Clear();

            DeleteHiddenObjects();

            if (isMoveOut)
            {
                cameraManager.ActivateCamJailEntry2();

                if (currentPrisoner < 3)
                {
                    Destroy(prisoners[currentPrisoner - 1]);

                    currentPrisoner++;
                    StepInScanning(() => {
                        OnCharacterReachedPlatform();
                    });
                }
                else
                {
                    EndLevel();
                }
            }
            else
            {
                StepOutScanning(() => {
                    cameraManager.ActivateCamJailEntry2();

                    if (currentPrisoner < 3)
                    {
                        Destroy(prisoners[currentPrisoner - 1]);

                        currentPrisoner++;
                        StepInScanning(() => {
                            OnCharacterReachedPlatform();
                        });
                    }
                    else
                    {
                        EndLevel();
                    }
                });
            }
           
        }

        void SpawnPrisoner()
        {
            for (int i = 0; i < 3; i++)
            {
                JailEntry_SO jailEntry_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetJailEntryInfo[i];
                GameObject obj = Instantiate(jailEntry_SO.prisoner.pf_character, queuePos[i].position, Quaternion.Euler(0, -90, 0), transform);
                prisoners.Add(obj);
                obj.GetComponent<Animator>().applyRootMotion = true;
                obj.GetComponent<Animator>().Play("Idle_jailEntry");
            }
        }

        void StepInScanning(System.Action callback)
        {
            prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("walk");

            LerpObjectPosition2.instance.LerpObject(prisoners[currentPrisoner - 1].transform, scannerPos.position, lerpSpeed, () =>
            {
                Debug.Log("reched");
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("idle");

                callback.Invoke();
            }
            );
        }

        void StepInHandCheck(System.Action callback)
        {
            Debug.Log("going to hand check");

            cameraManager.ActivateCamJailEntry3();
            prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("walk");

            LerpObjectPosition2.instance.LerpObject(prisoners[currentPrisoner - 1].transform, handCheckPos.position, lerpSpeed, () =>
            {
                Debug.Log("reached hand check");
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("idle");
                callback.Invoke();
            }
            );
        }

        void StepOutScanning(System.Action callback)
        {
            LerpObjectRotation.instance.LerpObject(prisoners[currentPrisoner - 1].transform, Quaternion.Euler(new Vector3(0, prisoners[currentPrisoner - 1].transform.rotation.eulerAngles.y - 90.0f, 0)), rotLerpSpeed, () => {
                LerpPassPosition(callback);
            });
        }

        void LerpPassPosition(System.Action callback)
        {
            //if (guestMood == GuestMood.Angry)
            //    GetComponent<Animator>().Play("AngryWalk");
            //else
            //    GetComponent<Animator>().Play("walk");

            prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("walk");

            LerpObjectPosition.instance.LerpObject(prisoners[currentPrisoner - 1].transform, stepOutPos.position, lerpSpeed, () => {

                Debug.Log("prisoner out");
                callback.Invoke();
            });
        }

        void OnCharacterReachedPlatform()
        {

            cam3.transform.position = cam3DefaultPos;

            // Spawn hidden objects
            JailEntry_SO jailEntry_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetJailEntryInfo[currentPrisoner - 1];
            for (int j = 0; j < jailEntry_SO.listHiddenObjects.Count; j++)
            {
                //GameObject hiddenObj = Instantiate(jailEntry_SO.listHiddenObjects[j], GetUniqueHiddenPose(), Quaternion.identity, transform);

                GameObject hiddenObj = Instantiate(jailEntry_SO.listHiddenObjects[j], hidePos[(int)jailEntry_SO.hidePos[j]].position, Quaternion.identity, transform);
                hiddenObjects.Add(hiddenObj);
            }

            scanPreview.GetComponent<Image>().sprite = jailEntry_SO.scanImage;

            ShowPopUpEntry();

            jailEntryUi.SetupPunishment(jailEntry_SO);
        }

        void ShowPopUpEntry()
        {
            popUpEntry.SetActive(true);

            audioSource.clip = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetJailEntryInfo[currentPrisoner - 1].aud_firstDialogue;
            audioSource.Play();

            txt_popUpEntry.WholeText = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetJailEntryInfo[currentPrisoner - 1].firstDialogue;

            txt_popUpEntry.ShowTextResponse(() =>
            {
                StartCoroutine(OnPopupEntryDone());
            });
        }

        IEnumerator OnPopupEntryDone()
        {
            yield return new WaitForSeconds(0.5f);
            popUpEntry.SetActive(false);

         //   yield return new WaitForSeconds(0.4f);

            scanPreview.SetActive(true);
            scanParticle.SetActive(true);

            // Start scanning
            Timer.Delay(1f, () =>
            {
                StartScanning();
            });
        }

        void OnCharacterReachedHandCheck()
        {
            metalDetector.SetActive(true);
            MoveDetectorIn();
        }

        void StartScanning()
        {
            Debug.Log("doing a scan");

            audioSource.clip = aud_scanner;
            audioSource.Play();

            Timer.Delay(1f, () =>
            {
                scanPreview.GetComponent<ScaleBounce>().Bounce();
                Debug.Log("scan done");
                OnStepAheadClicked();
                scanParticle.SetActive(false);
            });
        }

        void MoveDetectorIn()
        {
            GameObject lerpObjectPosition = Instantiate(pf_lerpObject);

            lerpObjectPosition.GetComponent<LerpObject>().Lerp(metalDetector.transform, detetcorInPos.position, 0.3f, () =>
            {
                metalDetector.GetComponent<Lean.Common.LeanConstrainToColliders>().enabled = true;

                Debug.Log("detector moved in");
                jailEntryUi.ActivateLoop(true);

                if (!offsetRecorded)
                {
                    offsetRecorded = true;
                    Offset = cam3.transform.position - metalDetector.transform.position;
                }

                moveCamWithDetector = true;

                isDetectorStep = true;

                Debug.Log("Destroyed");
                Destroy(lerpObjectPosition);

            });
        }

        void MoveDetectorOut()
        {
            metalDetector.GetComponent<Lean.Common.LeanConstrainToColliders>().enabled = false;

            GameObject lerpObjectPosition = Instantiate(pf_lerpObject);
            lerpObjectPosition.GetComponent<LerpObject>().Lerp(metalDetector.transform, detetcorOutPos.position, 0.3f, () =>
            {
                metalDetector.SetActive(false);
                Debug.Log("Destroyed");
                Destroy(lerpObjectPosition);
            });
        }


        Vector3 GetUniqueHiddenPose()
        {
            Vector3 pos;
            int randNo;
            do
            {
                randNo = Random.Range(0, hidePos.Length);
                pos = hidePos[randNo].position;
                Debug.Log("------- twiceeeee");

            } while (hiddenObjectIndexes.Contains(randNo));

            hiddenObjectIndexes.Add(randNo);
            return pos;
        }

        void EndLevel()
        {
            for (int i = 0; i < 3; i++)
            {
                Destroy(prisoners[i]);
            }

            prisoners.Clear();

            Timer.Delay(.5f, () =>
            {
                _mPlayPhasesControl._OnPhaseFinished();

            });

        }

        void Update()
        {
#if !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    OnClick(Vector2.one);
                }
                else
                if (touch.phase == TouchPhase.Moved)
                {
                    OnDrag(Vector2.one);
                }
                else
                if (touch.phase == TouchPhase.Ended)
                {
                    OnClickEnd(Vector2.one);
                }
            }
#endif
        }

        void LateUpdate()
        {
            if (moveCamWithDetector)
            {
                Vector3 desiredPosition = metalDetector.transform.position + Offset;
                Vector3 smoothedPosition = Vector3.Lerp(cam3.transform.position, desiredPosition, 0.125f);
                cam3.transform.position = smoothedPosition;

                //Vector3 targetPosition = metalDetector.transform.position + Offset;
                //cam3.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f);
            }
        }
        void OnClick(Vector2 pos)
        {
            if (!isDetectorStep)
                return;

            metalDetector.GetComponent<Animator>().SetBool("idle", true);

            cameraManager.ActivateCamMetalDetector();
            jailEntryUi.ActivateLoop(false);

            jailEntryUi.ActivateAllowPanel(false);
            jailEntryUi.ActivateArrestPanel(false);
        }

        void OnDrag(Vector2 pos)
        {
            if (!isDetectorStep)
                return;

            //Debug.DrawRay(raycaster.transform.position, transform.TransformDirection(raycaster.transform.forward) * 10, Color.green);

            if (Physics.Raycast(raycaster.transform.position, transform.TransformDirection(raycaster.transform.forward * 10), out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("hiddenObject"))
                {
                    Debug.DrawRay(raycaster.transform.position, transform.TransformDirection(raycaster.transform.forward) * 10, Color.green);
                    hit.collider.GetComponent<Animator>().SetTrigger("popOut");

                    Debug.Log("found object");

                    numberOfObjectsFound++;
                    metalDetector.GetComponent<Animator>().SetTrigger("glow");

                   // metalDetectorGlow.material.SetColor("_EmissionColor",glowColorRed);

                    JailEntry_SO jailEntry_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetJailEntryInfo[currentPrisoner - 1];
                    if (numberOfObjectsFound == jailEntry_SO.listHiddenObjects.Count)
                    {
                        Timer.Delay(1f, () =>
                        {
                            ShowPopUpExit();

                            isDetectorStep = false;
                            metalDetector.GetComponent<Lean.Common.LeanConstrainToColliders>().enabled = false;
                            MoveDetectorOut();
                            cameraManager.ActivateCamJailEntry3();
                        });
                    }

                    hit.collider.enabled = false;
                }
                else
                {

                //    metalDetectorGlow.material.SetColor("_EmissionColor", glowColorGreen);

                    Debug.DrawRay(raycaster.transform.position, transform.TransformDirection(raycaster.transform.forward) * 10, Color.red);
                }

            }
        }

        void ShowPopUpExit()
        {
            scanPreview.SetActive(false);
            popUpExit.SetActive(true);

            audioSource.clip = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetJailEntryInfo[currentPrisoner - 1].aud_caughtDialogue;
            audioSource.Play();

            txt_popUpExit.WholeText = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetJailEntryInfo[currentPrisoner - 1].caughtDialogue;

            txt_popUpExit.ShowTextResponse(() =>
            {
                StartCoroutine(OnPopupExitDone());
            });
        }

        IEnumerator OnPopupExitDone()
        {
            yield return new WaitForSeconds(0.5f);
            jailEntryUi.ActivateArrestPanel(true);

        }

        void OnArrestClicked(string arrestType)
        {
            if (currentPrisoner == 3)
            {
                Debug.Log("bad decision");
                Progress.Instance.WasBadDecision = true;
            }

            ShowResponseMsg(false);

            popUpExit.SetActive(false);

            DeleteHiddenObjects();

            if (arrestType == Punishment.Slap.ToString())
            {
                audioSource.clip = aud_slap;
                audioSource.Play();
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("Slap");
                Timer.Delay(1f, () =>
                {
                    Catch();
                });
            }
            else if (arrestType == Punishment.Taser.ToString())
            {
                audioSource.clip = aud_taser;
                audioSource.Play();
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("Taser");
                prisoners[currentPrisoner - 1].GetComponent<PunishmentParticles>().ActivateShock();
                Timer.Delay(1.1f, () =>
                {
                    prisoners[currentPrisoner - 1].GetComponent<PunishmentParticles>().DeActivateShock();
                    Catch();
                });
            }
            else if (arrestType == Punishment.ChickenDance.ToString())
            {
                audioSource.clip = aud_chickenDance;
                audioSource.Play();
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("ChickenDance");
                Timer.Delay(4f, () =>
                {
                    Catch();
                });
            }
            else if (arrestType == Punishment.JumpingJack.ToString())
            {
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("JumpingJacks");
                Timer.Delay(4f, () =>
                {
                    Catch();
                });
            }
            else if (arrestType == Punishment.HammerHit.ToString())
            {
                hammer.SetActive(true);
                audioSource.clip = aud_hammerHit;
                audioSource.Play();
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("HammerHit");

                Timer.Delay(3f, () =>
                {
                    hammer.SetActive(false);
                    EndStep(true, 1);
                });
            }
            else if (arrestType == Punishment.LowBlow.ToString())
            {
                audioSource.clip = aud_lowBlow;
                audioSource.Play();
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("LowBlow");

              //  cryEmoji.SetActive(true);
                Timer.Delay(3f, () =>
                {
                   // cryEmoji.SetActive(false);
                    EndStep(true, 1);
                });
            }
            else if (arrestType == Punishment.SpiderBucket.ToString())
            {
                spiderBucket.SetActive(true);

                Timer.Delay(.4f, () =>
                {
                    audioSource.clip = aud_spiderBucket;
                    audioSource.Play();
                    prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("SpiderBucket");
                    prisoners[currentPrisoner - 1].GetComponent<PunishmentParticles>().SpiderBucket();
                });
                
                Timer.Delay(4f, () =>
                {
                    spiderBucket.SetActive(false);
                    Catch();
                });
            }
            else if (arrestType == Punishment.Spit.ToString())
            {
                spit.SetActive(true);
                audioSource.clip = aud_spit;
                audioSource.Play();
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("Spit");
                Timer.Delay(3f, () =>
                {
                    spit.SetActive(false);
                    Catch();
                });
            }
        }

        void Catch()
        {
            prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("idle");
            prisoners[currentPrisoner - 1].GetComponent<Animator>().SetInteger("scared", 2);
            cop.GetComponent<Animator>().SetTrigger("catch");

            audioSource.clip = aud_arrest;
            audioSource.Play();

            GameObject lerpObjectPosition = Instantiate(pf_lerpObject);

            //Get Cop In
            lerpObjectPosition.GetComponent<LerpObject>().Lerp(cop.transform, copPosIn.position, 0.5f, () =>
            {
                Debug.Log("cop going in");

                Timer.Delay(0.5f, () =>
                {
                    Arrest();

                    Debug.Log("Destroyed");
                    Destroy(lerpObjectPosition);
                });
            });
        }
        
        void Arrest()
        {
            // Get cop out
            GameObject lerpObjectPosition = Instantiate(pf_lerpObject);
            lerpObjectPosition.GetComponent<LerpObject>().Lerp(cop.transform, copPosOut.position, 0.1f, () =>
            {
                Debug.Log("cop going out");
                Destroy(lerpObjectPosition);
            });

            //Get Prisoner out
            GameObject lerpObjectPosition2 = Instantiate(pf_lerpObject);
            lerpObjectPosition2.GetComponent<LerpObject>().Lerp(prisoners[currentPrisoner - 1].transform, stepOutPos.position, 0.1f, () =>
            {
                Debug.Log("prisoner going out");

                EndStep(false, 0);
                Destroy(lerpObjectPosition2);
            });
        }

        void EndStep(bool isMoveOut, float delay)
        {
           // ShowResponseMsg(responseType);
            jailEntryUi.ActivateCheckPanel(false);
            moveCamWithDetector = false;
            metalDetector.GetComponent<Lean.Common.LeanConstrainToColliders>().enabled = false;
            MoveDetectorOut();

            Debug.Log("delay is "+delay);

            Timer.Delay(delay, () =>
            {
                prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("idle");
                NextPrisoner(isMoveOut);
            });
        }

        void OnClickEnd(Vector2 pos)
        {
            if (!isDetectorStep)
                return;

            heldTimer = 0;
            metalDetector.GetComponent<Animator>().SetBool("idle", false);

            //loader.gameObject.SetActive(false);
            //loader.fillAmount = heldTimer;

            cameraManager.ActivateCamJailEntry3();
            CheckItemFoundCondition();


        }

        void CheckItemFoundCondition()
        {
            JailEntry_SO jailEntry_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetJailEntryInfo[currentPrisoner - 1];

            if (numberOfObjectsFound == jailEntry_SO.listHiddenObjects.Count)
            {
               // jailEntryUi.ActivateArrestPanel(true);
            }
            else
            {
                jailEntryUi.ActivateAllowPanel(true);
                jailEntryUi.ActivateLoop(true);
            }
        }

        void DeleteHiddenObjects()
        {
            for (int i = 0; i < hiddenObjects.Count; i++)
            {
                Destroy(hiddenObjects[i]);
            }

            hiddenObjects.Clear();
        }
    }
}
