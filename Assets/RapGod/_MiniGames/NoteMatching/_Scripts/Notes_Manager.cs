using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrisonControl;
public class Notes_Manager : MonoBehaviour
{
    public static Notes_Manager instance;
    public PlayPhasesControl playPhasesControl;
    public Transform endpoint1, endpoint2;
    public GameObject WinPanel, squareParent, PlayBar;
    public int temp;
    public AudioSource SRC;
    public AudioClip clip1, clip2, clip3;

    public GameObject[] grid, box;
    public TilesSO tiles_SO;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    int j;
    private void OnEnable()
    {
        InitLevelData();
        Init();
    }

    void InitLevelData()
    {
        Level_SO level = playPhasesControl.levels[Progress.Instance.CurrentLevel - 1];
        tiles_SO = level.GetTilesSO;
    }
    void Init()
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
        yield return new WaitForSeconds(4);
        //WinPanel.SetActive(true);
        PlayBar.SetActive(true);
        PlayBar.GetComponent<Animator>().SetTrigger("play");
        Timer.Delay(5f, () =>
        {
            LevelComplete();
        });
    }

    public void showPanel()
    {
        StartCoroutine(ShowWinPanel());
    }

    public void PlayTheBar()
    {
        if (temp < 4)
        {
            temp++;
            PlayBar.SetActive(true);
            PlayBar.GetComponent<Animator>().SetTrigger("play");
        }
        else
        {
            PlayBar.SetActive(false);
        }
    }

    void LevelComplete()
    {
        Reset();
        playPhasesControl._OnPhaseFinished();
    }

    void Reset()
    {
        for (int i = 0; i < box.Length; i++)
        {
            Destroy(box[i]);
        }
        box = new GameObject[12];
        PlayBar.SetActive(true);

    }
}