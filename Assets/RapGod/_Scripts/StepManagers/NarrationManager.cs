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
        private AudioSource narrationAudio;

        [SerializeField]
        private GameObject player, enemy;

        [SerializeField]
        private RuntimeAnimatorController playerAnimatorController, enemyAnimatorController;
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
            string currentConversation = string.Empty;
            optionPanel.SetActive(false);
            popUp.SetActive(false);

            Timer.Delay(1.5f, () =>
            {
                string currentResponse;
                //Checking if def converstaion is over
                if (conversation < narration.default_conversation.Length)
                {
                    currentConversation = narration.default_conversation[conversation];
                    //Assinging player response
                    option1.text = narration.positiveResponse[conversation];
                    option2.text = narration.negativeResponse[conversation];
                }

                if (conversation != 0)
                {
                    //Play player audio response
                    PlayAudio(positive ? GetAudioClip(narration.aud_positiveResponse,response) : GetAudioClip(narration.aud_negetiveResponse,response));
                    if (positive)
                    {
                        currentResponse = narration.positive_conversation[response];
                    }
                    else
                    {
                        currentResponse = narration.negetive_conversation[response];
                    }
                    //Check if response is available
                    if (currentResponse != string.Empty)
                    {
                        //Showing response dialogue
                        ShowDialogue(currentResponse, positive, () =>
                        {

                            Timer.Delay(1.0f, () =>
                            {
                                popUp.SetActive(false);
                                //Check if def conversation is over
                                if (conversation < narration.default_conversation.Length)
                                {
                                    Timer.Delay(1.0f, () =>
                                    {
                                        //Showing def coversation after response
                                        ShowDefaultRespone(currentConversation);
                                    });
                                }
                                else
                                    //If no converstaion and response is over
                                    LevelEnd();
                            });
                        });
                        response++;
                        //Returning from function if response is there
                        return;
                    }
                    response++;
                }
                //Print def conversation when no response is available
                if (conversation < narration.default_conversation.Length)
                    ShowDefaultRespone(currentConversation);
                else
                    LevelEnd();
            });

        }

        void ShowDialogue(string text, bool positive, System.Action afterRespone = null)
        {
            typewriter.WholeText = text;
            popUp.SetActive(true);
            typewriter.ShowTextResponse(afterRespone);
            //Play enemy response anim
            PlayAnim(positive ? narration.positive_anim[response] : narration.negetive_anim[response]);
            //Play audio response
            PlayAudio(positive ? GetAudioClip(narration.aud_positiveConv, response) : GetAudioClip(narration.aud_negetiveConv, response));
        }

        void ShowDefaultRespone(string text)
        {
            typewriter.WholeText = text;
            popUp.SetActive(true);
            typewriter.ShowTextResponse(() =>
            {
                optionPanel.SetActive(true);
            });
            //play enemy def anim
            PlayAnim(narration.default_anim[conversation]);
            //play audio def conv
            PlayAudio(GetAudioClip(narration.aud_defaultConv, conversation));
            conversation++;
        }

        void Reset()
        {
            conversation = 0;
            response = 0;
            Destroy(spawnPosition.playerPos.transform.GetChild(0).gameObject);
            Destroy(spawnPosition.enemyPos.transform.GetChild(0).gameObject);
        }

        void PlayAnim(NarrationAnimation anim)
        {
            switch (anim)
            {
                case NarrationAnimation.Talking1:
                    enemy.GetComponent<Animator>().CrossFade("Talking1", 0.1f);
                    break;
                case NarrationAnimation.Annoyed:
                    enemy.GetComponent<Animator>().CrossFade("Annoyed", 0.1f);
                    break;
                default:
                    enemy.GetComponent<Animator>().CrossFade("Idle", 0.1f);
                    break;
            }
        }

        void PlayAudio(AudioClip clip)
        {
            if (clip == null) return;

            if (narrationAudio.isPlaying)
                narrationAudio.Stop();

            narrationAudio.clip = clip;
            narrationAudio.Play();
        }

        AudioClip GetAudioClip(AudioClip[] clips, int index)
        {
            if (clips.Length == 0) return null;

            if (index >= clips.Length) { return null; }

            return clips[index];
        }

        void LevelEnd()
        {
            Reset();
            playPhasesControl._OnPhaseFinished();
        }
    }


}

