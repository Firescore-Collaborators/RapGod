using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrisonControl;

public class SlapANdRun_Instantiate : MonoBehaviour
{
    public GameObject playerContainer,prisonerContainer,wardenContainer;
    public GameObject PlayerPrefab,PrisonerPrefabContainer,PrisonerPrefab,wardenPrefab,ObstacleConainer;
    public SlapAndRun_CellTrigger slapAndRun_CellTrigger;
    public SlapAndRun_UiManager uiManager;

    [SerializeField]
    private List<GameObject> prisoners;

    [SerializeField]
    private List<GameObject> obstaclePrefab;

    [SerializeField]
    private ScenarioManager scenarioManager;

    [SerializeField]
    private GameObject chaseTarget;

    public SlapAndRunStep slapAndRunStep;


    public System.Action OnLevelEnd;
    public System.Action OnTutorialShow;

    public GameObject temp_warden;

    public string wardenString;

    private void Awake()
    {
        slapAndRun_CellTrigger.onGateClosed += OnGateClosed;
    }

    private void OnDestroy()
    {
        slapAndRun_CellTrigger.onGateClosed -= OnGateClosed;
        obj.GetComponent<SlapAndRun_PlayerController>().OnGameStarted -= OnTutorialEnable;
    }

    void OnGateClosed()
    {
  
        OnLevelEnd?.Invoke();
    }

    void OnTutorialEnable()
    {
        OnTutorialShow?.Invoke();
    }
    GameObject obj;

    void Start()
    {
        obj = Instantiate(PlayerPrefab, playerContainer.transform);
        obj.GetComponent<SlapAndRun_PlayerController>().scenarioManager = scenarioManager;
        obj.GetComponent<SlapAndRun_PlayerController>().slapAndRunStep = slapAndRunStep;
        obj.GetComponent<SlapAndRun_PlayerController>().uiManager  = uiManager;
        chaseTarget = obj.GetComponent<SlapAndRun_PlayerController>().chaseTarget;
        temp_warden = Instantiate(wardenPrefab, wardenContainer.transform);
        temp_warden.GetComponent<Animator>().runtimeAnimatorController = PrisonerPrefab.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController;
        temp_warden.transform.localPosition = Vector3.zero;
        temp_warden.transform.localEulerAngles = Vector3.zero;

        temp_warden.GetComponent<SlapAndRun_warden>().ShowConversation(wardenString);

        obj.GetComponent<SlapAndRun_PlayerController>().OnGameStarted += OnTutorialEnable;
       // += OnTutorialEnable;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;

        
        for(int i =0;i<prisonerContainer.transform.childCount;i++)
        {
            GameObject tempObj = Instantiate(prisoners[Random.Range(0,prisoners.Count)], prisonerContainer.transform.GetChild(i).transform);
            tempObj.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            GameObject prefabContainer = Instantiate(PrisonerPrefabContainer, prisonerContainer.transform.GetChild(i).transform);
            tempObj.transform.SetParent(prefabContainer.transform.GetChild(0));
            tempObj.transform.localPosition = Vector3.zero;
            tempObj.transform.localEulerAngles = Vector3.zero;
              
            tempObj.gameObject.GetComponent<Animator>().runtimeAnimatorController = PrisonerPrefab.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController;
            prefabContainer.GetComponent<SlapAndRun_PrisionerController>().anim = tempObj.gameObject.GetComponent<Animator>();
            prefabContainer.GetComponent<SlapAndRun_PrisionerController>().scenarioManager = scenarioManager;
            prefabContainer.GetComponent<SlapAndRun_PrisionerController>().chaseTarget  = chaseTarget;
           // prefabContainer.GetComponent<SlapAndRun_PrisionerController>().moving_obstacle = true;
            prefabContainer.transform.localEulerAngles = Vector3.zero;
          //  tempObj.transform.SetParent(prisonerContainer.transform);
        }

        if (ObstacleConainer != null)
        {
            for (int i = 0; i < ObstacleConainer.transform.childCount; i++)
            {
                GameObject tempObj = Instantiate(obstaclePrefab[Random.Range(0, obstaclePrefab.Count)], ObstacleConainer.transform.GetChild(i).transform);
                tempObj.transform.localEulerAngles = Vector3.zero;
                tempObj.transform.localPosition = Vector3.zero;
                //  tempObj.transform.SetParent(prisonerContainer.transform);
            }
        }
    }

    public void OnStartRun()
    {
        obj.GetComponent<SlapAndRun_PlayerController>().OnStartRun();
    }
}
