using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlapAndRun_UiManager : MonoBehaviour
{
    public GameObject[] remark;
    public GameObject failPanle;
    private void OnEnable()
    {
        failPanle.SetActive(false);
    }
    public void OnShowRemark()
    {
        int i = Random.Range(0, remark.Length);
        remark[i].SetActive(true);
    }
   
}
