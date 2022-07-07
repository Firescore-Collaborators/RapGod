using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudienceManager : MonoBehaviour
{
    public GameObject AudienceContainer;
    public int audienceNo, playerNo, enemyNo;
    public float playerFollower, enemyFollower;


    [SerializeField]
    private GameObject PlayerContainer, EnemeyContainer;

    [SerializeField]
    private Image progressBar, follower_img;

    [SerializeField]
    private Text playerFollower_text, EnemyFollower_text;


    public GameObject EnemyHeadTarget, PlayerHeadTarget;

    [SerializeField]
    private Color initialCol;

    [SerializeField]
    private List<GameObject> PlayerAudience, EnemyAudience = new List<GameObject>();

    private float playerFollowerTarget, enemyFollowerTarget;
    float progressbarTarget;
    bool playerWin;

    // Start is called before the first frame update
    void Start()
    {
        progressbarTarget = 0.5f;
        playerFollowerTarget = progressbarTarget * 100;
        enemyFollowerTarget = 0.6f * 100;

        playerNo = 0;
        enemyNo = 0;
        OnGameEnd();
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.fillAmount = LeanTween.linear(progressBar.fillAmount, progressbarTarget, 0.05f);
        playerFollower = LeanTween.linear(playerFollower, playerFollowerTarget, 0.2f);
        enemyFollower = 60;// LeanTween.linear(enemyFollower, enemyFollowerTarget, 0.2f);

        playerFollower_text.text = "" + playerFollower.ToString("F1") + " k";
        EnemyFollower_text.text = "" + enemyFollower.ToString("F1") + " k";

    }

    public void OnMovePlayerSide(int _no)
    {
        //audienceNo++;
        for (int i = 0; i < AudienceContainer.transform.childCount; i++)
        {
            AudienceContainer.transform.GetChild(i).gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Cheering");
        }

        //for (int i = 0; i < 10; i++)
        //{
        //    if (audienceNo >= AudienceContainer.transform.childCount)
        //    {
        //        // Invoke("OnShoot", 2);
        //        break;
        //    }

        //    AudienceContainer.transform.GetChild(audienceNo).gameObject.AddComponent<AudienceMove>().targetPos = PlayerContainer.transform.GetChild(playerNo).transform.position;
        //    PlayerAudience.Add(AudienceContainer.transform.GetChild(audienceNo).gameObject);
        //    audienceNo++;
        //    playerNo++;
        //}

        //  progressbarTarget = ((float)playerNo) / ((float)(playerNo + enemyNo));
        // progressbarTarget = progressbarTarget + 0.1f;
        // playerFollowerTarget = progressbarTarget * 100;
        // enemyFollowerTarget = (1 -progressbarTarget ) * 100;

        // StartCoroutine(OnFollowerChange(Color.green));

    }

    public void OnMoveEnemySide(int _no)
    {
        //audienceNo++;
        //for (int i = 0; i < AudienceContainer.transform.childCount; i++)
        //{
        //    AudienceContainer.transform.GetChild(i).gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Cheering");
        //}

        //for (int i = 0; i < 5; i++)
        //{
        //    if (audienceNo >= AudienceContainer.transform.childCount)
        //    {
        //      //  Invoke("OnShoot", 2);
        //         break;
        //    }
        //    AudienceContainer.transform.GetChild(audienceNo).gameObject.AddComponent<AudienceMove>().targetPos = EnemeyContainer.transform.GetChild(enemyNo).transform.position;
        //    EnemyAudience.Add(AudienceContainer.transform.GetChild(audienceNo).gameObject);
        //    audienceNo++;
        //    enemyNo++;
        //}

        //   progressbarTarget = ((float)playerNo) / ((float)(playerNo + enemyNo));
        // progressbarTarget = progressbarTarget - 0.1f;
        // playerFollowerTarget = progressbarTarget * 100;
        // enemyFollowerTarget = (1 - progressbarTarget) * 100;

        // StartCoroutine(OnFollowerChange(Color.white));

    }

    public void OnGameEnd()
    {
        for (int i = 0; i < AudienceContainer.transform.childCount; i++)
        {
            AudienceContainer.transform.GetChild(i).gameObject.transform.GetChild(1).GetComponent<Animator>().Play("CheeringLoop");
        }
    }

    public void Reset()
    {
        for (int i = 0; i < AudienceContainer.transform.childCount; i++)
        {
            AudienceContainer.transform.GetChild(i).gameObject.transform.GetChild(1).GetComponent<Animator>().Play("Idle");
        }
    }

    public void OnShoot()
    {
        StartCoroutine(ThrowObject());
    }


    public bool CheckWinner(bool _playerWin)
    {
        playerWin = _playerWin;

        if (playerWin)
        {
            //Invoke("OnShoot", 2f);
        }
        else
        {
            //Invoke("OnShoot", 0.0f);
        }
        return true;

    }

    IEnumerator ThrowObject()
    {
        // GameObject target = null;

        for (int i = 0; i < 15; i++)
        {
            AudienceContainer.transform.GetChild(i).GetComponent<Audience>().OnShoot(playerWin ? EnemyHeadTarget : PlayerHeadTarget);
            //  Instantiate(throwablePrefab,)
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator OnFollowerChange(Color _col)
    {
        playerFollower_text.color = _col;
        // follower_img.transform.localScale;
        LeanTween.scale(follower_img.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.6f);
        yield return new WaitForSeconds(0.6f);
        LeanTween.scale(follower_img.gameObject, Vector3.one, 0.4f);
        yield return new WaitForSeconds(0.5f);
        playerFollower_text.color = initialCol;
    }

}
