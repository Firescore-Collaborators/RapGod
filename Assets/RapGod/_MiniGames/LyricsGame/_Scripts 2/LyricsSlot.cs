using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class LyricsSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        if (eventData.pointerDrag != null)
        {

            LyricsManagerNew.instance.LyricsSO[LyricsManagerNew.instance.LevelNo].option.Add(eventData.pointerDrag.transform.GetChild(0).GetComponent<TMP_Text>().text);

//            LyricsManagerNew.instance.LyricsSO[LyricsManagerNew.instance.LevelNo].option[0] = eventData.pointerDrag.transform.GetChild(0).GetComponent<TMP_Text>().text;

            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                = GetComponent<RectTransform>().anchoredPosition;

            //eventData.pointerDrag.GetComponent<LyricsBlock>().isPlaced = true;
            eventData.pointerDrag.transform.GetChild(0).SetParent(transform);
            eventData.pointerDrag.gameObject.SetActive(false);

            if(LyricsManagerNew.instance.LyricsSO[LyricsManagerNew.instance.LevelNo].option.Count >= 2)
            {
                LyricsManagerNew.instance.LevelNo++;
                Invoke("LoadNextPanel", 5);
            }

        }
        
    }

    public void LoadNextPanel()
    {
        //if (LyricsManagerNew.instance.LevelNo <= LyricsManagerNew.instance.LyricsSO.Length)
        {
            LyricsManagerNew.instance.Panel[LyricsManagerNew.instance.LevelNo].SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
