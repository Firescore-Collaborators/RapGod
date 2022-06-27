using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PrisonControl
{
    public class FoodPlatingStep : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private GamePlayStep _mGamePlayStep;

        [SerializeField]
        private CameraManager cameraManager;

        [SerializeField]
        private FoodPlatingUi foodPlatingUi;

        [SerializeField]
        private GameObject pf_lerpObject;

        [SerializeField]
        private Transform[] queuePos;

        [SerializeField]
        private Transform standPos, exitPos;

        private float lerpSpeed, rotLerpSpeed;
        private int currentPrisoner;
        private List<GameObject> prisoners;

        GameObject currentPlate;

        [SerializeField]
        private int selectedFood;

        private bool readyForPlating;

        // Raycaster
        [SerializeField]
        private Transform raycaster;
        public Vector2 startPos;
        Ray ray;
        RaycastHit hit;
        float heldTimer = 0;

        [SerializeField]
        private GameObject [] pf_foodItems;

        [SerializeField]
        private Transform foodParent;

        private List<int> listItemDropped;

        [SerializeField]
        private GameObject tutorialHand;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        RespondMessage respondMsg;

        FoodPlating_SO current_foodPlating_SO;

        [SerializeField]
        private GameObject btn_continue;

        void Awake()
        {
            lerpSpeed = 1f;
            rotLerpSpeed = .3f;
            prisoners = new List<GameObject>();
            listItemDropped = new List<int>();

            foodPlatingUi._OnDoneClicked += OnDoneClicked;
            foodPlatingUi._OnFoodSelected += OnFoodSelected;
            foodPlatingUi._OnResponseClicked += OnResponseClicked;

            //foodPlatingUi._OnRequestShown += _OnRequestShown;
            //foodPlatingUi._OnResponseShown += _OnResponseShown;


            InputManager.MouseDragStarted += OnClick;
            InputManager.MouseDragged += OnDrag;
            InputManager.MouseDragEnded += OnClickEnd;
        }

        private void OnDestroy()
        {
            foodPlatingUi._OnDoneClicked -= OnDoneClicked;
            foodPlatingUi._OnFoodSelected -= OnFoodSelected;
            foodPlatingUi._OnResponseClicked -= OnResponseClicked;


            InputManager.MouseDragStarted -= OnClick;
            InputManager.MouseDragged -= OnDrag;
            InputManager.MouseDragEnded -= OnClickEnd;
        }

        private void OnEnable()
        {
            currentPrisoner = 0;
            StartCoroutine(Entry());
        }

        IEnumerator Entry()
        {
            if (currentPlate)
                Destroy(currentPlate);

            SpawnPrisoner();

            yield return new WaitForSeconds(0.5f);

            currentPrisoner++;
            StepAhead(() => {
                OnReached();
            });
        }

        void SpawnPrisoner()
        {
            for (int i = 0; i < 3; i++)
            {
                FoodPlating_SO foodPlating_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetFoodPlatingInfo[i];
                GameObject obj = Instantiate(foodPlating_SO.prisoner.pf_character, queuePos[i].position, Quaternion.Euler(0, -90, 0), transform);
                prisoners.Add(obj);
                obj.GetComponent<Animator>().applyRootMotion = true;
                obj.GetComponent<Animator>().Play("Idle_jailEntry");
            }
        }

        void StepAhead(System.Action callback)
        {
            prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("walk");

            LerpObjectPosition2.instance.LerpObject(prisoners[currentPrisoner - 1].transform, standPos.position, lerpSpeed, () =>
            {
                callback.Invoke();
            }
            );
        }

        void OnReached()
        {
            listItemDropped.Clear();

            prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("idle");

            if (currentPlate)
                Destroy(currentPlate);

            currentPlate = Instantiate(_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetFoodPlatingInfo[currentPrisoner - 1].pf_plate, transform);

            Timer.Delay(0.5f, () =>
            {
                readyForPlating = true;
            });

            current_foodPlating_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetFoodPlatingInfo[currentPrisoner - 1];
            foodPlatingUi.ActivateRequestPopup(true, current_foodPlating_SO.foodRequest, current_foodPlating_SO.aud_foodReq, () =>
            {
                Timer.Delay(1, () =>
                {
                    foodPlatingUi.ActivateRequestPopup(false);
                    StartPlating(true);
                });
            });
        }

        void _OnRequestShown()
        {
           
        }

        void StartPlating(bool showTut)
        {
            Debug.Log("selected food " + _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetFoodPlatingInfo[currentPrisoner - 1].list_displayFoodItems[0]);

            selectedFood = (int)_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetFoodPlatingInfo[currentPrisoner - 1].list_displayFoodItems[0];

            foodPlatingUi.ActivateFoodPanel(true);
            foodPlatingUi.SetUi(currentPrisoner - 1);

            cameraManager.ActivateCamFoodPlating2();
            readyForPlating = true;

            if(showTut)
                tutorialHand.SetActive(true);

            if (!Progress.Instance.IsfoodPlatingTutDone)
            {
                btn_continue.SetActive(false);
            }
        }

        void OnDoneClicked()
        {
            tutorialHand.SetActive(false);

            cameraManager.ActivateCamFoodPlating1(0.5f);

            if (Check(current_foodPlating_SO))
            {
                foodPlatingUi.ActivateResponsePanel(true, current_foodPlating_SO.positiveResponseBtn, current_foodPlating_SO.negetiveResponseBtn, current_foodPlating_SO.positiveResponse, false, current_foodPlating_SO.aud_posResponse, () =>
                {
                    Timer.Delay(1, () =>
                    {
                        NextPrisoner();
                        foodPlatingUi.CloseResponsePanel();
                    });
                });
            }
            else
            {
                foodPlatingUi.ActivateResponsePanel(true, current_foodPlating_SO.positiveResponseBtn, current_foodPlating_SO.negetiveResponseBtn, current_foodPlating_SO.negetiveResponse, true, current_foodPlating_SO.aud_negResponse);
            }
        }

        bool Check(FoodPlating_SO info)
        {
            for (int i = 0; i < info.list_requestFoodItems.Count; i++)
            {
                if (!listItemDropped.Contains((int)info.list_requestFoodItems[i]))
                {
                    return false;
                }
            }
            return true;
        }

        bool Check2(int selectedFood)
        {
            for (int i = 0; i < current_foodPlating_SO.list_requestFoodItems.Count; i++)
            {
                if ((int)current_foodPlating_SO.list_requestFoodItems[i] == selectedFood)
                {
                    return true;

                }
            }
            return false;
        }

        void NextPrisoner()
        {
            currentPlate.GetComponent<Animator>().SetTrigger("out");

            StepOutScanning(() => {

                if (currentPrisoner < 3)
                {
                    currentPrisoner++;
                    StepAhead(() => {
                        OnReached();
                    });
                }
                else
                {
                    EndLevel();
                }
            });
        }

        void StepOutScanning(System.Action callback)
        {
            LerpObjectRotation.instance.LerpObject(prisoners[currentPrisoner - 1].transform,
                Quaternion.Euler(new Vector3(0, prisoners[currentPrisoner - 1].transform.rotation.eulerAngles.y + 90f, 0)), rotLerpSpeed, () => {
                    LerpPassPosition(callback);
                });
        }

        void LerpPassPosition(System.Action callback)
        {
            prisoners[currentPrisoner - 1].GetComponent<Animator>().SetTrigger("walk");

            LerpObjectPosition.instance.LerpObject(prisoners[currentPrisoner - 1].transform, exitPos.position, lerpSpeed, () => {

                Debug.Log("prisoner out");
                callback.Invoke();
            });
        }

        void OnFoodSelected(int foodIndex)
        {
            selectedFood = foodIndex;
            Debug.Log("------ index is " + foodIndex);
        }

        void OnResponseClicked(bool responseType){

            // If response is +ve show panel again
            if (responseType)
            {
                foodPlatingUi.ActivateResponsePanel(false, "", "", "", false, null);
                NextPrisoner();
            }
            // If response is -ve next customer
            else
            {
                if (currentPrisoner == 3)
                {
                    Progress.Instance.WasBadDecision = true;
                }
                StartPlating(false);
            }
        }

        void OnClick(Vector2 pos)
        {
            if (!readyForPlating)
                return;

            tutorialHand.SetActive(false);

            if (!Progress.Instance.IsfoodPlatingTutDone)
            {
                Progress.Instance.IsfoodPlatingTutDone = true;

                btn_continue.SetActive(true);
            }

            heldTimer = 0;
        }

        void OnDrag(Vector2 pos)
        {
            if (!readyForPlating)
                return;

            heldTimer += Time.deltaTime;
            if (heldTimer < 0.1f)
            {
                return;
            }

            heldTimer = 0;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("plate") || hit.collider.gameObject.CompareTag("food"))
                {
                    if (!listItemDropped.Contains(selectedFood))
                    {
                        listItemDropped.Add(selectedFood);

                        if (Check2(selectedFood))
                        {
                        //    respondMsg.ShowCorrectMsg();
                        }
                    }
                    Instantiate(pf_foodItems[selectedFood], new Vector3(hit.point.x, 1.5f, hit.point.z), pf_foodItems[selectedFood].transform.rotation, currentPlate.transform);

                    audioSource.Play();
                }
            }
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

        void OnClickEnd(Vector2 pos)
        {
            if (!readyForPlating)
                return;

            heldTimer = 0;
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

    }
}
