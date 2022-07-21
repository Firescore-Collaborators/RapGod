using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CustomizationManager : MonoBehaviour
{
    [SerializeField] Transform bling, hair, body;
    [SerializeField] List<GameObject> tabs;
    [SerializeField] List<GameObject> blingTypes;
    [SerializeField] List<GameObject> hairTypes;
    [SerializeField] List<GameObject> bodyTypes;
    void Start()
    {
        Init();
    }

    void Init()
    {
        //Bling
        for (int i = 0; i < bling.childCount; i++)
        {
            Transform child = bling.GetChild(i);
            //add function to event listener
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { BlingSelect(child.GetSiblingIndex()); });
            child.GetComponent<EventTrigger>().triggers.Add(entry);
        }

        //Hair
        for (int i = 0; i < hair.childCount; i++)
        {
            Transform child = hair.GetChild(i);
            //add function to event listener
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { HairSelect(child.GetSiblingIndex()); });
            child.GetComponent<EventTrigger>().triggers.Add(entry);
        }

        //Body
        for (int i = 0; i < body.childCount; i++)
        {
            Transform child = body.GetChild(i);
            //add function to event listener
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { BodySelect(child.GetSiblingIndex()); });
            child.GetComponent<EventTrigger>().triggers.Add(entry);
        }
    }

    void BlingSelect(int index)
    {
        DeselectBling();
        blingTypes[index].SetActive(true);
    }

    void HairSelect(int index)
    {
        DeselectHair();
        hairTypes[index].SetActive(true);
    }
    void BodySelect(int index)
    {
        DeselectBody();
        bodyTypes[index].SetActive(true);
    }

    public void TabSelect(int index)
    {
        DeselectAll();
        tabs[index].transform.GetChild(1).GetComponent<Image>().enabled = true;
        switch (index)
        {
            case 0:
                hair.gameObject.SetActive(true);
                break;
            case 1:
                body.gameObject.SetActive(true);
                break;
            case 2:
                bling.gameObject.SetActive(true);
                break;

        }
    }

    void DeselectAll()
    {
        foreach (GameObject tab in tabs)
        {
            tab.transform.Find("Selected").GetComponent<Image>().enabled = false;
        }
        bling.gameObject.SetActive(false);
        hair.gameObject.SetActive(false);
        body.gameObject.SetActive(false);
    }

    void DeselectBling()
    {
        foreach (GameObject blingType in blingTypes)
        {
            blingType.SetActive(false);
        }
    }

    void DeselectHair()
    {
        foreach (GameObject hairType in hairTypes)
        {
            hairType.SetActive(false);
        }
    }
    void DeselectBody()
    {
        foreach (GameObject bodyType in bodyTypes)
        {
            bodyType.SetActive(false);
        }
    }
}
