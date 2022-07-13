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
        private VFXSO vFXSO;
        [SerializeField]
        private SpawnPosition spawnPosition;

        [SerializeField]
        private RuntimeAnimatorController playerAnimator, enemyAnimator;

        [SerializeField]
        GameObject player, enemy, handUI, tapFx;

        public PlayPhasesControl m_playPhaseControl;
        HypeMeterFxController m_hypeMeterFxController
        {
            get
            {
                return GetComponent<HypeMeterFxController>();
            }
        }

        public AudioSource lyricsAudioSource, bgmAudioSource, sfx;

        [SerializeField]
        private GameObject OptionPanle, restartPanel, PopUpPanle, winnerConfitti, initialPopUpPos;

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
        private GameObject homePanle, rewardPanle, rapFinishPanel;

        public RapBattleDataSO rapData;
        public Transform tapSmashPanel;
        public Text punchline, hintText;
        public Image hypeMeter;
        public StepProgress stepProgress;
        MultiTouchManager multiTouchManager
        {
            get
            {
                return GetComponent<MultiTouchManager>();
            }
        }

        public RespondMessageController respondMessageController;
        Coroutine handCoroutine;
        // [SerializeField]
        // private Color[] FogColor, lightColor, planeColor;
        [NaughtyAttributes.Foldout("Audio")]
        public AudioClip audienceCorrect, audienceWrong, answerCorrect, answerWrong, applauseLoop;

        [SerializeField]
        private MeshRenderer lightLObj, lightRObj, planeObj, stageObj, stageLightObj;
        public static event System.Action onTapped;
        int levelNo;
        int textNo;
        int rapWordNo;
        int rapCameraIndex = 0;
        bool print = false;
        float duration, durationTarget;
        int currentCorrectCount;
        int currentWrongCount;
        int sideNo;
        String answer;
        bool removedLetter;
        Vector3 popUpPos;
        [SerializeField] int currentLyric;

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

        void OnDisable()
        {
            LevelReset();
        }

        void Init()
        {
            // player = characterList.SpawnCharacter(playerPos);

            // enemy = Utils.spawnGameObject(enemy, enemyPos);
            // enemy_anim = enemy.GetComponent<Animator>();
            // 
            //Spawn Character and Assing animator
            spawnPosition = EnvironmentList.instance.GetEnvironment(environmentType);
            player = characterList.SpawnCharacter(spawnPosition.playerPos);
            player.GetComponent<Animator>().runtimeAnimatorController = playerAnimator;
            player_anim = player.GetComponent<Animator>();
            enemy = Utils.spawnGameObject(rapData.enemyCharacter, spawnPosition.enemyPos);
            enemy_anim = enemy.GetComponent<Animator>();
            enemy_anim.runtimeAnimatorController = enemyAnimator;

            //Get Audience reference
            audienceManager = EnvironmentList.instance.GetAudienceManager;
            audienceManager.EnemyHeadTarget = enemy;
            audienceManager.PlayerHeadTarget = player;

            //Set BGM
            bgmAudioSource.clip = rapData.rapBattleLyricSO.bgm;
            bgmAudioSource.Play();

            stepProgress.gameObject.SetActive(true);
        }

        void InitLevelData()
        {
            Level_SO level = m_playPhaseControl.levels[Progress.Instance.CurrentLevel - 1];
            rapData = level.GetRapBattleSO;
            environmentType = level.GetRapBattleSO.environment.envType;
            multiTouchManager.inputSequence = level.GetRapBattleSO.inputSequence;
            GetComponent<TouchInputs>().multiTapLimit = level.GetRapBattleSO.tapSmashLimit;
            stepProgress.Init(rapData.rapBattleLyricSO.leveldata.Count);
        }
        public void OnNextRapWord()
        {
            if (player_anim != null)
                player_anim.SetBool("Idle", true);

            PopUP_rect.gameObject.GetComponent<Image>().sprite = popUp_sprite[0];

            PopUP_rect.transform.position = initialPopUpPos.transform.position;

            levelNo = PlayerPrefs.GetInt("Level", 0);
            popUp_text.text = "" + rapData.rapBattleLyricSO.leveldata[rapWordNo].RapString;
            DummyText.text = "<#161616>" + rapData.rapBattleLyricSO.leveldata[rapWordNo].RapString + "</color>";
            hintText.text = rapData.rapBattleLyricSO.leveldata[rapWordNo].hintLine;
            Timer.Delay(1f, () =>
            {
                hintText.transform.parent.gameObject.SetActive(true);
            });
            MakeColorText(DummyText, rapData.rapBattleLyricSO.leveldata[rapWordNo].MarkWord);
            PopUpPanle.SetActive(true);
            stepProgress.ActivateCurrentStep();
        }

        void MakeColorText(TMP_Text _text, string _word)
        {
            _text.text = _text.text.Replace(_word, "<#0ec005>" + _word + " </color>");
        }

        public void OnRequestShowOption()
        {
            StartCoroutine("ShowOption");
        }

        IEnumerator ShowOption()
        {
            optionA_text.text = rapData.rapBattleLyricSO.leveldata[rapWordNo].OptionA.ToString();
            optionB_text.text = rapData.rapBattleLyricSO.leveldata[rapWordNo].OptionB.ToString();
            optionA_text.transform.parent.GetComponent<Button>().interactable = true;
            optionB_text.transform.parent.GetComponent<Button>().interactable = true;


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

        void NextLevel()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnReset();
                OnLevelEnd();
            }
        }

        private void Update()
        {
            NextLevel();
            if (Input.GetKeyDown(KeyCode.F1))
            {
                StartTapSmash();
            }
            PrintTextEffect();
        }

        void PrintTextEffect()
        {
            if (print && textNo < rapChar.Count)
            {
                duration += Time.deltaTime;
                if (duration > 0.05f)
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
                        stepProgress.UpdateStep(sideNo == 1 ? true : false);
                        if (sideNo == 1)
                        {
                            currentCorrectCount++;
                            audienceManager.OnMovePlayerSide(0);
                        }
                        else
                        {
                            currentWrongCount++;
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
                respondMessageController.ShowCorrectResponse();
                //PlayAudio(true);
                //Conffiti_btn.SetActive(true);
            }
            else
            {
                //  ConffitiWrong_btn.SetActive(true);
                _Answer.transform.parent.gameObject.GetComponent<Image>().sprite = sprites_btn[1];
                respondMessageController.ShowWrongResponse();
                //PlayAudio(false);
            }
        }

        public void OnSelectAnswer(TMP_Text _Answer)
        {
            _Answer.transform.parent.GetComponent<Button>().interactable = false;
            Conffiti_btn.SetActive(false);
            ConffitiWrong_btn.SetActive(false);
            // _Answer.gameObject.SetActive(false);
            //  textAnswer_anim[LevelNo].gameObject.SetActive(true);
            StartCoroutine(CheckAnswer(_Answer));
            PlayAudio(sideNo == 1 ? true : false);
            hintText.transform.parent.gameObject.SetActive(false);
            PlaySfx(sideNo == 1 ? answerCorrect : answerWrong);
        }

        IEnumerator CheckAnswer(TMP_Text _Answer)
        {

            LeanTween.move(PopUP_rect.gameObject, popUpPos, 1f);

            if (sideNo == 1)
            {
                DummyText.text = DummyText.text.Replace("______", "<#0ec005>" + _Answer.text + " </color>");
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
            //Rap Starts
            SetRapCamera();
            print = false;
            player_anim.SetBool("Idle", false);
            // string tempText = popUp_text.text;
            if (sideNo == 1)
            {
                //  tempText = tempText.Replace("______", "<#0ec005>" + _Answer.text + " </color>");
                answer = "<#0ec005>" + _Answer.text + " </color>";
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
            Utils.SpawnEfxWithDestroy(EnvironmentList.instance.GetCurrentEnvironment.multiTapFx[0], vFXSO.musicNote1, 3f);
            textAnswer_anim[levelNo].gameObject.SetActive(false);
            OptionPanle.SetActive(false);
            _Answer.gameObject.SetActive(true);
            OptionPanle.transform.GetChild(0).gameObject.SetActive(false);
            OptionPanle.transform.GetChild(1).gameObject.SetActive(false);
            Timer.Delay(1f, () =>
            {
                PlaySfx(sideNo == 1 ? audienceCorrect : audienceWrong);
            });


            StopCoroutine("WaitAndPrint");
        }

        void SetRapCamera()
        {
            if (rapCameraIndex >= rapData.rapCameras.Count)
            {
                EnvironmentList.instance.SetRapCamera(0);
                return;
            }
            print("RapCameraIndex : " + (int)rapData.rapCameras[rapCameraIndex] + " , rapCameraCount : " + rapData.rapCameras.Count);
            EnvironmentList.instance.SetRapCamera((int)rapData.rapCameras[rapCameraIndex]);
            rapCameraIndex++;
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
                bool playerWin = currentCorrectCount >= currentWrongCount ? true : false;
                if (!audienceManager.CheckWinner(playerWin))
                {

                }
                else
                {
                    ui_anim.enabled = true;

                    if (playerWin)
                    {
                        ui_anim.SetTrigger("win");
                        winnerConfitti.SetActive(true);
                        remarks.color = Color.green;
                        remarks.text = "WINNER!";
                        remarks.gameObject.SetActive(true);
                        player_anim.SetBool("win", true);
                        enemy_anim.SetBool("loose", true);
                        respondMessageController.ShowCorrectRespone("WINNER MENTALITY !");
                        Invoke("TapSmash", 1f);
                    }
                    else
                    {
                        ui_anim.SetTrigger("loose");
                        remarks.text = "LOSER!";
                        remarks.color = Color.red;
                        remarks.gameObject.SetActive(true);
                        player_anim.SetBool("loose", true);
                        enemy_anim.SetBool("win", true);
                        respondMessageController.ShowWrongResponse("NO HYPE");
                        Timer.Delay(3f, () =>
                        {
                            restartPanel.SetActive(true);
                        });

                    }
                }

            }
            else
            {
                OnNextRapWord();
            }
        }

        void StartTapSmash()
        {
            rapWordNo = 100;
            OnReset();
        }

        void TapSmash()
        {
            audienceManager.OnGameEnd();
            tapSmashPanel.gameObject.SetActive(true);
            stepProgress.gameObject.SetActive(false);
            multiTouchManager.onMultiTaping += OnMultiTap;
            multiTouchManager.onInputRaised += OnTapOver;
            multiTouchManager.Init();
            onTapped += Tapped;
            m_hypeMeterFxController.InitParticleAndCount();
            EnvironmentList.instance.SetRapCamera(0);
            handUI.SetActive(true);
            //Progress.Instance.TapSmashTut = true;
        }

        void OnMultiTap(float value, bool tapped)
        {
            hypeMeter.fillAmount = Remap.remap(value, 0, rapData.tapSmashLimit, 0, 1);
            player_anim.speed = Remap.remap(value, 0, rapData.tapSmashLimit, 1, rapData.maxAnimationSpeed);
            if (tapped)
            {
                onTapped?.Invoke();
                //Tapped();
            }
        }

        void Tapped()
        {
            //StartCoroutine(HandUIState(false, 1f));

            // if (handCoroutine != null)
            //     StopCoroutine(handCoroutine);

            //handUI.SetActive(false);
            player_anim.Play(rapData.rapAnimation.ToString());
            //print(MainCameraController.instance.CurrentCamera.transform);
            GetComponent<CameraShake>().Shake(MainCameraController.instance.CurrentCamera.transform);
            m_hypeMeterFxController.SpawnHypeAnimEndFx(1.5f);
            Utils.SpawnEfxWithDestroy(Input.mousePosition, tapSmashPanel, tapFx, 2f);
            //handCoroutine = StartCoroutine(HandUIState(true, 2f));
            if (sfx.isPlaying) { return; }
            PlaySfx(applauseLoop);
        }
        void OnTapOver()
        {
            player_anim.speed = 1;
            player_anim.CrossFade(rapData.rapPose.ToString(), 0.1f);
            //punchline.text = rapData.punchLine;
            //punchline.transform.parent.gameObject.SetActive(true);
            rapFinishPanel.SetActive(true);
            m_hypeMeterFxController.SpawnFountainFx();
            hypeMeter.fillAmount = 0;
            tapSmashPanel.gameObject.SetActive(false);
            Timer.Delay(6f, () =>
            {
                OnLevelEnd();
            });
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
            punchline.transform.parent.gameObject.SetActive(false);
            currentCorrectCount = 0;
            currentWrongCount = 0;
            hintText.transform.parent.gameObject.SetActive(false);
            stepProgress.Reset();
            audienceManager.Reset();
            m_hypeMeterFxController.Reset();
            rapWordNo = 0;
            currentLyric = 0;
            multiTouchManager.Reset();
            rapFinishPanel.SetActive(false);
            onTapped = null;
            restartPanel.SetActive(false);
            rapCameraIndex = 0;
        }

        void PlayAudio(bool correct)
        {
            if (lyricsAudioSource.isPlaying)
            {
                lyricsAudioSource.Stop();
            }
            lyricsAudioSource.clip = correct == true ? rapData.rapBattleLyricSO.correctLyrics[currentLyric] : rapData.rapBattleLyricSO.wrongLyrics[currentLyric];
            Timer.Delay(1.5f, () =>
            {
                lyricsAudioSource.Play();
            });
            currentLyric++;
        }

        void PlaySfx(AudioClip clip)
        {
            if (sfx.isPlaying)
            {
                sfx.Stop();
            }
            sfx.clip = clip;
            sfx.Play();
        }
        [NaughtyAttributes.Button]
        void ResetTutorial()
        {
            Progress.Instance.TapSmashTut = false;
        }

        IEnumerator HandUIState(bool state, float time)
        {
            yield return new WaitForSeconds(time);
            handUI.SetActive(state);
        }
        // public void OnNextLevelLoad()
        // {
        //     Application.LoadLevel(0);
        // }
    }
}
