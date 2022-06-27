using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrisonControl
{
    public class CheckCellManager : MonoBehaviour
    {
        public static CheckCellManager instance;

        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;
        private CellCheck_SO cellCheck_SO;

        [SerializeField]
        private GameObject mainCam;

        [SerializeField]
        private CameraManager cameraManager;

        [SerializeField]
        private CameraControl cameraControl;

        CellData cellData
        {
            get
            {
                return cellCheck_SO.pf_cell.GetComponent<CellData>();
            }
        }
        public Transform hidObjContent;
        public GameObject cloudEfx;
        public GameObject glowEfx;
        public Transform arrestPanel;
        public Transform lerpPostion;
        public Transform cellSpawnPos;
        public float lerpSpeed;
        public float objectDisappearTime;

        private Animator prisonerAnim;
        private GameObject prisoner;
        [SerializeField]
        private Transform pisonerSpawnPos;

        public Animator prisonDoor;
        public Transform prisonerTarget1;
        public Transform prisonerTarget2;

        public TypewriterEffect typewriterEffect;
        public List<GameObject> hiddenObjects = new List<GameObject>();
        //  public List<Texture2D> hiddenUIIcon = new List<Texture2D>();
        public bool raycast = false;
        public CellData currentCell;
        public GameObject allowButton;
        public float allowOnTimer = 30.0f;
        public Coroutine allowOnCoroutine;

        [SerializeField]
        private AudioClip aud_slap, aud_taze, aud_tap, aud_itemFound, aud_lowBlow, aud_hammer, aud_spit, aud_spiderBucket, aud_chickenDance;

        [SerializeField]
        private GameObject hammer, spit;

        [SerializeField]
        private Image img_icon;

        [SerializeField]
        private Text txt_punishment;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        void OnEnable()
        {
            StartCoroutine(Step());
        }

        IEnumerator Step()
        {
            cameraControl.SetCurrentCamera("StartCamera", 0);

            cellCheck_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetCellCheckInfo;

            if (prisoner)
                Destroy(prisoner);

            prisoner = Instantiate(cellCheck_SO.prisoner, pisonerSpawnPos);
            prisonerAnim = prisoner.GetComponent<Animator>();

            prisonerAnim.Play("Idle");
            yield return new WaitForSeconds(.1f);
            Init();
            Timer.Delay(0.3f, () =>
            {
                DoorOpen();
            });
            CharacterInit();
        }

        void Init()
        {
            for (int i = 0; i < cellSpawnPos.childCount; i++)
            {
                Destroy(cellSpawnPos.GetChild(i).gameObject);
            }

            for (int i = 0; i < hidObjContent.childCount; i++)
            {
                Destroy(hidObjContent.GetChild(i).gameObject);
            }

            currentCell = Instantiate(cellData, cellSpawnPos);

            cameraControl.AddCamera(cellData.gameCamera1);
            cameraControl.AddCamera(cellData.gameCamera2);

            txt_punishment.text = cellCheck_SO.punishment.ToString();

            img_icon.sprite = Resources.Load("PunishmentEmoji/" + cellCheck_SO.punishment, typeof(Sprite)) as Sprite;

            for (int i = 0; i < cellData.hiddenObjects.Count; i++)
            {
                Debug.Log("name " + cellData.hiddenObjects[i].name);
                Image obj = Instantiate(cellCheck_SO.pf_hiddenObject, hidObjContent).GetComponent<Image>();
                obj.name = cellData.hiddenObjects[i].name;
                Texture2D texture = cellCheck_SO.pf_cell.GetComponent<CellData>().hiddenObj_icons.Find(x => x.name == obj.name);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                obj.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
                hiddenObjects.Add(obj.gameObject);
            }
            if(currentCell.tutorial)
                currentCell.tutorial.SetActive(false);
            lerpPostion = currentCell.lerpPostion;



            /*Timer.Delay(1.0f, () =>
            {
                cameraControl.SetCurrentCamera("GameCamera1", 1);
            });*/


        }


        void Update()
        {
            if (!raycast) return;
            SetInput();
        }

        void SetInput()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
#if UNITY_EDITOR

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.TryGetComponent<Interactables>(out Interactables interactable))
                    {
                        GetComponent<AudioSource>().clip = aud_tap;
                        GetComponent<AudioSource>().Play();

                        interactable.Interact();
                    }
                }
