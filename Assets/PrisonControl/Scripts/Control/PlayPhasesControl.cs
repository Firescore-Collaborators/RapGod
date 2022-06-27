using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tabtale.TTPlugins;

namespace PrisonControl
{
    public class PlayPhasesControl : State
    {
        public event System.Action OnAllPhasesComplete;
        public event System.Action OnGameplayCanceled;

        [SerializeField]
        private AudioManager audioManager;


        public override void OnEnter()
        {
            audioManager.SetNormalBg();

            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("Level", Progress.Instance.CurrentLevel + (Progress.Instance.LevelMultiplier * levels.Length));
            TTPGameProgression.FirebaseEvents.LevelUp(Progress.Instance.CurrentLevel + (Progress.Instance.LevelMultiplier * levels.Length), parameter);


            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("missionType", "levels");
            parameters.Add("missionStartedType", "new");
            TTPGameProgression.FirebaseEvents.MissionStarted(Progress.Instance.CurrentLevel + (Progress.Instance.LevelMultiplier * levels.Length), parameters);


            mGameplayUi.SetActive(true);

            //mCurrentPhaseNumber = 0;
            //mPhaseCursor = (Progress.Instance.CurrentLevel >= Config.LEVEL_OF_FIRST_SANDING_APPEARANCE) ? 0 : 1;
            //mTotalPhaseCount = System.Enum.GetNames(typeof(PlayPhase)).Length - mPhaseCursor;

            //BeginNextPhase();

            totalMiniLevels = levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes.Length;
            currentMiniLevel = 0;

            idCheck_cursr = 0;
            lunchBox_cursr = 0;
            bribeBox_cursr = 0;
            correctAnswers = 0;
            Progress.Instance.WasBadDecision = false;

            gamePlay.gameObject.SetActive(true);

            //    BeginNextLevel();
        }

        public void BeginNextLevel()
        {

            Debug.Log("Current level is "+Progress.Instance.CurrentLevel);
            //Linup Characters
            DisplayLevelData();
        }

