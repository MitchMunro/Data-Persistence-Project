using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuAndSettings : MonoBehaviour
{
    public static MenuAndSettings Instance;

    public TMP_InputField inputField;
    public string savedPlayerName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeName()
    {

    }

    public void Play()
    {
        savedPlayerName = inputField.text;

        if (savedPlayerName != "")
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("Please enter name.");
        }
    }

    public void HighScores()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
