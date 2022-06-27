using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class Rotate : MonoBehaviour
    {
        public int rotateSpeed;

        void Start()
        {

        }

        void LateUpdate()
        {
            transform.Rotate(-rotateSpeed * transform.forward * Time.deltaTime);
        }
    }
}
