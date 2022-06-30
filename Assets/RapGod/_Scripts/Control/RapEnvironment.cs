using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class RapEnvironment : MonoBehaviour
{
    public Transform playerNarrationPos, enemyNarrationPos, playerRapPos, enemyRapPos;
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
