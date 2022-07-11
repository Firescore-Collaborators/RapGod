using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentUIManager : MonoBehaviour
{
    public static AgentUIManager instance;
    public GameObject[] AgentUI;

    public Agent_SO[] agent_SO;
    public Transform CenterScreen;

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
         AgentUI = new GameObject[agent_SO.Length];

        for (int i = 0; i < agent_SO.Length; i++)
        {
            AgentUI[i] = Instantiate(Panel, new Vector2(CenterScreen.position.x + 700 * i, CenterScreen.position.y), Quaternion.identity, PanelParent.transform);
            AgentUI[i].transform.GetChild(1).GetComponent<TMP_Text>().text = agent_SO[i].AgentName;
            AgentUI[i].transform.GetChild(2).GetComponent<Image>().sprite = agent_SO[i].AgentPic;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
