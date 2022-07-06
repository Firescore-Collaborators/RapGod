using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualizerManager : MonoBehaviour
{
    public static EqualizerManager instance;

    public GameObject[] squares;

    public GameObject[] slider;

    public GameObject[] Limit;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
