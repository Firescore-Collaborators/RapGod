using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrisonControl;

public class PencilMoveScript : MonoBehaviour
{
    public Vector3 screenPoint;
    public Vector3 offset;
    public GameObject WinPanel;

    [SerializeField]
    private int pen_cp, max_cp ;
    private GameObject[] cp;

    public System.Action onReset;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        cp = GameObject.FindGameObjectsWithTag("sign_cp");
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = Vector3.zero;
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        //curPosition.z = curPosition.y;

        //curPosition.y = 0.75f;
        curPosition.y = transform.position.y;

        transform.position = curPosition;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("sign_cp"))
        {
            other.GetComponent<MeshRenderer>().material.color = Color.green;

            if (!other.GetComponent<CPChecker>().isSelected)
            {
                pen_cp++;
            }
            other.GetComponent<CPChecker>().isSelected = true;
            CheckWin();
        }
    }

    IEnumerator ShowWin()
    {
        print("show win");
        yield return new WaitForSeconds(2);
        WinPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        WinPanel.SetActive(false);
        onReset?.Invoke();
    }
    
    void CheckWin()
    {
        if(pen_cp >= max_cp)
        {
            StartCoroutine(ShowWin());
        }    
    }
}
