using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuitcaseBribe", menuName = "PrisonControl/SuitcaseBribe", order = 51)]
public class SuitcaseBribe_SO : ScriptableObject
{
    [SerializeField]
    private GameObject suitcase;

    [SerializeField]
    private BribeTypes bribeType;

    [SerializeField]
    private string msg;

    [SerializeField]
    private int totalBundles;

    public AudioClip aud_intro;


    public enum BribeTypes
    {
        Money,
        Gold,
        Other
    }

    public GameObject ReturnSuitcasePrefab()
    {
        return suitcase;
    }

    public BribeTypes ReturnBribeType()
    {
        return bribeType;
    }

    public string ReturnMessage()
    {
        return msg;
    }

    public int ReturnTotalBundles()
    {
        return totalBundles;
    }
}
