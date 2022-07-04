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
        }
        void Init()
        {
            spawnPosition = EnvironmentList.instance.GetEnvironment(rapEnvironmentType);
            player = characterList.SpawnCharacter(spawnPosition.playerPos);
            player.GetComponent<Animator>().runtimeAnimatorController = playerAnimatorController;
            enemy = Utils.spawnGameObject(rapBattleData.enemyCharacter, spawnPosition.enemyPos);
            enemy.GetComponent<Animator>().runtimeAnimatorController = enemyAnimatorController;
            InitMultiTouchCallback();
        }


        void InitMultiTouchCallback()
        {
            multiTouchManager.inputSequence = handShakeSO.inputSequence;
            multiTouchManager.onInputRaised += Print;
            multiTouchManager.onSequenceComplete += LevelEnd;
            multiTouchManager.onInputAssignedWithType += InputAssinged;
            multiTouchManager.onInputRaisedWithIndex += PlayAnim;
            multiTouchManager.Init();
        }

        void PlayAnim(int index)
        {
            player.GetComponent<Animator>().CrossFade(handShakeSO.player[index].ToString(), animTransitionTime, 0, 0f);
            enemy.GetComponent<Animator>().CrossFade(handShakeSO.enemy[index].ToString(), animTransitionTime, 0, 0f);
        }

        void InputAssinged(TouchInputType type)
        {
            swipeInstructions.text = type.ToString();
        }

        void Print()
        {
            Debug.Log("Correct Input");
        }

        void LevelEnd()
        {
            Debug.Log("Level End");
        }
    }
}
