using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PrisonControl
{
    public class PrisonYardStep : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private CameraManager cameraManager;

        [SerializeField]
        private Image loader;

        private PrisonYard_SO prisonYard_SO;

        [SerializeField]
        private Transform[] spawnPoses;

        [SerializeField]
        private Transform[] camPoses;

        [SerializeField]
        private PrisonYardUi prisonYardUi;

        [SerializeField]
        private Transform mainCamTransform, camPunishment, gun;

        public bool isDetected, isLoading;

        // Raycaster
     //   [SerializeField]
        private Transform raycaster;
        private Vector2 startPos;
        private Ray ray;
        private RaycastHit hit;
        private float heldTimer;
        private float heldDelay;

        [SerializeField]
        private Transform scenarioHolder;

        private PrisonYardScenario currPrisonYardScenario;

        bool readyForRaycast;

        [SerializeField]
        private GameObject dragTut;

        [SerializeField]
        private RespondMessage respondMessage;

        private bool levelEnded;

        void Awake()
        {
            raycaster = mainCamTransform.transform;

            InputManager.MouseDragStarted += OnClick;
            InputManager.MouseDragged += OnDrag;
            InputManager.MouseDragEnded += OnClickEnd;

            prisonYardUi._OnActionTaken += OnActionTaken;
            prisonYardUi._OnLevelEnd += OnLevelEnd;

        }

        void OnDestroy()
        {
            InputManager.MouseDragStarted -= OnClick;
            InputManager.MouseDragged -= OnDrag;
            InputManager.MouseDragEnded -= OnClickEnd;

            prisonYardUi._OnActionTaken -= OnActionTaken;
            prisonYardUi._OnLevelEnd -= OnLevelEnd;
        }


        private void Update()
        {

#if !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (!isLoading)
                        dragTut.SetActive(false);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (!isLoading)
                        dragTut.SetActive(true);
                }
            }
#endif
            if (!readyForRaycast)
                return;

            RayCast();
        }
        private void LateUpdate()
        {
            loader.fillAmount = heldTimer / heldDelay;
        }
        private void OnEnable()
        {
            heldDelay = 1;
            heldTimer = 0;
            loader.fillAmount = 0;
            levelEnded = false;
            Setup();

            readyForRaycast = false;
            Invoke("Delay", 2);
        }

        void Delay()
        {
            readyForRaycast = true;
            dragTut.SetActive(true);
        }

        void Setup()
        {
            prisonYard_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetPrisonYardInfo;

            for (int i = 0; i < prisonYard_SO.pf_scenario.Length; i++)
            {
                GameObject obj = Instantiate(prisonYard_SO.pf_scenario[i], spawnPoses[(int)prisonYard_SO.spawnPoses[i]].position, Quaternion.identity, scenarioHolder);
                obj.GetComponent<PrisonYardScenario>().scenarioIndex = i;
                obj.GetComponent<PrisonYardScenario>().prisonYardStep = this;
            }
            
        }

        void OnClick(Vector2 pos)
        {

#if UNITY_EDITOR
            if (!isLoading)
                dragTut.SetActive(false);
#endif

        }

        void OnDrag(Vector2 pos)
        {
            
        }

        void OnClickEnd(Vector2 pos)
        {
#if UNITY_EDITOR
            if (!isLoading)
                dragTut.SetActive(true);
#endif

        }

        void RayCast()
        {
            if (levelEnded)
                return;

            if (isDetected)
                return;

            if (Physics.Raycast(raycaster.transform.position, transform.TransformDirection(raycaster.transform.forward * 10), out hit, Mathf.Infinity))
            {
                isLoading = true;
                if (hit.collider.gameObject.CompareTag("yardTarget"))
                {
                    heldTimer += Time.deltaTime;
                    if(heldTimer > heldDelay)
                    {
                        heldTimer = 0;

                        hit.collider.enabled = false;

                        currPrisonYardScenario = hit.collider.GetComponent<PrisonYardScenario>();
                        ShowPopup(currPrisonYardScenario);
                        isDetected = true;
                        dragTut.SetActive(false);

                        mainCamTransform.GetComponent<Lean.Touch.LeanMultiUpdate>().enabled = false;
                        
                        camPunishment.position = camPoses[(int)prisonYard_SO.spawnPoses[hit.collider.GetComponent<PrisonYardScenario>().scenarioIndex]].position;
                        camPunishment.GetComponent<Lean.Common.LeanPitchYaw>().Yaw = 0;
                        camPunishment.GetComponent<Lean.Common.LeanPitchYaw>().Pitch = 5;

                        cameraManager.ActivateCamPrisonYardPunishment(1);

                        return;
                    }
                    cameraManager.ActivateCam2PrisonYard(1);
                }
                else
                {
                    isLoading = false;
                    heldTimer = 0;
                    cameraManager.ActivateCam1PrisonYard(1);
                    prisonYardUi.HidePopup();
                }

            }
        }

        void ShowPopup(PrisonYardScenario prisonYardScenario)
        {
            // Activate 
            Timer.Delay(2f, () =>
            {
                prisonYardUi.ShowPopUp(prisonYardScenario.scenarioIndex);
                prisonYardScenario.Alert();
            });
        }

        void OnActionTaken(bool isPositive)
        {
            if (!isPositive)
            {
                // Punishment
                gun.gameObject.SetActive(true);
            }
            else
            {
                currPrisonYardScenario.BackToWork();

                if (!currPrisonYardScenario.isGuilty)
                    MoveToNextScenario(true);
                else
                    MoveToNextScenario(false);
            }
        }

        public void MoveToNextScenario(bool isCorrectDecision)
        {
            if (isCorrectDecision)
            {
                respondMessage.ShowCorrectMsg();
                Progress.Instance.IncreamentRating(2);
            }
            else
            {
                respondMessage.ShowWrongMsg();
                Progress.Instance.DecreamentRating(2);
            }

            Timer.Delay(1f, () =>
            {
                gun.gameObject.SetActive(false);
                prisonYardUi.MoveToNextScenario();

                mainCamTransform.GetComponent<Lean.Touch.LeanMultiUpdate>().enabled = true;
                isDetected = false;

                dragTut.SetActive(true);
            });
        }

        void OnLevelEnd()
        {
            levelEnded = true;

            dragTut.SetActive(false);
            cameraManager.DeActivateCamPrisonYard();

            Timer.Delay(1f, () =>
            {
                for (int i = 0; i < scenarioHolder.transform.childCount; i++)
                {
                    Destroy(scenarioHolder.transform.GetChild(i).gameObject);
                }
                _mPlayPhasesControl._OnPhaseFinished();
            });
        }
    }
}
