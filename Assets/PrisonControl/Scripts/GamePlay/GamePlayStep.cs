using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class GamePlayStep : MonoBehaviour
    {
        public List<GuestController> guestList;

        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private Transform [] charQueuePos;

        public Transform guestStandPos, guestLeavePos, guestHolder;

        public Transform copStandPos, copLeavePos;

        private void Awake()
        {
            guestList = new List<GuestController>();
        }
        void OnEnable()
        {
            guestList.Clear();

            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.PhoneBooth)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.Interrogation)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.Warden)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.MugShot)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.JailEntry)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.FoodPlating)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.PrisonYard)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.CCTVMonitor)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.CellCheck)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.SlapAndRun)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            {
                // On Desk Flow // ID step and Item check
                OnDeskFlow();

            }
        }

        void OnDeskFlow()
        {
            int index = 0;

            foreach (Guest_SO guest in _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].Guests)
            {
                GameObject obj = Instantiate(guest.pf_character, charQueuePos[index].position, Quaternion.identity, guestHolder);

                obj.GetComponent<GuestController>().GuestID = index;
                obj.GetComponent<GuestController>().gamePlayStep = this;
                obj.GetComponent<GuestController>().lerpSpeed = 0.5f;


                guestList.Add(obj.GetComponent<GuestController>());
                index++;
            }
            StartCoroutine(Steps());
        }

        IEnumerator Steps()
        {
            yield return new WaitForSeconds(0);
            NextGuest(false);
        }

        public void NextGuest(bool wasArrested)
        {
            if (_mPlayPhasesControl.CurrentMiniLevel == 0)
            {
                Debug.Log("_mPlayPhasesControl.CurrentMiniLevel "+ _mPlayPhasesControl.CurrentMiniLevel);
                guestList[_mPlayPhasesControl.CurrentMiniLevel].StepHead(() => {
                    _mPlayPhasesControl.BeginNextLevel();
                });
            }
            else
            {
                if (wasArrested)
                    EnterGuest();
                else
                    ExitEnterGuest();
            }
        }

        public void LastGuest(System.Action callback)
        {
            guestList[_mPlayPhasesControl.CurrentMiniLevel - 1].StepOut(() =>
            {
                callback?.Invoke();
                Destroy(guestList[_mPlayPhasesControl.CurrentMiniLevel - 1].gameObject);

            });
        }

        public void EnterGuest()
        {
            Debug.Log("Only entering guest");
                guestList[_mPlayPhasesControl.CurrentMiniLevel].StepHead(() => {
                    _mPlayPhasesControl.BeginNextLevel();
                    //Destroy(guestList[_mPlayPhasesControl.CurrentMiniLevel - 1].gameObject);
                    //  guestList.RemoveAt(_mPlayPhasesControl.CurrentMiniLevel - 1);
                });
        }

        public void ExitEnterGuest()
        {
            Debug.Log("entering exiting guest");

            guestList[_mPlayPhasesControl.CurrentMiniLevel - 1].StepOut(() => {

                  guestList[_mPlayPhasesControl.CurrentMiniLevel].StepHead(() => {
                           Destroy(guestList[_mPlayPhasesControl.CurrentMiniLevel - 1].gameObject);
                      _mPlayPhasesControl.BeginNextLevel();
                      //  guestList.RemoveAt(_mPlayPhasesControl.CurrentMiniLevel - 1);
                  });
              });
        }


        public void ArrestGuest()
        {
            Debug.Log("Arrest Guest");
            guestList[_mPlayPhasesControl.CurrentMiniLevel].ArrestOut(() => {
              //  _mPlayPhasesControl.BeginNextLevel();

                Destroy(guestList[_mPlayPhasesControl.CurrentMiniLevel].gameObject);
                //guestList.RemoveAt(_mPlayPhasesControl.CurrentMiniLevel);
             //   _mPlayPhasesControl._OnMiniLevelFinished();

            });
        }
        public void OnArrestStarted()
        {
            guestList[_mPlayPhasesControl.CurrentMiniLevel].ScaredReaction();
        }

        public void OnSuspicious()
        {
            guestList[_mPlayPhasesControl.CurrentMiniLevel].SuspiciousReaction();
        }

        public void OnSuspiciousItemCheck()
        {
            guestList[_mPlayPhasesControl.CurrentMiniLevel].SuspiciousReactionItemCheck();
        }

        public void OnBrideRejected()
        {
            guestList[_mPlayPhasesControl.CurrentMiniLevel].AngryReaction();
        }
    }
}
