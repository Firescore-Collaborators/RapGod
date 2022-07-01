using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Notes_Manager : MonoBehaviour
{
    public static Notes_Manager instance;
    public Transform endpoint1, endpoint2;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


}