        void DisplayLevelData()
        {
            // Check for ID Step
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.IDCheck)
            {
                idCheck_cursr++;
                mPlayPhasesStateMachine.SwitchState(PlayPhase.IDCheck);
               // iDStepManager.AssignData();
            }
            else
            // Check for LunchBox step
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.LunchBox)
            {
                lunchBox_cursr++;
                mPlayPhasesStateMachine.SwitchState(PlayPhase.FoodTray);
            }
            else
            // Check for BribeBox step
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.BribeBox)
            {
                bribeBox_cursr++;
                mPlayPhasesStateMachine.SwitchState(PlayPhase.BribeBox);
            }else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.PhoneBooth)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.PhoneBooth);
            }
            else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.Interrogation)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.Interrogation);
            }
            else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.Warden)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.Warden);
            }
            else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.MugShot)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.MugShot);
            }
            else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.JailEntry)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.JailEntry);
            }
            else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.FoodPlating)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.FoodPlating);
            }
            else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.PrisonYard)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.PrisonYard);
            }
            else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.CCTVMonitor)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.CCTVMonitor);
            }
            else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.CellCheck)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.CellCheck);
            }
            else
            if (levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[currentMiniLevel] == Level_SO.LevelTypes.SlapAndRun)
            {
                mPlayPhasesStateMachine.SwitchState(PlayPhase.SlapAndRun);
            }
        }

        public override void OnExit()
        {
            Resources.UnloadUnusedAssets();
        }

        public override bool OnBackButtonPressed()
        {
            if (!PopupManager.Instance.OnBackButtonPressed())
            {
                mConfirmationUi.ShowConfirmation(
                    () => {
                        PopupManager.Instance.PopPopup();
                        StopPlay();
                    },
                    () => {
                        PopupManager.Instance.PopPopup();
                    }
                );
            }

            return true;
        }

        private void StopPlay()
        {
            mGameplayUi.SetActive(false);
            mPlayPhasesStateMachine.Reset();
            OnGameplayCanceled?.Invoke();
        }

        private void Awake()
        {
            mGameplayUi.SetActive(false);

            mPlayPhasesStateMachine = new StateMachine<PlayPhase>(
                (PlayPhase.Warden, GetComponent<WardenState>()),
                (PlayPhase.IDCheck, GetComponent<IDCheckState>()),
                (PlayPhase.FoodTray, GetComponent<FoodTrayState>()),
                (PlayPhase.BribeBox, GetComponent<BribeBoxState>()),
                (PlayPhase.PhoneBooth, GetComponent<PhoneBoothState>()),
                (PlayPhase.Interrogation, GetComponent<InterrogationState>()),
                (PlayPhase.JailEntry, GetComponent<JailEntryState>()),
                (PlayPhase.MugShot, GetComponent<MugShotState>()),
                (PlayPhase.FoodPlating, GetComponent<FoodPlatingState>()),
                (PlayPhase.PrisonYard, GetComponent<PrisonYardState>()),
                (PlayPhase.CCTVMonitor, GetComponent<CCTVMonitorState>()),
                (PlayPhase.CellCheck, GetComponent<CellCheckState>()),
                (PlayPhase.SlapAndRun, GetComponent<SlapAndRunState>())
            );
        }

        //private void BeginNextPhase()
        //{
        //    mPlayPhasesStateMachine.SwitchState(PHASES[mPhaseCursor]);
        //    mPlayPhasesUi.SetProgress(PHASES[mPhaseCursor], mCurrentPhaseNumber + 1, mTotalPhaseCount);
        //}

        public void _OnPhaseFinished()
        {

            gamePlay.gameObject.SetActive(false);
            mGameplayUi.SetActive(false);
            mPlayPhasesStateMachine.Reset();
            OnAllPhasesComplete?.Invoke();

        }
        public void _OnRestart()
        {
            StartCoroutine("Restart");
        }

        IEnumerator Restart()
        {
            gamePlay.gameObject.SetActive(false);
            mGameplayUi.SetActive(false);
            mPlayPhasesStateMachine.Reset();
            yield return new WaitForSeconds(0.01f);
            gamePlay.gameObject.SetActive(true);
            mGameplayUi.SetActive(true);

        }
        public void _OnMiniLevelFinished()
        {
            currentMiniLevel++;

            if(currentMiniLevel >= totalMiniLevels){
               
                gamePlay.LastGuest(() => {
                    gamePlay.gameObject.SetActive(false);

                    mGameplayUi.SetActive(false);
                    mPlayPhasesStateMachine.Reset();
                    OnAllPhasesComplete?.Invoke();
                });
            }
            else
            {
                gamePlay.NextGuest(false);
            }
        }

        public void _OnMiniLevelFinished2()
        {
            currentMiniLevel++;

            if (currentMiniLevel >= totalMiniLevels)
            {
                gamePlay.gameObject.SetActive(false);

                mGameplayUi.SetActive(false);
                mPlayPhasesStateMachine.Reset();
                OnAllPhasesComplete?.Invoke();
            }
            else
            {
                gamePlay.NextGuest(true);
            }
        }

        // To be called by the states
        //public void _OnPhaseFinished()
        //{
        //    ++mPhaseCursor;
        //    ++mCurrentPhaseNumber;
        //    if (mCurrentPhaseNumber == mTotalPhaseCount)
        //    {
        //        mGameplayUi.SetActive(false);
        //        mPlayPhasesStateMachine.Reset();
        //        OnAllPhasesComplete?.Invoke();
        //    }
        //    else
        //    {
        //        BeginNextPhase();
        //    }
        //}

        public int TotalMiniLevel
        {
            get
            {
                return totalMiniLevels;
            }
        }

        public int CurrentMiniLevel
        {
            get
            {
                return currentMiniLevel;
            }
        }

        [SerializeField]
        GameObject mGameplayUi;

        [SerializeField]
        PlayPhasesUi mPlayPhasesUi;

        [SerializeField]
        ConfirmationUi mConfirmationUi;

        [SerializeField]
        public Level_SO [] levels;

        [SerializeField]
        GamePlayStep gamePlay;

        [SerializeField]
        IDStepManager iDStepManager;

        private StateMachine<PlayPhase> mPlayPhasesStateMachine;
        private int mTotalPhaseCount;
        private int mCurrentPhaseNumber;
        private int mPhaseCursor;

        [HideInInspector]
        private int totalMiniLevels , currentMiniLevel;

        [HideInInspector]
        public int idCheck_cursr = 0;
        [HideInInspector]
        public int lunchBox_cursr = 0;
        [HideInInspector]
        public int bribeBox_cursr = 0;
        [HideInInspector]
        public int correctAnswers;

        // In order of play
        private static readonly PlayPhase[] PHASES =
        {
            PlayPhase.Warden,
            PlayPhase.IDCheck,
            PlayPhase.FoodTray,
            PlayPhase.BribeBox,
            PlayPhase.PhoneBooth,
            PlayPhase.Interrogation,
            PlayPhase.JailEntry,
            PlayPhase.MugShot,
            PlayPhase.FoodPlating,
            PlayPhase.PrisonYard,
            PlayPhase.CCTVMonitor,
            PlayPhase.CellCheck,
            PlayPhase.SlapAndRun
        };
    }
}
