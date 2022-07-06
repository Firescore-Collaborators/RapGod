using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepProgress : MonoBehaviour
{
    public Color correctColor, wrongColor;
    public List<GameObject> progressStep = new List<GameObject>();
    public GameObject progressStepPrefab;
    public ProgressStepUISO progressStepUISO;
    int currentIndex = 0;
    public void Init(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject step = Instantiate(progressStepPrefab, transform);
            if (i == 0)
            {
                step.transform.GetChild(0).GetComponent<Image>().sprite = Utils.NewSprite(progressStepUISO.leftTexture);
            }
            else
            if (i == count - 1)
            {
                step.transform.GetChild(0).GetComponent<Image>().sprite = Utils.NewSprite(progressStepUISO.rightTexture);
            }
            else
            {
                step.transform.GetChild(0).GetComponent<Image>().sprite = Utils.NewSprite(progressStepUISO.centerTexture);
            }
            progressStep.Add(step.transform.GetChild(0).gameObject);
        }
    }

    public void UpdateStep(bool correct)
    {
        progressStep[currentIndex].transform.GetChild(0).GetComponent<Image>().color = correct ?  correctColor : wrongColor;
        currentIndex++;
    }

    public void ActivateCurrentStep()
    {
        progressStep[currentIndex].transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void Reset()
    {
        currentIndex = 0;
        for(int i = 0; i < progressStep.Count; i++)
        {
            Destroy(progressStep[i]);
        }
        progressStep.Clear();
    }
}
