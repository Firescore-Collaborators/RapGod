using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EnvironmentList : MonoBehaviour
{
    public static EnvironmentList instance;

    public List<RapEnvironment> environments = new List<RapEnvironment>();
    RapEnvironment currentEnvironment;
    public AudienceManager GetAudienceManager
    {
        get
        {
            return currentEnvironment.audienceManager;
        }
    }

    public RapEnvironment GetCurrentEnvironment
    {
        get
        {
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
        Init();
    }

    public SpawnPosition GetEnvironment(RapEnvironmentType environment)
    {
        SpawnPosition spawnPosition = new SpawnPosition();
        if (currentEnvironment != null)
        {
            SwitchOffEnvironment();
        }
        currentEnvironment = environments[(int)environment.environment];
        currentEnvironment.gameObject.SetActive(true);

        switch (environment.position)
        {
            case EnvironmentPosition.Narration:
                spawnPosition.playerPos = currentEnvironment.playerNarrationPos;
                spawnPosition.enemyPos = currentEnvironment.enemyNarrationPos;
                MainCameraController.instance.SetCurrentCamera(currentEnvironment.narrationCam.name, 0);
                break;
            case EnvironmentPosition.Rap:
                spawnPosition.playerPos = currentEnvironment.playerRapPos;
                spawnPosition.enemyPos = currentEnvironment.enemyRapPos;
                MainCameraController.instance.SetCurrentCamera(currentEnvironment.rapCam.name, 0);
                if (currentEnvironment.audienceManager != null)
                {
                    currentEnvironment.audienceManager.gameObject.SetActive(true);
                }
                break;
        }
        return spawnPosition;
    }

    public void SwitchOffEnvironment()
    {
        if (currentEnvironment != null)
        {
            currentEnvironment.gameObject.SetActive(false);
            if(currentEnvironment.audienceManager != null)
            {
                currentEnvironment.audienceManager.gameObject.SetActive(false);
            }
        }
    }
    [Button]
    void Init()
    {
        environments.Clear();
        for(int i = 0; i < transform.childCount; i++)
        {
            environments.Add(transform.GetChild(i).GetComponent<RapEnvironment>());
        }
    }
}
