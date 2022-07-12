using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PrisonControl
{
    public class AgentUIManager : MonoBehaviour
    {
        [SerializeField] private PlayPhasesControl _mPlayPhasesControl;
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
            if (instance == null)
            {
                instance = this;
            }
        }
        // Start is called before the first frame update
        void OnEnable()
        {
            InitLevelData();
            InitPanel();
        }

        void InitLevelData()
        {
            Level_SO level = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1];
            agentsList_SO = level.GetAgentsListSO;
        }

        void InitPanel()
        {
            AgentUI = new GameObject[agentsList_SO.agentList.Count];

            for (int i = 0; i < agentsList_SO.agentList.Count; i++)
            {
                AgentUI[i] = Instantiate(Panel, new Vector2(CenterScreen.position.x + 700 * i, CenterScreen.position.y), Quaternion.identity, PanelParent.transform);
                AgentUI[i].transform.GetChild(1).GetComponent<TMP_Text>().text = agentsList_SO.agentList[i].AgentName;
                AgentUI[i].transform.GetChild(2).GetComponent<Image>().sprite = agentsList_SO.agentList[i].AgentPic;
                AgentUI[i].GetComponent<AgentUIPanel>().selectButton.onClick.AddListener(() =>
                {
                    OnAgentSelected();
                });
            }
        }

        public void OnAgentSelected()
        {
            _mPlayPhasesControl._OnPhaseFinished();
            LevelEnd();
        }

        void LevelEnd()
        {
            Reset();
        }

        void Reset()
        {
            for (int i = 0; PanelParent.transform.childCount > i; i++)
            {
                Destroy(PanelParent.transform.GetChild(i).gameObject);
            }
        }

    }
}
