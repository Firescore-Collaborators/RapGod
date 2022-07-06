using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCountSpawn : MonoBehaviour
{
    [HideInInspector]
    public int count;
    [HideInInspector]
    public GameObject particlePrefab;
    public Transform spawnPos;
    int currentCount = 0;


    ParticleCountSpawn(int count, GameObject particlePrefab)
    {
        this.count = count;
        this.particlePrefab = particlePrefab;
    }

    void IncreaseCount()
    {
        currentCount++;
        if (currentCount < count)
        {
            GameObject efx = Utils.SpawnEfxWithDestroy(spawnPos, particlePrefab, 3f);
            currentCount = 0;
        }
    }
}
