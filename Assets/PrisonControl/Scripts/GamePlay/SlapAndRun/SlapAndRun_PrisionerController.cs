using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlapAndRun_PrisionerController : MonoBehaviour
{
    // public Rigidbody chest;
    // public float UPforce,ForwardForce;
    // public RootMotion.Dynamics.PuppetMaster puppetMaster;

    [HideInInspector]
    public ScenarioManager scenarioManager;
    public NavMeshAgent meshAgent;

    [HideInInspector]
    public GameObject chaseTarget;
    int randomChase;
    public bool run;

    [HideInInspector]
    public Animator anim;

    [SerializeField]
    private AudioSource slap_sfx;

    [SerializeField]
    private GameObject slapEffect;

    [SerializeField]
    private GameObject[] slapTextEffect;

    bool InCell;

    public AnimationClip[] animationClips;

    [SerializeField]
    private Vector2Int X;

    public bool moving_obstacle,rotate_Obstacle;
    float targetx;
    private void OnEnable()
    {
        Debug.Log(gameObject.transform.position);
       // scenarioManager = GameObject.Find("Nav Mesh Manager").gameObject.GetComponent<ScenarioManager>();
       //  chaseTarget = GameObject.Find("Target Collection").gameObject;//Target Collection
    }

    private void Start()
    {
        //  anim.

        AnimatorOverrideController aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        foreach (var a in aoc.animationClips)
        {
            int i = Random.Range(0, animationClips.Length);
            if (a.name == "Idle")
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, animationClips[i]));

            if (a.name == "Idle_1")
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, animationClips[i]));
        }

        aoc.ApplyOverrides(anims);
        anim.runtimeAnimatorController = aoc;

        randomChase = 0;
    }

    private void Update()
    {
        if (run)
        {
            Vector3 chasePos = chaseTarget.transform.GetChild(randomChase).transform.position;
            if (Vector3.Distance(meshAgent.transform.position, chasePos) < 5)
            {
                meshAgent.speed = 5.5f;
            }
            meshAgent.SetDestination(chasePos);
        }
        else if(InCell)
        {
            transform.GetChild(0).transform.eulerAngles = Vector3.Lerp(transform.GetChild(0).transform.eulerAngles, new Vector3(0, 180, 0), 5 * Time.deltaTime);
        }

        if(moving_obstacle)
        {
            Vector3 tempPos = gameObject.transform.position;

            if (gameObject.transform.position.x >= X.x)
            {
                targetx = X.y;
            }
            else if (gameObject.transform.position.x <= X.y)
            {
                targetx = X.x;
            }
            tempPos.x = targetx;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, tempPos, 10 * Time.deltaTime);
        }
       
    }

    public void AddForce(int _dir, GameObject _obj, Vector3 _pos)
    {
        Instantiate(slapEffect, _pos, Quaternion.identity,this.gameObject.transform);

        if (_obj.transform.parent.gameObject.GetComponent<SlapAndRun_PlayerController>().SlapCounter == 3)
        {
            _obj.transform.parent.gameObject.GetComponent<SlapAndRun_PlayerController>().SlapCounter = 0;
            Instantiate(slapTextEffect[Random.Range(0,slapTextEffect.Length)], _pos, Quaternion.identity, this.gameObject.transform);
        }
        StartCoroutine(GettingSlap(_dir, _obj));
    }

    IEnumerator GettingSlap(int _dir, GameObject _obj)
    {
        //    if (_dir == 1)
        //    {
        //        anim.SetTrigger("Rslap"); //Rslap
        //    }
        //    else
        //    {
        //        anim.SetTrigger("Lslap");
        //    }

     

        Vector3 posA = gameObject.transform.position;
        Vector3 posB = _obj.transform.position;
        Vector3 dir = (posB - posA).normalized;
     

        if (transform.eulerAngles.y % 180 == 0)
        {
            if (_dir == 1)
            {
                anim.SetTrigger("Lslap"); 
            }
            else
            {
                anim.SetTrigger("Rslap");
            }
        }
        else if (transform.eulerAngles.y > 180)
        {
            anim.SetTrigger("Lslap");
         
        }
        else
        {
            anim.SetTrigger("Rslap");
        }

        slap_sfx.Play();

        yield return new WaitForSeconds(0.04f);
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //puppetMaster.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
        //chest.AddForceAtPosition(Vector3.up * UPforce, chest.transform.position, ForceMode.Impulse);
        //if (_dir == 1)
        //{
        //    gameObject.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.right * ForwardForce, gameObject.transform.position, ForceMode.Force);
        //}
        //else
        //{
        //    gameObject.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.right * -ForwardForce, gameObject.transform.position, ForceMode.Force);
        //}

        //  yield return new WaitForSeconds(0.5f);
        //  gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //  yield return new WaitForSeconds(1f);
          Alive();
    }

  
    void Alive()
    {
      //  puppetMaster.enabled = false;
        scenarioManager.onSetupCharacter(meshAgent);
        meshAgent.enabled = true;
      //  puppetMaster.state = RootMotion.Dynamics.PuppetMaster.State.Alive;
      //  puppetMaster.mode = RootMotion.Dynamics.PuppetMaster.Mode.Kinematic;
        anim.SetBool("run",true);
        run = true;
        // gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

   public void OnSetCellPosition()
    {
        run = false;
        StartCoroutine("OnCellPosition");
    }

    IEnumerator OnCellPosition()
    {
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => meshAgent.remainingDistance  < 3);
        meshAgent.Stop();
      
        yield return new WaitForSeconds(1.4f);
        meshAgent.enabled = false;
        anim.SetBool("run", false);
        InCell = true;
    }

    public void OnStop()
    {
        anim.SetBool("run", false);
        run = false;
        meshAgent.enabled = false;
        Vector3 tempPos = chaseTarget.transform.localPosition;
        tempPos.z = tempPos.z - 1;
        chaseTarget.transform.localPosition = tempPos;
      
       
    }
}
