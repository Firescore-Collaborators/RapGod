using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class SignatureManager : MonoBehaviour
    {
        [SerializeField] private PlayPhasesControl _mPlayPhasesControl;
        GameObject signaturePrefab;
        void OnEnable()
        {
            InitLevelData();
            Init();
        }
        
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                LevelEnd();
            }
        }

        void InitLevelData()
        {
            Level_SO level = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1];
            signaturePrefab = level.GetSignaturePrefab;
        }   

        void Init()
        {
            signaturePrefab = Instantiate(signaturePrefab);
        }

        void LevelEnd()
        {
            Destroy(signaturePrefab);
            _mPlayPhasesControl._OnPhaseFinished();
        }
    }
}

