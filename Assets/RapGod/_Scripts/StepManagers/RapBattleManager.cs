using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using NaughtyAttributes;

namespace PrisonControl

{
    public class RapBattleManager : MonoBehaviour
    {

        public RapEnvironmentType environmentType;
        [SerializeField]
        [Foldout("SOData")]
        private CharacterListSO characterList;
        [SerializeField]
        [Foldout("SOData")]
        private VFXSO vFXSO;
        [SerializeField]
        private SpawnPosition spawnPosition;

        [SerializeField]
        [Foldout("Animator")]
        private RuntimeAnimatorController playerAnimator, enemyAnimator, girlAnimator;

        [SerializeField]
        GameObject handUI, tapFx;

        GameObject player, enemy, girl;
        public PlayPhasesControl m_playPhaseControl;
        [Foldout("Cameras")]
        public Camera renderCamera;
        [Foldout("Cameras")]
        public RenderTexture m_renderTexture;
        HypeMeterFxController m_hypeMeterFxController
        {
            get
            {
                return GetComponent<HypeMeterFxController>();
            }
        }

        [Foldout("AuidoSource")]
        public AudioSource lyricsAudioSource, bgmAudioSource, sfx;

        [SerializeField]
        private GameObject OptionPanle, restartPanel, PopUpPanle, youtubePanel, winnerConfitti, initialPopUpPos;

        [SerializeField]
        [Foldout("UI")]
        public Text remarks;

        [SerializeField]
        [Foldout("UI")]
        private TMP_Text optionA_text, optionB_text;

        [SerializeField]
        private Color[] colors;

        List<char> rapChar = new List<char>();

        [SerializeField]
        [Foldout("UI")]
        private TMP_Text popUp_text, DummyText;

        [SerializeField]
        private RectTransform PopUP_rect;

        [SerializeField]
        private AudienceManager audienceManager;

        [SerializeField]
        [Foldout("Animator")]

        private Animator player_anim, enemy_anim, ui_anim, girl_Anim;

        [SerializeField]
        private GameObject[] textAnswer_anim;

        [SerializeField]
        private Sprite[] sprites_btn;
        [Foldout("Particles")]
        [SerializeField]
        private GameObject Conffiti_btn, ConffitiWrong_btn, hearts;

        [SerializeField]
        [Foldout("UI")]
        private Image[] hand;

        [SerializeField]
        [Foldout("UI")]
        private Sprite[] popUp_sprite;

        [SerializeField]
        private GameObject homePanle, rewardPanle, rapFinishPanel;
        [Foldout("SOData")]
        public RapBattleDataSO rapData;
        public Transform tapSmashPanel;
        [Foldout("Youtube UI")]
        public Text liveText, viewText, likeText, videoTitle;
        [Foldout("UI")]
        public Text punchline, hintText;
        [Foldout("UI")]
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
        private string cherry;
        public static event System.Action onTapped;
        int levelNo;
        int textNo;
        int rapWordNo;
        float viewCount, liveCount;
        int rapCameraIndex = 0;
        bool print = false;
        bool increaseYoutubeVariables = false;
        bool playerWin = false;
        float duration, durationTarget;
        int currentCorrectCount;
        int currentWrongCount;
        int sideNo;
        String answer;
        bool removedLetter;
        Vector3 popUpPos;
        [SerializeField] int currentLyric;
        List<Vector3> playerPos = new List<Vector3>();
        List<Vector3> enemyPos = new List<Vector3>();
        public int girlWalkIndex;
        Quaternion girlRotation;

