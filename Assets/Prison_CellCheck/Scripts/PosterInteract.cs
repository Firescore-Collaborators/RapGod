using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterInteract : Interactables
{
    public Transform target;
    public override void Interact()
    {
        LerpObjectPosition.instance.LerpObject(transform, target.position, 0.5f);
    }
}
