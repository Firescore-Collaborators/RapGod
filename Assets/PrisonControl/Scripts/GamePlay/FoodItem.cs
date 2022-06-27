using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class FoodItem : MonoBehaviour
    {
        void OnCollisionEnter()
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Invoke("MakeKinemetic", 1f);
        }

        void MakeKinemetic()
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
