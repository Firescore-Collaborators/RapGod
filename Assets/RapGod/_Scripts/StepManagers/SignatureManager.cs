using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class SignatureManager : MonoBehaviour
    {
        [SerializeField] private PlayPhasesControl _mPlayPhasesControl;
        [SerializeField] Color bgColor;
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
            MainCameraController.instance.SetCameraSolidColor(bgColor);
        }   

        void Init()
        {
            signaturePrefab = Instantiate(signaturePrefab);
            PencilMoveScript pen = signaturePrefab.transform.Find("Pen").GetComponent<PencilMoveScript>();
            pen.onReset += LevelEnd;
        }

        void LevelEnd()
        {
            Destroy(signaturePrefab);
            MainCameraController.instance.ResetCameraColor();
            _mPlayPhasesControl._OnPhaseFinished();
        }
    }
}

