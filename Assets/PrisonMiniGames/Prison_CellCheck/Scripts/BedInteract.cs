using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedInteract : Interactables
{
    public Animator anim;
    public override void Interact()
    {
        anim.Play("BedFlip");
    }   
}
