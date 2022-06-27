using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAndRun_CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private  float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector3 pos;
    private void Update()
    {
        pos = gameObject.transform.localPosition;
        pos.x = target.transform.localPosition.x;
        pos.x = Mathf.Clamp(pos.x, -2.8f, 3.9f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
       
        gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, pos, speed * Time.deltaTime);
    }
}
