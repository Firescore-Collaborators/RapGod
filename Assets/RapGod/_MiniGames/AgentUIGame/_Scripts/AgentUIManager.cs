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
        [SerializeField] private AudioClip bgMusic;
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
            AudioManager.instance.PlayBGMusic(bgMusic);
        }

        void InitPanel()
        {
            AgentUI = new GameObject[agentsList_SO.agentList.Count];

            for (int i = 0; i < agentsList_SO.agentList.Count; i++)
            {
                AgentUI[i] = Instantiate(Panel, new Vector2(CenterScreen.position.x + 700 * i, CenterScreen.position.y), Quaternion.identity, PanelParent.transform);
                AgentUI[i].transform.GetChild(1).GetComponent<TMP_Text>().text = agentsList_SO.agentList[i].AgentName;
                AgentUI[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().sprite = agentsList_SO.agentList[i].AgentPic;
                int index = AgentUI[i].transform.GetSiblingIndex();
                AgentUI[i].GetComponent<AgentUIPanel>().selectButton.onClick.AddListener(() =>
                {
                    OnAgentSelected();
                    ScaleSelectedPanel(index);
                });
            }
        }
        
        void ScaleSelectedPanel(int index)
        {
            Transform panel = PanelParent.transform.GetChild(index);
            panel.LeanScale((Vector3.one * 1.5f),0.25f);
        }

        public void OnAgentSelected()
        {
            Timer.Delay(2f, () =>
            {
                _mPlayPhasesControl._OnPhaseFinished();
                LevelEnd();
            });
        }

        void LevelEnd()
        {
            Reset();
            EnvironmentList.instance.SwitchOffEnvironment();
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
