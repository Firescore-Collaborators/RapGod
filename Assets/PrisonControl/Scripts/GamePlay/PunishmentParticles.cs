using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunishmentParticles : MonoBehaviour
{
    [SerializeField]
    private GameObject pf_particles, pf_lowBlowParticle, pf_spiders, pf_bucket, pf_ice, pos;

    private SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    private Material shock_material;

    [SerializeField]
    private bool isWarden;

    public void SlapParticles()
    {

        GameObject obj = Instantiate(pf_particles, transform);
        Destroy(obj, 2);
    }

    public void LowBlowParticles()
    {
        GameObject obj = Instantiate(pf_lowBlowParticle, transform);
        Destroy(obj, 2);
    }

    public void ActivateShock()
    {
        if (!isWarden)
        {
            meshRenderer = GameObject.Find("Rhett_Outfit4").GetComponent<SkinnedMeshRenderer>();
            foreach (Material mat in meshRenderer.sharedMaterials)
            {
                Debug.Log("**** shader name " + mat.shader.name);
                if (mat != null && mat.shader != null && mat.shader.name != null && mat.shader.name == "Shader Graphs/ShockShaderPBR")
                {
                    Debug.Log("**** activate shock ");
                    mat.SetFloat("electricity_amt", 5);

                    Timer.Delay((1), () =>
                    {
                        mat.SetFloat("electricity_amt",0);
                    });
                }

            }
        }
        else
        {
            shock_material.SetFloat("electricity_amt", 6);
            Timer.Delay((1), () =>
            {
                shock_material.SetFloat("electricity_amt", 0);
            });
        }
    }


    public void DeActivateShock()
    {
        foreach (Material mat in meshRenderer.materials)
        {
            if (mat != null && mat.shader != null && mat.shader.name != null && mat.shader.name == "Shader Graphs/ShockShaderPBR")
            {
                Debug.Log("**** deactivate shock ");
                mat.SetFloat("electricity_amt", 0);
            }
        }
    }

    public void SpiderBucket()
    {
        StartCoroutine(SpawnSpiders());
    }

    IEnumerator SpawnSpiders()
    {
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.001f);
            float Rand = ((float)Random.Range(-5, 5) / 100);
            Debug.Log("Rand " + Rand);
            GameObject obj = Instantiate(pf_spiders, new Vector3(transform.position.x + Rand, 2.26f, transform.position.z + Rand), pf_spiders.transform.rotation);
            obj.GetComponent<SpiderMovement>().no = i;
        }
    }

    public void IceBucket()
    {
        StartCoroutine(SpawnIce());
    }

    IEnumerator SpawnIce()
    {
        for (int i = 0; i < 300; i++)
        {
            yield return new WaitForSeconds(0.0000001f);
            float Rand = ((float)Random.Range(-5, 5) / 100);
            Debug.Log("Rand " + Rand);
            GameObject obj = Instantiate(pf_ice, new Vector3(pos.transform.position.x + Rand, 2.26f, transform.position.z + Rand), pf_spiders.transform.rotation);
            obj.GetComponent<IceMovement>().no = i;
        }
    }



    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SpiderBucket();
        }
    }
}
