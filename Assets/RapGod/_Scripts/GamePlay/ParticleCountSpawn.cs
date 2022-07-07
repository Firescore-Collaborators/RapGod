using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCountSpawn : MonoBehaviour
{
    public int count;
    [HideInInspector]
    public GameObject particlePrefab;
    public List<Transform> spawnPos;
    int currentCount = 0;


    ParticleCountSpawn(int count, GameObject particlePrefab)
    {
        this.count = count;
        this.particlePrefab = particlePrefab;
    }

    public void IncreaseCount()
    {
        currentCount++;
        if (currentCount >= count)
        {
            Transform toSpawn = spawnPos[Random.Range(0, spawnPos.Count)];
            GameObject efx = Utils.SpawnEfxWithDestroy(toSpawn, particlePrefab, 3f);
            currentCount = 0;
        }
        print("ParticleTapped : " + currentCount);
    }
}