#else

                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (hit.collider.TryGetComponent<Interactables>(out Interactables interactable))
                        {
                            interactable.Interact();
                        }
                    }
                }
#endif
            }
        }

        void CharacterInit()
        {
            prisonerAnim.Play("WalkOut");
            LerpObjectPosition.instance.LerpObject(prisoner.transform, prisonerTarget1.position, 2.0f, () =>
            {
                LerpObjectRotation.instance.LerpObject(prisoner.transform, prisonerTarget1.rotation, 0.5f, () =>
                {
                    LerpObjectPosition.instance.LerpObject(prisoner.transform, prisonerTarget2.position, 1.0f, () =>
                    {
                        LerpObjectRotation.instance.LerpObject(prisoner.transform, prisonerTarget2.rotation, 0.5f, () =>
                        {
                            prisonerAnim.Play("Idle");
                            PlayDialogue(cellCheck_SO.prisonerDialogue, cellCheck_SO.audDialogue,() =>
                            {
                                StepIntoGame();
                            });

                        });
                    });
                });

            });
        }

        void PlayDialogue(string dialogue, AudioClip aud, System.Action callback = null)
        {
            cameraControl.SetCurrentCamera("Arrest", 0.5f);
            Timer.Delay(1.0f, () =>
            {
                prisonerAnim.Play("Talk");
                typewriterEffect.transform.parent.gameObject.SetActive(true);
                Timer.Delay(0.5f, () =>
                {
                    GetComponent<AudioSource>().clip = aud;
                    GetComponent<AudioSource>().Play();

                    typewriterEffect.WholeText = dialogue;
                    typewriterEffect.ShowTextResponse(() =>
                    {
                        Timer.Delay(1.0f, () =>
                        {
                            typewriterEffect.transform.parent.gameObject.SetActive(false);
                            typewriterEffect.textMeshPro.text = "";
                            Timer.Delay(0.5f, () =>
                            {
                                callback();
                                prisonerAnim.CrossFade("Idle",0.05f);
                            });
                        });
                    });
                });
            });
        }


        void StepIntoGame()
        {
            Timer.Delay(1.5f, () =>
           {
               cameraControl.SetCurrentCamera("StartCamera", 0.5f);
               Timer.Delay(1.5f, () =>
               {
                   cameraControl.SetCurrentCamera("GameCamera2", 0.5f);
                   Timer.Delay(1.0f, () =>
                   {
                       hidObjContent.transform.parent.gameObject.SetActive(true);
                       hidObjContent.gameObject.SetActive(true);
                       Timer.Delay(3f, () =>
                       {
                           raycast = true;
                           allowOnCoroutine = StartCoroutine(AllowButton());
                           if (currentCell.tutorial)
                           {
                               currentCell.tutorial.SetActive(true);
                           }
                           print("Game Start");
                       });
                   });
               });

           });
        }

        public void OffTut()
        {
            if (cellData.tutorial)
            {
                currentCell.tutorial.SetActive(false);
            }
        }

        void DoorOpen()
        {
            prisonDoor.Play("DoorOpen");
        }

        public void Found(string name)
        {
            GetComponent<AudioSource>().clip = aud_itemFound;
            GetComponent<AudioSource>().Play();

            hiddenObjects.Find(x => x.name == name).GetComponent<Animator>().SetTrigger("pop");
            hiddenObjects.Find(x => x.name == name).GetComponent<Image>().color = Color.green;

            hiddenObjects.Remove(hiddenObjects.Find(x => x.name == name));
            if (hiddenObjects.Count == 0)
            {
                Timer.Delay(1.0f, () =>
                {
                    Suspicious();
                });
                print("You found all the objects");
            }
        }

        public void Suspicious()
        {
            PlayDialogue(cellCheck_SO.prisonerExitDialogue, cellCheck_SO.audDialogueExit, () =>
            {
                arrestPanel.gameObject.SetActive(true);
            });
            StopCoroutine(allowOnCoroutine);

            if(currentCell.tutorial)
                currentCell.tutorial.SetActive(false);
            allowButton.SetActive(false);
            //cameraControl.SetCurrentCamera("Arrest", 1);
            //prisonerAnim.Play("Talk");
            hidObjContent.transform.parent.gameObject.SetActive(false);
            hidObjContent.gameObject.SetActive(false);
        }
        public void Punish()
        {
            Progress.Instance.IncreamentRating(3);
            arrestPanel.gameObject.SetActive(false);

            if (cellCheck_SO.punishment == Punishment.Slap)
            {
                Slap();
            }
            else if (cellCheck_SO.punishment == Punishment.Taser)
            {
                Taze();
            }
            else if(cellCheck_SO.punishment == Punishment.LowBlow)
            {
                LowBlow();
            }
            else if (cellCheck_SO.punishment == Punishment.HammerHit)
            {
                HammerHit();
            }
            else  if (cellCheck_SO.punishment == Punishment.Slap)
            {
                SpiderBucket();
            }
            else if (cellCheck_SO.punishment == Punishment.Spit)
            {
                Spit();
            }
            else if (cellCheck_SO.punishment == Punishment.ChickenDance)
            {
                ChickenDance();

            }
            Progress.Instance.WasBadDecision = true;

        }

        void Slap()
        {
            GetComponent<AudioSource>().clip = aud_slap;
            GetComponent<AudioSource>().Play();

            prisonerAnim.Play("Slap");
            prisonerAnim.gameObject.GetComponent<PunishmentParticles>().SlapParticles();
            Timer.Delay(3.0f, () =>
            {
                EndLevel();
            });
        }

        void Taze()
        {
            GetComponent<AudioSource>().clip = aud_taze;
            GetComponent<AudioSource>().Play();

            prisonerAnim.Play("Tazer");
            prisonerAnim.gameObject.GetComponent<PunishmentParticles>().ActivateShock();

            Timer.Delay(3.0f, () =>
            {
                EndLevel();
            });
        }

        void LowBlow()
        {
            GetComponent<AudioSource>().clip = aud_lowBlow;
            GetComponent<AudioSource>().Play();

            prisonerAnim.Play("LowBlow");
            prisonerAnim.gameObject.GetComponent<PunishmentParticles>().LowBlowParticles();

            Timer.Delay(3.0f, () =>
            {
                EndLevel();
            });
        }

        void HammerHit()
        {
            hammer.SetActive(true);
            GetComponent<AudioSource>().clip = aud_hammer;
            GetComponent<AudioSource>().Play();

            prisonerAnim.Play("HammerHit");

            Timer.Delay(3.0f, () =>
            {
                hammer.SetActive(false);
                EndLevel();
            });
        }

        void Spit()
        {
            spit.SetActive(true);
            GetComponent<AudioSource>().clip = aud_spit;
            GetComponent<AudioSource>().Play();

            prisonerAnim.Play("Spit");
            Timer.Delay(3.0f, () =>
            {
                spit.SetActive(false);
                EndLevel();
            });
        }

        void SpiderBucket()
        {
            GetComponent<AudioSource>().clip = aud_spiderBucket;
            GetComponent<AudioSource>().Play();

            prisonerAnim.Play("SpiderBucket");

            Timer.Delay(3.0f, () =>
            {
                spit.SetActive(false);
                EndLevel();
            });
        }
        void ChickenDance()
        {
            GetComponent<AudioSource>().clip = aud_chickenDance;
            GetComponent<AudioSource>().Play();

            prisonerAnim.Play("ChickenDance");

            Timer.Delay(3.0f, () =>
            {
                spit.SetActive(false);
                EndLevel();
            });
        }

        public void LetGo()
        {
            Progress.Instance.DecreamentRating(3);

            prisonerAnim.Play("happy1");
            arrestPanel.gameObject.SetActive(false);

            Timer.Delay(1.0f, () =>
            {
                EndLevel();
            });
        }

        public void Explode()
        {
            cloudEfx.transform.position = lerpPostion.position;
            cloudEfx.SetActive(true);
            Timer.Delay(2.0f, () =>
            {
                cloudEfx.SetActive(false);
            });
        }

        public void Glow(Transform parent)
        {
            glowEfx.transform.parent = parent;
            glowEfx.transform.position = parent.position;
            glowEfx.SetActive(true);
            Timer.Delay(2.0f, () =>
            {
                glowEfx.SetActive(false);
            });
        }

        public void EndLevel()
        {
            _mPlayPhasesControl._OnPhaseFinished();
        }

        IEnumerator AllowButton()
        {
            yield return new WaitForSeconds(allowOnTimer);
            allowButton.SetActive(true);
        }
    }
}
