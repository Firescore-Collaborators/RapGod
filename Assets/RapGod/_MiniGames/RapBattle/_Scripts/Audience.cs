using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audience : MonoBehaviour
{
    [SerializeField]
    private GameObject ShootPos;
    [SerializeField]
    private GameObject ThrowablePrefab;
    public void OnShoot( GameObject _target)
    {
        GameObject tempObj =  Instantiate(ThrowablePrefab, ShootPos.transform.position, Quaternion.identity);
        tempObj.GetComponent<ThrowableObject>().target = _target;
    }
}
