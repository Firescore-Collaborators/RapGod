using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentList : MonoBehaviour
{
    public static EnvironmentList instance;

    public List<RapEnvironment> environments = new List<RapEnvironment>();

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
        RapEnvironment currentEnvironment = environments[(int)environment.environment];
        switch(environment.position)
        {
            case EnvironmentPosition.Narration:
                spawnPosition.playerPos = currentEnvironment.playerNarrationPos;
                spawnPosition.enemyPos = currentEnvironment.enemyNarrationPos;
                MainCameraController.instance.SetCurrentCamera("DefaultEnvironment_Narration",0);
                break;
            case EnvironmentPosition.Rap:
                spawnPosition.playerPos = currentEnvironment.playerRapPos;
                spawnPosition.enemyPos = currentEnvironment.enemyRapPos;
                MainCameraController.instance.SetCurrentCamera("DefaultEnvironment_Rap",0);
                break;
        }
        return spawnPosition;
    }
}
