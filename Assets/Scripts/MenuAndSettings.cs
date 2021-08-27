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

    public Button playButton;
    public Button HSButton;
    public Button QuitButton;
    public bool workDone = false;

    private void Start()
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

    private void Update()
    {

        if (!workDone && SceneManager.GetSceneByBuildIndex(0) == SceneManager.GetActiveScene())
        {
            SetUpButtons();
            workDone = true;
        }
    }

    void SetUpButtons()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(Play);

        playButton = GameObject.Find("HighScoresButton").GetComponent<Button>();
        playButton.onClick.AddListener(HighScores);

        playButton = GameObject.Find("QuitButton").GetComponent<Button>();
        playButton.onClick.AddListener(Quit);

        inputField = GameObject.Find("InputField").GetComponent<TMP_InputField>();
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
