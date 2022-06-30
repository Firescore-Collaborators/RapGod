using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentList : MonoBehaviour
{
    public static EnvironmentList instance;

    public List<RapEnvironment> environments = new List<RapEnvironment>();
    RapEnvironment currentEnvironment;
    public AudienceManager GetAudienceManager
    {
        get{
            return currentEnvironment.audienceManager;
        }
    }

    public RapEnvironment GetCurrentEnvironment
    {
        get{
            return currentEnvironment;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public SpawnPosition GetEnvironment(RapEnvironmentType environment)
    {
        SpawnPosition spawnPosition = new SpawnPosition();
        if(currentEnvironment!= null)
        {
           currentEnvironment.gameObject.SetActive(false); 
        }
        currentEnvironment = environments[(int)environment.environment];
        currentEnvironment.gameObject.SetActive(true);
        
        switch(environment.position)
        {
            case EnvironmentPosition.Narration:
                spawnPosition.playerPos = currentEnvironment.playerNarrationPos;
                spawnPosition.enemyPos = currentEnvironment.enemyNarrationPos;
                MainCameraController.instance.SetCurrentCamera(currentEnvironment.narrationCam.name,0);
                break;
            case EnvironmentPosition.Rap:
                spawnPosition.playerPos = currentEnvironment.playerRapPos;
                spawnPosition.enemyPos = currentEnvironment.enemyRapPos;
                MainCameraController.instance.SetCurrentCamera(currentEnvironment.rapCam.name,0);
                currentEnvironment.audienceManager.gameObject.SetActive(true);
                break;
        }
        return spawnPosition;
    }

    public void SwitchOffEnvironment()
    {
        if(currentEnvironment != null)
        {
            currentEnvironment.gameObject.SetActive(false);
            currentEnvironment.audienceManager.gameObject.SetActive(false);
        }
    }
}
