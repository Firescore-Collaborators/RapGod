using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class GamePlayStep : MonoBehaviour
    {

        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private Transform [] charQueuePos;

        public Transform guestStandPos, guestLeavePos, guestHolder;

        public Transform copStandPos, copLeavePos;

        private void Awake()
        {
        }
        void OnEnable()
        {
            if(_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.rapBattle)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.Narration)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.ProfileDp)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.HandShake)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
        }
    }
}
