using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrisonControl;

[System.Serializable]
public class ParticleAndCount
{
    public GameObject particlePrefab;
    public int count;
}
public class HypeMeterFxController : MonoBehaviour
{
    public VFXSO vfxData;
    GameObject hypeAnimEndFx, currentHypeAnimEndFx;
    GameObject endFountainFx
    {
        get
        {
            int random = Random.Range(0, endFountainFxList.Count);
            print(endFountainFxList.Count + "," + random);
            return endFountainFxList[random];
        }
    }
    public List<ParticleAndCount> onHypeParticles = new List<ParticleAndCount>();
    List<GameObject> endFountainFxList = new List<GameObject>();
    public List<GameObject> spawnedEfx = new List<GameObject>();
    List<ParticleCountSpawn> particleCountSpawns = new List<ParticleCountSpawn>();
    void Start()
    {
        Init();
    }

    void Init()
    {
        endFountainFxList.Clear();
        endFountainFxList.Add(vfxData.fireFountain);
        endFountainFxList.Add(vfxData.smokeFountain);
        hypeAnimEndFx = vfxData.sparkleFountain;
    }
    
    
    public void SpawnFountainFx()
    {
        GameObject efx = endFountainFx;
        for (int i = 0; i < EnvironmentList.instance.GetCurrentEnvironment.fountainFx.Count; i++)
        {
            spawnedEfx.Add(Utils.SpawnEfx(EnvironmentList.instance.GetCurrentEnvironment.fountainFx[i], efx, true));
        }
    }

    public void SpawnHypeAnimEndFx(float destroyTime)
    {
        for (int i = 0; i < EnvironmentList.instance.GetCurrentEnvironment.hypeAnimEndFx.Count; i++)
        {
            currentHypeAnimEndFx = Utils.SpawnEfxWithDestroy(EnvironmentList.instance.GetCurrentEnvironment.hypeAnimEndFx[i], hypeAnimEndFx, destroyTime, false);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < spawnedEfx.Count; i++)
        {
            Destroy(spawnedEfx[i].gameObject);
        }
        for(int i = 0; i < particleCountSpawns.Count; i++)
        {
            Destroy(particleCountSpawns[i]);
        }
        particleCountSpawns.Clear();
        spawnedEfx.Clear();
    }

    public void InitParticleAndCount()
    {
        for(int i = 0; i < onHypeParticles.Count; i++)
        {
            ParticleCountSpawn pfx = gameObject.AddComponent<ParticleCountSpawn>();
            pfx.count = onHypeParticles[i].count;
            pfx.spawnPos = EnvironmentList.instance.GetCurrentEnvironment.multiTapFx;
            pfx.particlePrefab = onHypeParticles[i].particlePrefab;
            RapBattleManager.onTapped += pfx.IncreaseCount;
            particleCountSpawns.Add(pfx);
        }
    }
}
