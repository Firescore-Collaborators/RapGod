using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrisonControl;
public class SlapAndRun_PlayerController : MonoBehaviour
{
  
    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject RslapCollider, LslapCollider;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject TouchControl, playerObj, camera, cameraInitialPos,targetRun,cameraRun,cameraIdle;

    [SerializeField]
    private SlapAndRun_RotateToward playerRoatation;

  //  [SerializeField]
    public SlapAndRun_UiManager uiManager;

   // [SerializeField]
    public ScenarioManager scenarioManager;
    public GameObject chaseTarget;

    GameObject CopPos;
    bool CellState;
    bool dead;
    public int SlapCounter;

    [SerializeField]
    private PrisonControl.CopOutfit_SO CopOutfit_SO;

    [SerializeField]
    private SkinnedMeshRenderer body, bottom, hair, hat, shoes, Tie, Top;
    bool MaleOutfit;
    bool OnCameraAnimation;
    Vector3 cameraRotaionInitial,cameraPositionInitial;

    public SlapAndRunStep slapAndRunStep;
    public System.Action OnGameStarted;

    private void OnEnable()
    {
        Application.targetFrameRate = 60;
        cameraPositionInitial = camera.transform.localPosition;
        camera.transform.position = cameraInitialPos.transform.position;
        cameraRotaionInitial = camera.transform.localEulerAngles;
        camera.transform.localEulerAngles = cameraInitialPos.transform.localEulerAngles;
        OnCameraAnimation = true;
        SetUpOutFit();
    }

