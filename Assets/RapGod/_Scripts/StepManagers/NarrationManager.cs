using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

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
        [Expandable]
        [SerializeField]
        private NarrationSO narration;
        [SerializeField]
        private Text option1, option2;

        [SerializeField]
        private GameObject optionPanel;

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
            narration = level.GetNarrationSO;
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
            PlayDialogue(true);
        }

        public void PlayDialogue(bool positive)
        {
            optionPanel.SetActive(false);
            popUp.SetActive(false);

            Timer.Delay(1.5f, () =>
            {
                string currentResponse;
                string currentConversation = narration.default_conversation[conversation];
                option1.text = narration.positiveResponse[conversation];
                option2.text = narration.negativeResponse[conversation];

                if (conversation != 0)
                {
                    if (positive)
                    {
                        currentResponse = narration.positive_conversation[response];
                    }
                    else
                    {
                        currentResponse = narration.negetive_conversation[response];
                    }
                    if (currentResponse != string.Empty)
                    {
                        ShowDialogue(currentResponse, () =>
                        {
                            Timer.Delay(1.0f, () =>
                            {
                                popUp.SetActive(false);
                                Timer.Delay(1.5f, () =>
                                {
                                    ShowDefaultRespone(currentConversation);
                                });
                            });
                        });
                        response++;
                        return;
                    }
                    response++;
                }
                ShowDefaultRespone(currentConversation);
            });


        }

        void ShowDialogue(string text, System.Action afterRespone = null)
        {
            typewriter.WholeText = text;
            popUp.SetActive(true);
            typewriter.ShowTextResponse(afterRespone);
        }

        void ShowDefaultRespone(string text)
        {
            typewriter.WholeText = text;
            popUp.SetActive(true);
            typewriter.ShowTextResponse(() =>
            {
                optionPanel.SetActive(true);
            });
            conversation++;
        }


    }


}

