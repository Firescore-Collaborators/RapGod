using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PasswordSO", menuName = "PasswordCheck/PasswordSO")]
public class PasswordSO : ScriptableObject
{
    public string password;
    public LettersSO letters;
    public string[] format;
}