    void SetUpOutFit()
    {
        int OutFitNo = 1;
        MaleOutfit = true;

        if(MaleOutfit)
        {
            if (CopOutfit_SO.maleOutfit.outfits[OutFitNo].body != null)
            {
                body.sharedMesh = CopOutfit_SO.maleOutfit.outfits[OutFitNo].body;
                body.material = CopOutfit_SO.maleOutfit.outfits[OutFitNo].body_mat;
            }
            else
            {
                body.gameObject.SetActive(true);
            }

            if (CopOutfit_SO.maleOutfit.outfits[OutFitNo].bottom != null)
            {
                bottom.sharedMesh = CopOutfit_SO.maleOutfit.outfits[OutFitNo].bottom;
                bottom.materials[0] = CopOutfit_SO.maleOutfit.outfits[OutFitNo].bottom_mat;
            }
            else
            {
                bottom.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.maleOutfit.outfits[OutFitNo].hair != null)
            {
                hair.sharedMesh = CopOutfit_SO.maleOutfit.outfits[OutFitNo].hair;
                hair.materials[0] = CopOutfit_SO.maleOutfit.outfits[OutFitNo].hair_mat;
            }
            else
            {
                hair.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.maleOutfit.outfits[OutFitNo].hat != null)
            {
                hat.sharedMesh = CopOutfit_SO.maleOutfit.outfits[OutFitNo].hat;
                hat.materials[0] = CopOutfit_SO.maleOutfit.outfits[OutFitNo].hat_mat;
            }
            else
            {
                hat.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.maleOutfit.outfits[OutFitNo].shoes != null)
            {
                shoes.sharedMesh = CopOutfit_SO.maleOutfit.outfits[OutFitNo].shoes;
                shoes.materials[0] = CopOutfit_SO.maleOutfit.outfits[OutFitNo].shoes_mat;
            }
            else
            {
                shoes.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.maleOutfit.outfits[OutFitNo].Tie != null)
            {
                Tie.sharedMesh = CopOutfit_SO.maleOutfit.outfits[OutFitNo].Tie;
                Tie.materials[0] = CopOutfit_SO.maleOutfit.outfits[OutFitNo].Tie_mat;
            }
            else
            {
                Tie.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.maleOutfit.outfits[OutFitNo].Top != null)
            {
                Top.sharedMesh = CopOutfit_SO.maleOutfit.outfits[OutFitNo].Top;
                Top.materials[0] = CopOutfit_SO.maleOutfit.outfits[OutFitNo].Top_mat;
            }
            else
            {
                Top.gameObject.SetActive(false);
            }

        }
        else
        {
            if (CopOutfit_SO.femaleOutfit.outfits[OutFitNo].body != null)
            {
                body.sharedMesh = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].body;
                body.material = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].body_mat;
            }
            else
            {
                body.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.femaleOutfit.outfits[OutFitNo].bottom != null)
            {
                bottom.sharedMesh = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].bottom;
                bottom.materials[0] = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].bottom_mat;
            }
            else
            {
                bottom.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.femaleOutfit.outfits[OutFitNo].hair != null)
            {
                hair.sharedMesh = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].hair;
                hair.materials[0] = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].hair_mat;
            }
            else
            {
                hair.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.femaleOutfit.outfits[OutFitNo].hat != null)
            {
                hat.sharedMesh = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].hat;
                hat.materials[0] = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].hat_mat;
            }
            else
            {
                hat.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.femaleOutfit.outfits[OutFitNo].shoes != null)
            {
                shoes.sharedMesh = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].shoes;
                shoes.materials[0] = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].shoes_mat;
            }
            else
            {
                shoes.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.femaleOutfit.outfits[OutFitNo].Tie != null)
            {
                Tie.sharedMesh = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].Tie;
                Tie.materials[0] = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].Tie_mat;
            }
            else
            {
                Tie.gameObject.SetActive(false);
            }

            if (CopOutfit_SO.femaleOutfit.outfits[OutFitNo].Top != null)
            {
                Top.sharedMesh = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].Top;
                Top.materials[0] = CopOutfit_SO.femaleOutfit.outfits[OutFitNo].Top_mat;
            }
            else
            {
                Top.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        Invoke("OnTutorialShow", 5f);
    }

    private void  OnTutorialShow()
    {
       
         OnGameStarted?.Invoke();
    }

   public void OnStartRun()
    {
        cameraRun.SetActive(true);
        cameraIdle.SetActive(false);
        TouchControl.GetComponent<Lean.Touch.LeanDragTranslate>().enabled = true;
        Debug.Log("game started");
        OnRun();
    }


    void LateUpdate()
    {
        if (dead)
            return;

        if (!CellState )
        {
            if (!OnCameraAnimation)
                transform.position = Vector3.MoveTowards(gameObject.transform.position, targetRun.transform.position, speed * Time.deltaTime);
        }
        else
        {
            Vector3 tempPos = CopPos.transform.position;
            tempPos.y = transform.position.y;

            if (Vector3.Distance(gameObject.transform.position, tempPos) < 1)
            {
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0, 180, 0), speed * Time.deltaTime);
                anim.SetBool("run", false);
                return;
            }
          
            playerObj.transform.localPosition = Vector3.MoveTowards(playerObj.transform.localPosition,new Vector3(0, -0.57f,0), speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(gameObject.transform.position, tempPos, speed * Time.deltaTime);
        }

        if (OnCameraAnimation)
        {
            camera.transform.eulerAngles = cameraInitialPos.transform.eulerAngles;
            camera.transform.position = cameraInitialPos.transform.position;
            // Vector3 tempPos = cameraPositionInitial;
            //// tempPos.z = 0;
            // camera.transform.transform.localPosition = Vector3.MoveTowards(camera.transform.transform.localPosition, tempPos, 40 * Time.deltaTime);
            // camera.transform.localEulerAngles = Vector3.MoveTowards(camera.transform.localEulerAngles, Vector3.zero, 10 * Time.deltaTime);

            // if (Vector3.Distance( camera.transform.transform.localPosition, tempPos) <= 0)
            // {
            //     OnCameraAnimation = false;
            //     camera.transform.transform.localPosition = cameraPositionInitial;
            //     camera.transform.localEulerAngles = cameraRotaionInitial;
            //     OnStartRun();
            // }
        }
    }

    public void OnRun()
    {
       
        OnCameraAnimation = false;
        anim.SetBool("run", true);
    }

    public void OnRightSlap()
    {
        SlapCounter++;
       
       StopCoroutine("CoolDownSlapCounter");
        if (SlapCounter > 3)
        {
           // uiManager.OnShowRemark();
           // SlapCounter = 0;
        }
        else
        {
            StartCoroutine("CoolDownSlapCounter");
        }
        anim.SetTrigger("slap_R");
    }

    public void OnLeftSlap()
    {
        
        SlapCounter++;
       
         StopCoroutine("CoolDownSlapCounter");
        if (SlapCounter > 3)
        {
           // uiManager.OnShowRemark();
           // SlapCounter = 0;
        }
        else
        {
            StartCoroutine("CoolDownSlapCounter");
        }
        anim.SetTrigger("slap_L");
    }

    GameObject prev_obj;
    public void FindTarget(GameObject _obj)
    {
        if(prev_obj == _obj)
        {
            return;
        }
        prev_obj = _obj;
        // Get direction from A to B
        Vector3 posA = anim.transform.position;
        Vector3 posB = _obj.transform.position;
        //Destination - Origin
        Vector3 dir = (posB - posA).normalized;
       
      
        if (dir.x > 0)
        {
            OnRightSlap();
        }
        else
        {
            OnLeftSlap();
        }
    }

    public void OnCellState(GameObject _cellPos, GameObject _prison)
    {
      
        if(CellState)
        {
            return;
        }
        cameraRun.SetActive(false);
        cameraIdle.SetActive(true);
        camera.SetActive(false);
        playerRoatation.enabled = false;
        TouchControl.GetComponent<Lean.Touch.LeanDragTranslate>().enabled = false;
      //  TouchControl.SetActive(false);
        CopPos = _cellPos;
        CellState = true;
        scenarioManager.OnSetTarget(_prison);
    }

    IEnumerator CoolDownSlapCounter()
    {
      
        while (SlapCounter != 0)
        {
            yield return new WaitForSeconds(1);
            SlapCounter--;
        }
    }

    public void OnDead()
    {
        if(dead)
        {
            return;
        }
        
        Invoke("OnRestart", 3f);
        uiManager.failPanle.SetActive(true);
        scenarioManager.OnStop();
      //  chaseTarget.transform.SetParent(null);
        dead = true;
        anim.SetBool("dead", true);
    }

    void OnRestart()
    {
        slapAndRunStep.OnFailed();
    }
}
