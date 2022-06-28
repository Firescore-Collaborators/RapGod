using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class SimpleInteract : Interactables
    {
        [SerializeField]
        private Outline outline;

        public override void Interact()
        {
            //CheckCellManager.instance.Glow(transform);
            CheckCellManager.instance.OffTut();
            CheckCellManager.instance.raycast = false;
            CheckCellManager.instance.lerpPostion.parent = null;
            LerpObjectPosition.instance.LerpObject(transform, CheckCellManager.instance.lerpPostion.position, CheckCellManager.instance.lerpSpeed);
            LerpObjectRotation.instance.LerpObject(transform, CheckCellManager.instance.lerpPostion.rotation, CheckCellManager.instance.lerpSpeed);

            Timer.Delay(CheckCellManager.instance.objectDisappearTime, () =>
            {
                gameObject.SetActive(false);
                CheckCellManager.instance.Found(gameObject.name);
                CheckCellManager.instance.Explode();
                CheckCellManager.instance.raycast = true;
            });

            outline.enabled = true;
        }
        
    }
}
