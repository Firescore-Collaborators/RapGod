using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Cinemachine;

public class RapEnvironment : MonoBehaviour
{
    public Transform playerNarrationPos, enemyNarrationPos, playerRapPos, enemyRapPos;
    [Foldout("Vfx")]
    public List<Transform> fountainFx, multiTapFx,hypeAnimEndFx = new List<Transform>();

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

//Cherry Pick
