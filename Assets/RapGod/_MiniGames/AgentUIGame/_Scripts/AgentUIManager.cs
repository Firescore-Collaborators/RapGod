using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentUIManager : MonoBehaviour
{
    public static AgentUIManager instance;

    public GameObject[] AgentUI;
    public Transform CenterScreen;

    [SerializeField]
    public int AgentNum;

    private void Awake()
    {
        if(instance== null)
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
