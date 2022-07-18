using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class GamePlayStep : MonoBehaviour
    {

        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

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
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.AgentSelect)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.Signature)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.Lyrics)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.BeatSort)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
            else
            if (_mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetLevelTypes[0] == Level_SO.LevelTypes.Equalizer)
            {
                _mPlayPhasesControl.BeginNextLevel();
            }
        }
    }
}
