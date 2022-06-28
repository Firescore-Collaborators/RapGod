using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonBookInteract : Interactables
{
    public Transform target;

    Transform cover;
    Vector3 initPos;
    Quaternion initRot;

    void Start()
    {
        cover = transform.GetChild(0);
        initPos = transform.position;
        initRot = transform.rotation;
    }
    public override void Interact()
    {
        print("Interact");
        LerpObjectPosition.instance.LerpObject(transform, target.position, 0.5f);
        LerpObjectRotation.instance.LerpObject(transform, target.rotation, 0.5f, () =>
        {
            LerpObjectRotation.instance.LerpObject(cover, Quaternion.Euler(-62f, -90, 270f), 0.5f, () =>
            {
            });
        });
    }

    void Close()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("Close");
        Timer.Delay(1.0f, () =>
        {
            LerpObjectPosition.instance.LerpObject(transform, initPos, 0.5f);
            LerpObjectRotation.instance.LerpObject(transform, initRot, 0.5f, () =>
            {

            });
        });

    }
}
