using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class LyricsSlot : MonoBehaviour, IDropHandler
{
    public bool isFilled;
    public void OnDrop(PointerEventData eventData)
    {
        if (!isFilled)
        {
            Debug.Log("OnDrop");

            if (eventData.pointerDrag != null)
            {
                LyricsManagerNew.instance.lyricsList[LyricsManagerNew.instance.LevelNo].option.Add(eventData.pointerDrag.transform.GetChild(0).GetComponent<TMP_Text>().text);

                //LyricsManagerNew.instance.lyricsList[LyricsManagerNew.instance.LevelNo].option[0] = eventData.pointerDrag.transform.GetChild(0).GetComponent<TMP_Text>().text;

                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                    = GetComponent<RectTransform>().anchoredPosition;

                //eventData.pointerDrag.GetComponent<LyricsBlock>().isPlaced = true;
                eventData.pointerDrag.transform.GetChild(0).position = transform.position;
                eventData.pointerDrag.transform.GetChild(0).SetParent(transform);

                eventData.pointerDrag.gameObject.SetActive(false);

                isFilled = true;

                if (LyricsManagerNew.instance.lyricsList[LyricsManagerNew.instance.LevelNo].option.Count >= 2)
                {
                    LyricsManagerNew.instance.FillLyrics();
                    LyricsManagerNew.instance.LevelNo++;
                    Invoke("LoadNextPanel", LyricsManagerNew.instance.SlideDelay);
                }
            }
        }
        else
        {
            if (eventData.pointerDrag != null)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                    = eventData.pointerDrag.GetComponent<LyricsBlock>().StartPosition;
            }
        }

    }

    public void LoadNextPanel()
    {
        {
            if (LyricsManagerNew.instance.LevelNo < 4)
                LyricsManagerNew.instance.Panel[LyricsManagerNew.instance.LevelNo].GetComponent<Animator>().SetTrigger("panelON");

            if (LyricsManagerNew.instance.LevelNo == 4)
            {
                LyricsManagerNew.instance.OutputScreen.SetActive(true);
                LyricsManagerNew.instance.OutputScreen.GetComponent<Animator>().SetTrigger("panelON");
            }
            if (LyricsManagerNew.instance.LevelNo > 0)
            {
                LyricsManagerNew.instance.Panel[LyricsManagerNew.instance.LevelNo - 1].GetComponent<Animator>().SetTrigger("panelOFF");
            }
            //LyricsManagerNew.instance.Panel[LyricsManagerNew.instance.LevelNo].SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            LyricsManagerNew.instance.OutputScreen.GetComponent<Animator>().SetTrigger("panelON");
        }
    }
}
