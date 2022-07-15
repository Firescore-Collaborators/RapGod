using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Cinemachine;

public class RapEnvironment : MonoBehaviour
{
    public Transform playerNarrationPos, enemyNarrationPos, playerRapPos, enemyRapPos;
    [Foldout("Vfx")]
    private string cherry;
    public List<Transform> fountainFx, multiTapFx, hypeAnimEndFx = new List<Transform>();
    [Foldout("Cameras")]
    public List<CinemachineVirtualCamera> rapCameras = new List<CinemachineVirtualCamera>();
    public CinemachineVirtualCamera narrationCam, rapCam;
    public AudienceManager audienceManager;
    public GameObject popUp;
    public TypewriterEffect typewriter
    {
        get
        {
            return popUp.transform.GetChild(0).GetComponent<TypewriterEffect>();
        }
    }
}

//Cherry Pick new
