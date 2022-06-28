using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBookInteract : Interactables
{
    public Transform target;
    SkinnedMeshRenderer mesh;
    bool toOpen;
    bool toClose = false;

    Vector3 initPos;
    Quaternion initRot;

    void Start()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();
        initPos = transform.position;
        initRot = transform.rotation;
    }
    void Update()
    {
        if (toOpen)
        {
            float val = mesh.GetBlendShapeWeight(0);
            val += Time.deltaTime * 200;
            mesh.SetBlendShapeWeight(0, val);
            if(val >= 100)
            {
                toOpen = false;
                mesh.SetBlendShapeWeight(0, 100);
            }
        }

        if(toClose)
        {
            float val = mesh.GetBlendShapeWeight(0);
            val -= Time.deltaTime * 200;
            mesh.SetBlendShapeWeight(0, val);
            if(val <= 0)
            {
                toClose = false;
                mesh.SetBlendShapeWeight(0, 0);
                LerpToDefault();
            }
        }
    }

    public override void Interact()
    {
        LerpObjectPosition.instance.LerpObject(transform, target.position, 0.5f);
        LerpObjectRotation.instance.LerpObject(transform, target.rotation, 0.5f,()=>
        {
            toOpen = true;
        });

    }

    public void Close()
    {
        toClose = true;
        toOpen = false;
    }
    void LerpToDefault()
    {
        LerpObjectPosition.instance.LerpObject(transform, initPos, 0.5f);
        LerpObjectRotation.instance.LerpObject(transform, initRot, 0.5f);
    }
}
