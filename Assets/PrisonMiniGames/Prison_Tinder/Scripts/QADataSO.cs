using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QA
{
    public string question;
    public string answer1;
    public string answer2;
    public string chosenAsnwer;
}

[CreateAssetMenu(fileName = "QADataSO", menuName = "Tinder/QADataSO", order = 1)]

public class QADataSO : ScriptableObject
{
    public List<QA> qaList = new List<QA>();
}
