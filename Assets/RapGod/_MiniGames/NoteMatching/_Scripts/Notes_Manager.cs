using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes_Manager : MonoBehaviour
{
    public static Notes_Manager instance;
    public Transform endpoint1, endpoint2;
    public GameObject WinPanel, squareParent;

    public AudioSource SRC;
    public AudioClip clip1, clip2, clip3;

    public GameObject[] grid, box;
    public Tiles_SO tiles_SO;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    int j;
    private void OnEnable()
    {
        for (int i = 0; i < grid.Length; i++)
        {
            if (tiles_SO.box[i] != null)
            {
                box[j] = Instantiate(tiles_SO.box[i], grid[i].transform.position, Quaternion.identity, squareParent.transform);
                j++;
            }
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

    IEnumerator ShowWinPanel()
    {
        yield return new WaitForSeconds(2);
        WinPanel.SetActive(true);
    }

    public void showPanel()
    {
        StartCoroutine(ShowWinPanel());
    }
}