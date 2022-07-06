using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    List<GameObject> spawnedEfx = new List<GameObject>();

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
    }

}
