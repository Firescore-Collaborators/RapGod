using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisitorSO", menuName = "PhoneBooth/VisitorSO")]
public class VisitorSO : ScriptableObject
{
    public List<string> dialogues = new List<string> ();
    public string statementText;
    public bool isGuilty;
    [HideInInspector]
    public Cameras vCamera;

    public List<AudioClip> aud_dialogs = new List<AudioClip>();
    public AudioClip aud_statement;
}
