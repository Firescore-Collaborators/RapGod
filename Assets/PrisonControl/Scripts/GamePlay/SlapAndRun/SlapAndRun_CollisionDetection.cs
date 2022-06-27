using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAndRun_CollisionDetection : MonoBehaviour
{
    public SlapAndRun_PlayerController playerController;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 14)
        {
            if (collision.gameObject.tag == "SlapAndRun_obstacle")
            {
                playerController.OnDead();
            }
            else
            {
                playerController.FindTarget(collision.gameObject);
            }
        }
    }
}
