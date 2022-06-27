using UnityEngine;

public class SpiderBucket : MonoBehaviour
{
    void Awake()
    {
        float rand = (float)Random.Range(5, 10) / 10;
        //Debug.Log("rand "+rand);
        Invoke("Disable", rand);
    }

    void Disable()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
