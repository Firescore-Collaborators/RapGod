using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class NarrationManager : MonoBehaviour
    {
        [SerializeField]
        private CharacterListSO characterList;
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
        private NarrationSO narration;

        GameObject popUp;
        TypewriterEffect typewriter;

        int conversation = 0;
        int response = 0;

        void OnEnable()
        {
            InitLevelData();
            Init();
        }

        void OnDisable()
        {

        }

        void InitLevelData()
        {
            Level_SO level = playPhasesControl.levels[Progress.Instance.CurrentLevel - 1];
            rapBattleData = level.GetRapBattleSO;
            rapEnvironmentType = level.GetRapBattleSO.environment.envType;
        }

        void Init()
        {
            spawnPosition = EnvironmentList.instance.GetEnvironment(rapEnvironmentType);
            player = characterList.SpawnCharacter(spawnPosition.playerPos);
            player.GetComponent<Animator>().runtimeAnimatorController = playerAnimatorController;
            enemy = Utils.spawnGameObject(rapBattleData.enemyCharacter, spawnPosition.enemyPos);
            enemy.GetComponent<Animator>().runtimeAnimatorController = enemyAnimatorController;
            popUp = EnvironmentList.instance.GetCurrentEnvironment.popUp;
            typewriter = popUp.transform.GetChild(0).GetComponent<TypewriterEffect>();
        }

        void PlayDialogue()
        {
            if (conversation != 0)
            {

            }
            //typewriter.ShowTextResponse();
            popUp.SetActive(true);
        }

    }
}

