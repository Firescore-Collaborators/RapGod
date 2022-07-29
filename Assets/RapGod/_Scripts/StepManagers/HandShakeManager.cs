using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace PrisonControl
{
    [RequireComponent(typeof(TouchInputs))]
    [RequireComponent(typeof(MultiTouchManager))]
    public class HandShakeManager : MonoBehaviour
    {
        [SerializeField]
        private CharacterListSO characterList;
        [SerializeField]
        private HandShakeSO handShakeSO;
        [SerializeField]
        private InputSequenceUISO inputSequenceUI;
        [Foldout("SwipeFTUE")]
        [SerializeField]
        private List<GameObject> swipeFTUE = new List<GameObject>();

        [SerializeField]
        private MultiTouchManager multiTouchManager
        {
            get
            {
                return GetComponent<MultiTouchManager>();
            }
        }
        [SerializeField]
        private PlayPhasesControl playPhasesControl;
        [SerializeField]
        private RapEnvironmentType rapEnvironmentType;
        [SerializeField]
        private RapBattleDataSO rapBattleData;
        private SpawnPosition spawnPosition;
        [SerializeField]
        private GameObject player, enemy;
        GameObject toSpawn;
        AnimationClip[] playerClip;
        [SerializeField]
        private RuntimeAnimatorController playerAnimatorController, enemyAnimatorController;
        [SerializeField]
        private float animTransitionTime = 0.1f;
        private int currentInstructionType;
        [SerializeField] private Image multiTapMeter;
        [SerializeField] private Transform inputSequenceUIPanel;
        private List<GameObject> sequenceUI = new List<GameObject>();
        bool isMultiTaping = false;
        int currentSequenceIndex
        {
            get
            {
                return multiTouchManager.currentSequenceIndex;
            }
        }
        GameObject currentSequenceUI;


        void OnEnable()
        {
            InitLevelData();
            Init();
            InitAnimatorClips();
        }

        void InitLevelData()
        {
            Level_SO level = playPhasesControl.levels[Progress.Instance.CurrentLevel - 1];
            rapBattleData = level.GetRapBattleSO;
            rapEnvironmentType = level.GetRapBattleSO.environment.envType;
            handShakeSO = level.GetHandShakeSO;
            GetComponent<TouchInputs>().multiTapLimit = handShakeSO.multiTapLimit;
        }
        void Init()
        {
            spawnPosition = EnvironmentList.instance.GetEnvironment(rapEnvironmentType);
            player = characterList.SpawnCharacter(spawnPosition.playerPos);
            player.GetComponent<Animator>().runtimeAnimatorController = playerAnimatorController;
            enemy = Utils.spawnGameObject(rapBattleData.enemyCharacter, spawnPosition.enemyPos);
            enemy.GetComponent<Animator>().runtimeAnimatorController = enemyAnimatorController;
            InitMultiTouchCallback();
            SpawnSequenceUI();
            swipeFTUE[currentInstructionType].SetActive(true);
        }

        void InitMultiTouchCallback()
        {
            multiTouchManager.onSequenceComplete += null;
            multiTouchManager.inputSequence = handShakeSO.inputSequence;
            multiTouchManager.onInputAssignedWithType += InputAssinged;
            multiTouchManager.onInputRaised += CorrectInputResult;
            multiTouchManager.onMultiTaping += OnMultiTap;
            multiTouchManager.onSequenceComplete += LevelEndAnim;
            //multiTouchManager.onInputRaisedWithIndex += PlayAnim;
            multiTouchManager.Init();
        }

        void Reset()
        {
            for (int i = 0; i < inputSequenceUIPanel.childCount; i++)
            {
                Destroy(inputSequenceUIPanel.GetChild(i).gameObject);
            }
            sequenceUI.Clear();
            Destroy(player);
            Destroy(enemy);
            multiTapMeter.fillAmount = 0;

        }

        void SpawnSequenceUI()
        {
            switch (handShakeSO.inputSequence.inputSequence[currentSequenceIndex])
            {
                case TouchInputType.swipeLeft:
                    toSpawn = inputSequenceUI.leftArrow;
                    break;
                case TouchInputType.swipeRight:
                    toSpawn = inputSequenceUI.rightArrow;
                    break;
                case TouchInputType.swipeUp:
                    toSpawn = inputSequenceUI.upArrow;
                    break;
                case TouchInputType.swipeDown:
                    toSpawn = inputSequenceUI.downArrow;
                    break;
                case TouchInputType.multiTap:
                    toSpawn = inputSequenceUI.multiTap;
                    break;
            }
            GameObject spawned = Instantiate(toSpawn, inputSequenceUIPanel);
            spawned.transform.parent = inputSequenceUIPanel.transform;
            sequenceUI.Add(spawned);
        }

        void CorrectInputResult()
        {
            if (isMultiTaping)
            {
                print("MultiTapOver");
            }
            else
            {
                player.GetComponent<Animator>().CrossFade(handShakeSO.player[currentSequenceIndex].ToString(), animTransitionTime, 0, 0f);
                enemy.GetComponent<Animator>().CrossFade(handShakeSO.enemy[currentSequenceIndex].ToString(), animTransitionTime, 0, 0f);
                AnimatorStateInfo playerState = player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                Timer.Delay(GetClipTime(handShakeSO.player[currentSequenceIndex].ToString()), () =>
                {
                    print("AnimationEnd");
                });
                OnAnimationPlay(GetClipTime(handShakeSO.player[currentSequenceIndex].ToString()));

                Destroy(inputSequenceUIPanel.transform.GetChild(0).gameObject);
            }
            //ColorUI();
        }

        void InitAnimatorClips()
        {
            playerClip = playerAnimatorController.animationClips;
        }

        float GetClipTime(string clipName)
        {
            for (int i = 0; i < playerClip.Length; i++)
            {
                if (playerClip[i].name == clipName)
                {
                    return playerClip[i].length;
                }
            }
            return 0;
        }
        void OnMultiTap(float currentValue, bool tapped)
        {
            multiTapMeter.fillAmount = Remap.remap(currentValue, 0, handShakeSO.multiTapLimit, 0, 1);
            if (tapped)
            {
                player.GetComponent<Animator>().Play(handShakeSO.player[currentSequenceIndex].ToString());
                enemy.GetComponent<Animator>().Play(handShakeSO.enemy[currentSequenceIndex].ToString());
            }
            float animationSpeed = Remap.remap(currentValue, 0, handShakeSO.multiTapLimit, 1, handShakeSO.animationMaxSpeed);
            player.GetComponent<Animator>().speed = animationSpeed;
            enemy.GetComponent<Animator>().speed = animationSpeed;
        }

        void InputAssinged(TouchInputType type)
        {
            currentInstructionType = (int)type;
            TouchDefaultState();
            switch (type)
            {
                case TouchInputType.multiTap:
                    Timer.Delay(GetClipTime(handShakeSO.player[currentSequenceIndex].ToString()), () =>
                    {
                        multiTapMeter.transform.parent.gameObject.SetActive(true);
                        isMultiTaping = true;
                    });
                    break;
            }
        }

        void TouchDefaultState()
        {
            multiTapMeter.transform.parent.gameObject.SetActive(false);
            isMultiTaping = false;
            player.GetComponent<Animator>().speed = 1f;
            enemy.GetComponent<Animator>().speed = 1f;
        }
        void ColorUI()
        {
            sequenceUI[currentSequenceIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }

        void SetInputStatus(bool status)
        {
            GetComponent<TouchInputs>().enabled = status;
        }

        void OnAnimationPlay(float activateTime)
        {
            //swipeInstructions.gameObject.SetActive(false);
            ResetSwipe();
            SetInputStatus(false);

            Timer.Delay(activateTime, () =>
            {
                //swipeInstructions.gameObject.SetActive(true);
                if (currentInstructionType != 4)
                {
                    swipeFTUE[currentInstructionType].SetActive(true);
                }
                SetInputStatus(true);
                SpawnSequenceUI();
            });
        }

        void ResetSwipe()
        {
            for (int i = 0; i < swipeFTUE.Count; i++)
            {
                swipeFTUE[i].SetActive(false);
            }
        }

        void LevelEndAnim()
        {
            multiTapMeter.transform.parent.gameObject.SetActive(false);
            for (int i = 0; i < inputSequenceUIPanel.childCount; i++)
            {
                Destroy(inputSequenceUIPanel.GetChild(i).gameObject);
            }
            Timer.Delay(1f, () =>
            {
                player.GetComponent<Animator>().speed = 1;
                enemy.GetComponent<Animator>().speed = 1;
                player.GetComponent<Animator>().CrossFade(handShakeSO.handShakeEnd.playerEnd.ToString(), animTransitionTime, 0, 0f);
                enemy.GetComponent<Animator>().CrossFade(handShakeSO.handShakeEnd.enemyEnd.ToString(), animTransitionTime, 0, 0f);
                Timer.Delay(GetClipTime(handShakeSO.handShakeEnd.playerEnd.ToString()), () =>
                {
                    LevelEnd();
                });
            });

        }
        void LevelEnd()
        {
            Timer.Delay(1f, () =>
            {
                playPhasesControl._OnPhaseFinished();
                Reset();
            });
        }
    }
}
