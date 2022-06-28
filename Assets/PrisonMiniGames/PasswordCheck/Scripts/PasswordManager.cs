using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordManager : MonoBehaviour
{
    public PasswordSO password;
    public GameObject passwordLetterPrefab;
    public GameObject letterPrefab;
    public Transform spawnPanel;
    public Transform letterSpawnParent;
    public Text title;
    public char[] passwordChars;
    public List<Transform> spawnloations = new List<Transform>();
    public List<GameObject> passwordLetters = new List<GameObject>();
    public List<Button> letterButtons = new List<Button>();
    int blankCount = 0;

    public System.Action _OnPasswordDone;

    void Start()
    {
        Init();
    }

    void Init()
    {
        SpawnPassword();
        SpawnLetters();
    }

    void SpawnPassword()
    {
        passwordChars = password.password.ToCharArray();
        for (int i = 0; i < password.password.Length; i++)
        {
            GameObject letter = Instantiate(passwordLetterPrefab, spawnPanel);
            letter.name = passwordChars[i].ToString();
            if(password.format[i] == string.Empty)
            {
                letter.transform.GetChild(0).GetComponent<Text>().text = "_";
                blankCount++;
            }
            else
            {
                letter.transform.GetChild(0).GetComponent<Text>().text = password.format[i];
            }
            //letter.transform.GetChild(0).GetComponent<Text>().text = password.format[i] == string.Empty ? "_" : password.format[i];

            passwordLetters.Add(letter);
        }
    }

    void SpawnLetters()
    {
        for (int i = 0; i < password.letters.letters.Length; i++)
        {
            Button letter = Instantiate(letterPrefab, spawnloations[i]).GetComponent<Button>();
            letter.transform.GetChild(0).GetComponent<Text>().text = password.letters.letters[i];
            letter.onClick.AddListener(() => { CheckLetter(letter); });
            letterButtons.Add(letter);
        }
    }

    void CheckLetter(Button letter)
    {
        string letterText = letter.transform.GetChild(0).GetComponent<Text>().text;
        for (int i = 0; i < passwordLetters.Count; i++)
        {
            if (passwordLetters[i].name == letterText)
            {
                letter.enabled = false;
                letterButtons.Remove(letter);
                SetButtonState(false);
                LerpObjectPosition.instance.LerpObject(letter.transform, passwordLetters[i].transform.position, 0.5f, () =>
                {
                    SetButtonState(true);
                    CheckBlanks();
                });
                passwordLetters[i].name = string.Empty;
                blankCount--;
                return;
            }
        }

        letter.GetComponent<ObjectShake>().Shake();
        print("Letter not found");
    }

    void SetButtonState(bool state)
    {
        foreach (Button button in letterButtons)
        {
            button.interactable = state;
        }
    }

    void CheckBlanks()
    {
        if (blankCount == 0)
        {
            title.text = "PASSWORD CORRECT";
            for(int i = 0; i < letterButtons.Count; i++)
            {
                Destroy(letterButtons[i].gameObject);
            }
            letterButtons.Clear();

            Timer.Delay(1, () =>
            {
                _OnPasswordDone?.Invoke();
            });
        }
    }

}
