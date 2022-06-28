using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionText : MonoBehaviour
{
    [SerializeField]
    private GameObject[] target;

    int targetNo;

    Vector3 initialPos;
    private void Start()
    {
        initialPos = gameObject.transform.position;
    }

    public void OnClicked()
    {
        gameObject.transform.SetParent(null);
        LeanTween.move(gameObject, target[targetNo].transform.position, 0.3f);
    }

    public void OnSettargetNo()
    {
        targetNo++;
    }

    private void OnDisable()
    {
        gameObject.transform.position = initialPos;
    }
}
