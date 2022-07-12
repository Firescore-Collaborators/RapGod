using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentUIManager : MonoBehaviour
{
    public static AgentUIManager instance;
    public GameObject[] AgentUI;

    //public Agent_SO[] agent_SO;
    public Transform CenterScreen;
    public AgentsList_SO agentsList_SO;
    public GameObject Panel, PanelParent, TempPanel;
    
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
         AgentUI = new GameObject[agentsList_SO.agentList.Count];

        for (int i = 0; i < agentsList_SO.agentList.Count; i++)
        {
            AgentUI[i] = Instantiate(Panel, new Vector2(CenterScreen.position.x + 700 * i, CenterScreen.position.y), Quaternion.identity, PanelParent.transform);
            AgentUI[i].transform.GetChild(1).GetComponent<TMP_Text>().text = agentsList_SO.agentList[i].AgentName;
            AgentUI[i].transform.GetChild(2).GetComponent<Image>().sprite = agentsList_SO.agentList[i].AgentPic;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
