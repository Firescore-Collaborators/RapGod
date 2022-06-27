using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAndRun_Moving : MonoBehaviour
{

    [SerializeField]
    private Vector2Int X;

    public bool moving_obstacle, rotate_Obstacle;
    float targetx;


    // Start is called before the first frame update
    void Start()
    {
        targetx = X.y;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 tempPos = gameObject.transform.localPosition;

        if (gameObject.transform.localPosition.x >= X.x)
        {
            targetx = X.y;
        }
        else if (gameObject.transform.localPosition.x <= X.y)
        {
            targetx = X.x;
        }
        tempPos.x = targetx;
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, tempPos, 3 * Time.deltaTime);
    }
}
