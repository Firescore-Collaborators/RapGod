using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField]
        private RuntimeAnimatorController playerAnimatorController, enemyAnimatorController;
        [SerializeField]
        private float animTransitionTime = 0.1f;
        [SerializeField] private Text swipeInstructions;
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
        void OnEnable()
        {
            InitLevelData();
            Init();
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

        }

        void InitMultiTouchCallback()
        {
            multiTouchManager.inputSequence = handShakeSO.inputSequence;
            multiTouchManager.onInputAssignedWithType += InputAssinged;
            multiTouchManager.onInputRaised += CorrectInputResult;
            multiTouchManager.onMultiTaping += OnMultiTap;
            multiTouchManager.onSequenceComplete += LevelEnd;
            //multiTouchManager.onInputRaisedWithIndex += PlayAnim;
            multiTouchManager.Init();
        }

        void SpawnSequenceUI()
        {
            for (int i = 0; i < handShakeSO.inputSequence.inputSequence.Count; i++)
            {
                GameObject toSpawn = new GameObject();
                switch (handShakeSO.inputSequence.inputSequence[i])
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
                sequenceUI.Add(spawned);
            }
        }

        void CorrectInputResult()
        {
            if (isMultiTaping)
            {
                print("MultiTapEnd");
            }
            else
            {
                player.GetComponent<Animator>().CrossFade(handShakeSO.player[currentSequenceIndex].ToString(), animTransitionTime, 0, 0f);
                enemy.GetComponent<Animator>().CrossFade(handShakeSO.enemy[currentSequenceIndex].ToString(), animTransitionTime, 0, 0f);
            }


            ColorUI();
        }

        void OnMultiTap(float currentValue)
        {
            multiTapMeter.fillAmount = Remap.remap(currentValue, 0, handShakeSO.multiTapLimit, 0, 1);
        }

        void InputAssinged(TouchInputType type)
        {
            swipeInstructions.text = type.ToString();
            TouchDefaultState();
            switch (type)
            {
                case TouchInputType.multiTap:
                    multiTapMeter.transform.parent.gameObject.SetActive(true);
                    isMultiTaping = true;
                    break;
            }
        }

        void TouchDefaultState()
        {
            multiTapMeter.transform.parent.gameObject.SetActive(false);
            isMultiTaping = false;
        }
        void ColorUI()
        {
            sequenceUI[currentSequenceIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }

        void LevelEnd()
        {
            Debug.Log("Level End");
        }
    }
}
