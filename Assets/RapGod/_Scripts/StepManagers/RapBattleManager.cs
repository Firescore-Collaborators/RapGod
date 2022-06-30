using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace PrisonControl

{
    public class RapBattleManager : MonoBehaviour
    {

        public RapEnvironmentType environmentType;
        [SerializeField]
        private CharacterListSO characterList;
        [SerializeField]
        private SpawnPosition spawnPosition;

        [SerializeField]
        private RuntimeAnimatorController playerAnimator, enemyAnimator;

        [SerializeField]
        GameObject player, enemy;

        public PlayPhasesControl m_playPhaseControl;

        [SerializeField]
        private GameObject OptionPanle, PopUpPanle, winnerConfitti, initialPopUpPos;

        [SerializeField]
        public Text remarks;

        [SerializeField]
        private TMP_Text optionA_text, optionB_text;

        [SerializeField]
        private Color[] colors;

        List<char> rapChar = new List<char>();

        [SerializeField]
        private TMP_Text popUp_text, DummyText;

        [SerializeField]
        private RectTransform PopUP_rect;

        [SerializeField]
        private AudienceManager audienceManager;

        [SerializeField]
        private Animator player_anim, enemy_anim, ui_anim;

        [SerializeField]
        private GameObject[] textAnswer_anim;

        [SerializeField]
        private Sprite[] sprites_btn;

        [SerializeField]
        private GameObject Conffiti_btn, ConffitiWrong_btn;

        [SerializeField]
        private Image[] hand;

        [SerializeField]
        private Sprite[] popUp_sprite;

        [SerializeField]
        private GameObject homePanle, rewardPanle;

        public RapBattleDataSO rapData;

        // [SerializeField]
        // private Color[] FogColor, lightColor, planeColor;

        [SerializeField]
        private MeshRenderer lightLObj, lightRObj, planeObj, stageObj, stageLightObj;

        int levelNo;
        int textNo;
        int rapWordNo;
        bool print = false;
        float duration, durationTarget;
        int sideNo;
        String answer;
        bool removedLetter;
        Vector3 popUpPos;

        private void OnEnable()
        {
            InitLevelData();
            Init();
            //int no = PlayerPrefs.GetInt("LevelNo", 1);
            //int fogNo = (no - 1) % FogColor.Length;
            //RenderSettings.fogColor = FogColor[fogNo];
            //lightLObj.material.SetColor("_TintColor", lightColor[fogNo]);
            //lightRObj.material.SetColor("_TintColor", lightColor[fogNo]);
            //planeObj.material.color = planeColor[fogNo];
            //stageObj.material.color = planeColor[fogNo];
            //stageLightObj.material.color = planeColor[fogNo];

            popUpPos = PopUP_rect.position;
            //levelNo_text.text = "Level " + no;
            homePanle.SetActive(true);
            rewardPanle.SetActive(false);
            OnNextRapWord();
        }

        void Init()
        {
            // player = characterList.SpawnCharacter(playerPos);

            // enemy = Utils.spawnGameObject(enemy, enemyPos);
            // enemy_anim = enemy.GetComponent<Animator>();
            // 

            spawnPosition = EnvironmentList.instance.GetEnvironment(environmentType);
            player = characterList.SpawnCharacter(spawnPosition.playerPos);
            player.GetComponent<Animator>().runtimeAnimatorController = playerAnimator;
            player_anim = player.GetComponent<Animator>();
            enemy = Utils.spawnGameObject(rapData.enemyCharacter, spawnPosition.enemyPos);
            enemy_anim = enemy.GetComponent<Animator>();
            enemy_anim.runtimeAnimatorController = enemyAnimator;
            audienceManager.EnemyHeadTarget = enemy;
            audienceManager.PlayerHeadTarget = player;
        }

        void InitLevelData()
        {
            Level_SO level = m_playPhaseControl.levels[Progress.Instance.CurrentLevel-1];
            rapData = level.GetRapBattleSO;
            environmentType = level.GetRapBattleSO.environment.envType;
        }
        public void OnNextRapWord()
        {
            player_anim.SetBool("Idle", true);
            PopUP_rect.gameObject.GetComponent<Image>().sprite = popUp_sprite[0];

            PopUP_rect.transform.position = initialPopUpPos.transform.position;

            levelNo = PlayerPrefs.GetInt("Level", 0);
            popUp_text.text = "" + rapData.rapBattleLyricSO.leveldata[rapWordNo].RapString;
            DummyText.text = "<#161616>" + rapData.rapBattleLyricSO.leveldata[rapWordNo].RapString + "</color>";

            MakeColorText(DummyText, rapData.rapBattleLyricSO.leveldata[rapWordNo].MarkWord);
            PopUpPanle.SetActive(true);
        }

        void MakeColorText(TMP_Text _text, string _word)
        {
            _text.text = _text.text.Replace(_word, "<#01FF00>" + _word + " </color>");
        }

        public void OnRequestShowOption()
        {
            StartCoroutine("ShowOption");
        }

        IEnumerator ShowOption()
        {
            optionA_text.text = rapData.rapBattleLyricSO.leveldata[rapWordNo].OptionA.ToString();
            optionB_text.text = rapData.rapBattleLyricSO.leveldata[rapWordNo].OptionB.ToString();

            answer = "<#161616>" + "______" + "</color>";

            char[] TemprapChar = popUp_text.text.ToCharArray();
            // remaningtext = popUp_text.text;
            for (int i = 0; i < TemprapChar.Length; i++)
            {
                rapChar.Add(TemprapChar[i]);
            }


            OptionPanle.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            OptionPanle.transform.GetChild(0).gameObject.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            OptionPanle.transform.GetChild(1).gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);


        }



        private void Update()
        {

            //if (Input.GetMouseButtonDown(1))
            //{
            //    hand[0].gameObject.SetActive(true);

            //}

            //if (Input.GetMouseButtonDown(0))
            //{
            //    hand[0].gameObject.SetActive(false);
            //    hand[1].gameObject.SetActive(true);

            //}

            //if (Input.GetMouseButtonUp(0))
            //{
            //    hand[1].gameObject.SetActive(false);

            //}

            //Vector2 mousePosition = Input.mousePosition;
            //hand[0].transform.position = mousePosition;
            //hand[1].transform.position = mousePosition;



            if (print && textNo < rapChar.Count)
            {
                duration += Time.deltaTime;
                if (duration > 0.1f)
                {
                    char letter = rapChar[textNo];
                    String Tempremaningtext = "";
                    String TempText = "";


                    for (int i = 0; i < rapChar.Count; i++)
                    {
                        if (rapChar[i] == '_')
                        {
                            if (i < textNo)
                            {
                                TempText = TempText + answer;
                                i = i + 6;
                            }
                            else
                            {
                                Tempremaningtext = Tempremaningtext + answer;
                                i = i + 6;
                            }


                        }

                        if (i > textNo)
                        {
                            Tempremaningtext = (Tempremaningtext + rapChar[i]).ToString();
                        }
                        else
                        {
                            TempText = (TempText + rapChar[i]).ToString();
                        }
                    }

                    if (letter == '_' && !removedLetter)
                    {
                        DummyText.text = "<#6F6F6F>" + TempText + "</color>" + " <#161616>" + Tempremaningtext + " </color>";
                        textNo = textNo + 6;
                        removedLetter = true;
                    }
                    else if (letter != '_')
                    {
                        DummyText.text = "<#6F6F6F>" + TempText + "</color>" + " <#161616>" + Tempremaningtext + " </color>";
                    }

                    if (textNo == rapChar.Count - 1)
                    {
                        print = false;
                        if (sideNo == 1)
                        {

                            audienceManager.OnMovePlayerSide(0);
                        }
                        else
                        {

                            audienceManager.OnMoveEnemySide(0);
                        }
                        Invoke("OnReset", 1f);
                    }

                    if (letter != '_')
                    {
                        duration = 0;
                        textNo++;
                    }
                }
            }
        }

        public void OnSelectSide(int _sideNo)
        {
            int i = rapData.rapBattleLyricSO.leveldata[rapWordNo].CorrectAnswer;

            if (i == 1)
            {
                sideNo = _sideNo;
            }
            else
            {
                if (_sideNo == 1)
                {
                    sideNo = 2;
                }
                else
                {
                    sideNo = 1;
                }
            }

        }

        public void OnSelectButton(TMP_Text _Answer)
        {
            if (sideNo == 1)
            {
                _Answer.transform.parent.gameObject.GetComponent<Image>().sprite = sprites_btn[0];
                //Conffiti_btn.SetActive(true);
            }
            else
            {
                //  ConffitiWrong_btn.SetActive(true);
                _Answer.transform.parent.gameObject.GetComponent<Image>().sprite = sprites_btn[1];
            }
        }

        public void OnSelectAnswer(TMP_Text _Answer)
        {



            Conffiti_btn.SetActive(false);
            ConffitiWrong_btn.SetActive(false);
            // _Answer.gameObject.SetActive(false);
            //  textAnswer_anim[LevelNo].gameObject.SetActive(true);
            StartCoroutine(CheckAnswer(_Answer));
        }

        IEnumerator CheckAnswer(TMP_Text _Answer)
        {

            LeanTween.move(PopUP_rect.gameObject, popUpPos, 1f);

            if (sideNo == 1)
            {
                DummyText.text = DummyText.text.Replace("______", "<#01FF00>" + _Answer.text + " </color>");
                //_Answer.transform.parent.gameObject.GetComponent<Image>().sprite = sprites_btn[0];
                Conffiti_btn.SetActive(true);
            }
            else
            {

                DummyText.text = DummyText.text.Replace("______", "<#FF0000>" + _Answer.text + " </color>");
                ConffitiWrong_btn.SetActive(true);
                // _Answer.transform.parent.gameObject.GetComponent<Image>().sprite = sprites_btn[1];
            }
            yield return new WaitForSeconds(0.5f);
            PopUP_rect.gameObject.GetComponent<Image>().sprite = popUp_sprite[1];
            yield return new WaitForSeconds(1.2f);
            print = false;
            player_anim.SetBool("Idle", false);
            // string tempText = popUp_text.text;
            if (sideNo == 1)
            {
                //  tempText = tempText.Replace("______", "<#01FF00>" + _Answer.text + " </color>");
                answer = "<#01FF00>" + _Answer.text + " </color>";
                // audienceManager.OnMovePlayerSide(_Answer.text.Length);
            }
            else
            {
                //   
                answer = "<#FF0000>" + _Answer.text + " </color>";
                //audienceManager.OnMoveEnemySide(_Answer.text.Length);
            }
            print = true;


            // _anim.SetBool("OnClicked",true);
            yield return new WaitForSeconds(0.7f);
            textAnswer_anim[levelNo].gameObject.SetActive(false);
            OptionPanle.SetActive(false);
            _Answer.gameObject.SetActive(true);
            OptionPanle.transform.GetChild(0).gameObject.SetActive(false);
            OptionPanle.transform.GetChild(1).gameObject.SetActive(false);
            StopCoroutine("WaitAndPrint");


        }

        public void OnReset()
        {

            rapWordNo++;
            winnerConfitti.SetActive(false);
            OptionPanle.SetActive(false);
            OptionPanle.transform.GetChild(0).gameObject.SetActive(false);
            OptionPanle.transform.GetChild(1).gameObject.SetActive(false);

            OptionPanle.transform.GetChild(0).GetComponent<Image>().sprite = sprites_btn[2];
            OptionPanle.transform.GetChild(1).GetComponent<Image>().sprite = sprites_btn[2];

            sideNo = 0;
            rapChar.Clear();
            print = false;
            removedLetter = false;
            textNo = 0;
            PopUpPanle.SetActive(false);
            

            if (rapData.rapBattleLyricSO.leveldata.Count <= rapWordNo)
            {
                rapWordNo = 0;

                /*int no = PlayerPrefs.GetInt("LevelNo", 1);
                no++;
                PlayerPrefs.SetInt("LevelNo", no);

                levelNo = PlayerPrefs.GetInt("Level", 0);
                levelNo++;

                if (levelNo >= levels.Count)
                {
                    levelNo = 0;
                }
                PlayerPrefs.SetInt("Level", levelNo);
                */

                if (!audienceManager.CheckWinner())
                {

                }
                else
                {
                    ui_anim.enabled = true;

                    if (audienceManager.playerFollower > audienceManager.enemyFollower)
                    {
                        ui_anim.SetTrigger("win");
                        winnerConfitti.SetActive(true);
                        remarks.color = Color.green;
                        remarks.text = "WINNER!";
                        remarks.gameObject.SetActive(true);
                        player_anim.SetBool("win", true);
                        enemy_anim.SetBool("loose", true);
                    }
                    else
                    {
                        ui_anim.SetTrigger("loose");
                        remarks.text = "LOSER!";
                        remarks.color = Color.red;
                        remarks.gameObject.SetActive(true);
                        player_anim.SetBool("loose", true);
                        enemy_anim.SetBool("win", true);
                    }
                }

                Invoke("OnLevelEnd", 6f);
            }
            else
            {
                OnNextRapWord();
            }
        }


        void OnLevelEnd()
        {
            // homePanle.SetActive(false);
            // rewardPanle.SetActive(true);
            LevelReset();
            m_playPhaseControl._OnPhaseFinished();
        }

        void LevelReset()
        {
            EnvironmentList.instance.SwitchOffEnvironment();
            Destroy(spawnPosition.enemyPos.transform.GetChild(0).gameObject);
            Destroy(spawnPosition.playerPos.transform.GetChild(0).gameObject);
        }

        // public void OnNextLevelLoad()
        // {
        //     Application.LoadLevel(0);
        // }
    }
}
