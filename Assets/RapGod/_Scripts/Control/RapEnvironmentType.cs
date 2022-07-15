using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum EnvironmentType
{
    defaultEnvironment,
    streetEnvironment,
    houseEnvironment,
    barEnvironment,
    studioEnvironment,
    agentOfficeEnvironment,
}
[System.Serializable]
public enum EnvironmentPosition
{
    Rap,Narration
}
[System.Serializable]
public class SpawnPosition
{
    public Transform playerPos;
    public Transform enemyPos;
}

[System.Serializable]
public class RapEnvironmentType 
{
    public EnvironmentType environment;
    public EnvironmentPosition position;
}
//Cherry Pick

