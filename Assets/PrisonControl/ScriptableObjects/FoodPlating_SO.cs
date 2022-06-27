using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodPlating", menuName = "PrisonControl/FoodPlating", order = 51)]
public class FoodPlating_SO : ScriptableObject
{
    public Prisoner_SO prisoner;

    public string foodRequest;
    public AudioClip aud_foodReq;

    public string positiveResponseBtn;
    public string negetiveResponseBtn;

    public string positiveResponse;
    public string negetiveResponse;

    public AudioClip aud_posResponse;
    public AudioClip aud_negResponse;

    public GameObject pf_plate;

    public List<FoodItems> list_requestFoodItems;
    public List<FoodItems> list_displayFoodItems;

    public enum FoodItems
    {
        Tomato,
        Cucumber,
        Chicken,
        Bread,
        EggSlice,
        Fries,
        Watermelon,
        Poop,
        Cockroach,
        Grasshopper,
        Spider,
    }
}

