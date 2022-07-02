using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Notes_Manager : MonoBehaviour
{
    public static Notes_Manager instance;
    public Transform endpoint1, endpoint2;

    public GameObject[] grid, box;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public int sum;
    public void Update()
    {
      
    }

    public void CubeCheck()
    {
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < box.Length; j++)
            {
                if (grid[i].transform.position == box[j].transform.position)
                {
                    if (grid[i].GetComponent<SlotScript>().SlotCode == box[j].GetComponent<UIMoveScript>().boxCode)
                    {
                        sum++;
                        Debug.Log(sum);
                    }
                }
            }
        }
    }
}