        private void OnEnable()
        {
            InitLevelData();
            Init();
            InitGirlPos();
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
            girl = Utils.spawnGameObject(rapData.girlCharacter, EnvironmentList.instance.GetCurrentEnvironment.girlRapPos);
            girl_Anim = girl.GetComponent<Animator>();
            girl_Anim.runtimeAnimatorController = girlAnimator;
            girlRotation = girl.transform.rotation;
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

        void InitGirlPos()
        {
            AddCheckPoint(spawnPosition.playerPos.transform.Find("GirlPos"), playerPos);
            AddCheckPoint(spawnPosition.enemyPos.transform.Find("GirlPos"), enemyPos);
        }

        void AddCheckPoint(Transform pos, List<Vector3> list)
        {
            list.Clear();
            float distance = Vector3.Distance(girl.transform.position, pos.position);
            int count = rapData.rapBattleLyricSO.leveldata.Count;
            float segment = distance / count;
            //0.75f = 3/4
            Vector3 dir = (pos.position - girl.transform.position).normalized;
            for (int i = 1; i <= count; i++)
            {
                // add a pos between girl pos and pos
                Vector3 newPos = girl.transform.position + (dir * i * segment);
                list.Add(newPos);
            }
        }

        void MoveGirl(bool isPlayer)
        {
            Vector3 pos = isPlayer ? playerPos[girlWalkIndex] : enemyPos[girlWalkIndex];
            LerpObjectPosition.instance.LerpObject(girl.transform, pos, 1, () =>
            {
                girl_Anim.CrossFade("Idle", 0.1f);
                LerpObjectRotation.instance.LerpObject(girl.transform,girlRotation,0.3f);
            });
            girl_Anim.CrossFade("Walk", 0.1f);
            girl.transform.LookAt(pos);
            girlWalkIndex++;
        }
        public void OnNextRapWord()
        {
            if (player_anim != null)
            {
                player_anim.CrossFade("Idle", 0.1f);
                enemy_anim.CrossFade("Idle", 0.1f);
            }
            //player_anim.SetBool("Idle", true);

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
            IncreaseYoutubeVariables();
        }

        void IncreaseYoutubeVariables()
        {
            if (!increaseYoutubeVariables) return;

            viewCount = (viewCount + (Time.deltaTime * 6));
            liveCount += Time.deltaTime;
            int likeCount = (int)(viewCount / 1.5f);
            viewText.text = (int)viewCount + "M Views";
            likeText.text = likeCount + "M Likes";
            // TimeSpan timeSpan = TimeSpan.FromSeconds(liveCount);
            // string timeText = string.Format("{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds);


            liveText.text = FormatSeconds(liveCount);
        }

        string FormatSeconds(float elapsed)
        {
            int d = (int)(elapsed * 100.0f);
            int minutes = d / (60 * 100);
            int seconds = (d % (60 * 100)) / 100;
            int hundredths = d % 100;
            return String.Format("{0:00}:{1:00}", minutes, seconds);
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
                            Timer.Delay(1.5f, () =>
                            {
                                MoveGirl(true);
                            });
                        }
                        else
                        {
                            currentWrongCount++;
                            audienceManager.OnMoveEnemySide(0);
                            Timer.Delay(1.5f, () =>
                            {
                                MoveGirl(false);
                            });
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
            PlayRapAnim();
            SetRapCamera();

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


            print = false;
            //player_anim.SetBool("Idle", false);
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
            EnvironmentList.instance.SetRapCamera((int)rapData.rapCameras[rapCameraIndex]);
            rapCameraIndex++;
        }

        void PlayRapAnim()
        {
            string playerAnimName = (rapData.rapAnimations[rapCameraIndex].playerAnim).ToString();
            string enemyAnimName = (rapData.rapAnimations[rapCameraIndex].enemyAnim).ToString();
            player_anim.CrossFade(playerAnimName, 0.1f);
            enemy_anim.CrossFade(enemyAnimName, 0.1f);
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
                playerWin = currentCorrectCount >= currentWrongCount ? true : false;
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
                        //player_anim.SetBool("win", true);
                        player_anim.CrossFade("Cheering", 0.01f);
                        enemy_anim.CrossFade("Lose", 0.01f);
                        respondMessageController.ShowCorrectRespone("WINNER MENTALITY !");
                        Invoke("TapSmash", 1f);
                    }
                    else
                    {
                        ui_anim.SetTrigger("loose");
                        remarks.text = "LOSER!";
                        remarks.color = Color.red;
                        remarks.gameObject.SetActive(true);
                        //player_anim.SetBool("loose", true);
                        EnvironmentList.instance.SetRapCamera(0, 1);
                        RapPose();
                        respondMessageController.ShowWrongResponse("NO HYPE");
                        Timer.Delay(5f, () =>
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
            EnvironmentList.instance.SetRapCamera(0, 1);
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
            player_anim.Play(rapData.rapEndAnimation.ToString());
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
            RapPose();
            //punchline.text = rapData.punchLine;
            //punchline.transform.parent.gameObject.SetActive(true);
            rapFinishPanel.SetActive(true);
            m_hypeMeterFxController.SpawnFountainFx();
            hypeMeter.fillAmount = 0;
            tapSmashPanel.gameObject.SetActive(false);
            //rapFinishPanel.SetActive(false);
            YoutubePanel();
            Timer.Delay(10f, () =>
            {
                OnLevelEnd();
            });
        }

        void RapPose()
        {
            string playerAnimName = playerWin ? rapData.rapPoses.playerSuccess.ToString() : rapData.rapPoses.playerFail.ToString();
            string enemyAnimName = playerWin ? rapData.rapPoses.enemyFail.ToString() : rapData.rapPoses.enemySuccess.ToString();
            player_anim.CrossFade(playerAnimName, 0.1f);
            enemy_anim.CrossFade(enemyAnimName, 0.1f);
        }

        void OnLevelEnd()
        {
            // homePanle.SetActive(false);
            // rewardPanle.SetActive(true);
            LevelReset();
            m_playPhaseControl._OnPhaseFinished();
        }

        void YoutubePanel()
        {
            youtubePanel.SetActive(true);
            renderCamera.gameObject.SetActive(true);
            renderCamera.targetTexture = m_renderTexture;
            videoTitle.text = rapData.youtubeVideoTitle;
            viewCount = UnityEngine.Random.Range(1, 10);
            liveCount = UnityEngine.Random.Range(60, 300);
            increaseYoutubeVariables = true;
            Timer.Delay(2f, () =>
            {
                hearts.SetActive(true);
            });
        }
        void LevelReset()
        {
            EnvironmentList.instance.SwitchOffEnvironment();
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
            renderCamera.targetTexture = null;
            youtubePanel.SetActive(false);
            renderCamera.gameObject.SetActive(false);
            increaseYoutubeVariables = false;
            hearts.SetActive(false);
            Destroy(spawnPosition.enemyPos.transform.GetChild(0).gameObject);
            Destroy(spawnPosition.playerPos.transform.GetChild(0).gameObject);
            playerWin = false;
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

//Cherry Pick