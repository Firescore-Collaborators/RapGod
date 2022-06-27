using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScenarioManager : MonoBehaviour
{
    public ObstacleAvoidanceType AvoidanceType;
    public NavMeshAgent AgentPrefab;
    public bool RandomizePriority = false;
    public float AgentSpeed = 2f;
    public float AgentRadius = 0.33f;

    [Header("Object References")]
    public GameObject Cubes;
    public NavMeshSurface Surface;

    public List<NavMeshAgent> Agents = new List<NavMeshAgent>();

    [Header("NavMesh Configurations")]
    public float AvoidancePredictionTime = 2;
    public int PathfindingIterationsPerFrame = 100;

    [Header("Circle Configuration")]
    public float CircleRadius = 25;
    [SerializeField]
    public int AgentsInCircle = 100;

    [Header("Narrow Path Configuration")]
    public int NarrowPathwayAgentsPerRegion = 25;
    public float NarrowPathwayOffset = 10;

    public float InvokeDelay = 2f;
    public GameObject Target;

    private void Start()
    {
        Surface.BuildNavMesh();
        //  Run1On1Scenario();
    }
    private void Update()
    {
        NavMesh.avoidancePredictionTime = AvoidancePredictionTime;
        NavMesh.pathfindingIterationsPerFrame = PathfindingIterationsPerFrame;
    }

  
    private void DestroyAllAgents()
    {
        Agents.ForEach(agent => Destroy(agent.gameObject));
        Agents.Clear();
    }

   

    private void SetupAgent(NavMeshAgent Agent)
    {
        Agent.obstacleAvoidanceType = AvoidanceType;
        Agent.radius = AgentRadius;
        Agent.speed = AgentSpeed;
        if (RandomizePriority)
        {
            Agent.avoidancePriority = Random.Range(0, 100);
        }
        Agents.Add(Agent);
    }
    
    public void onSetupCharacter(NavMeshAgent _navMeshAgent)
    {
        SetupAgent(_navMeshAgent);
    }

    //private void Set1On1Destinations()
    //{
    //    Agents[0].SetDestination(Agents[1].transform.position);
    //    Agents[1].SetDestination(Agents[0].transform.position);
    //}

    public void OnSetTarget(GameObject _pos)
    {
        Agents.ForEach(agent => agent.transform.GetComponent<Collider>().enabled = false);
        Agents.ForEach(agent => agent.transform.parent.GetComponent<SlapAndRun_PrisionerController>().OnSetCellPosition());
        Agents.ForEach(agent => agent.SetDestination(_pos.transform.position));
       
    }

    public void OnStop()
    {

        //  Agents.ForEach(agent => agent.transform.parent.GetComponent<SlapAndRun_PrisionerController>().OnSetCellPosition());
        Agents.ForEach(agent => agent.transform.parent.GetComponent<SlapAndRun_PrisionerController>().OnStop());

    }

}
