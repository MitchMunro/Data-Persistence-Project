using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;


public class HSMenu : MonoBehaviour
{
    public TMP_Text highScoresText;
    private HighScores highScores = new HighScores();

    private void Awake()
    {
        LoadHighScores();
        MenuAndSettings.Instance.workDone = false;
        
    }

    public void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var shs = JsonUtility.FromJson<SerializableHighScores>(json);
            highScores = new HighScores(shs);
        }
        else
        {
            highScores = new HighScores();
        }

        highScoresText.text = "";

        for (int i = 0; i < highScores.scores.Length; i++)
        {
            highScoresText.text += highScores.scores[i].Item2 + " - "
                + highScores.scores[i].Item1 + "\n";
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void EraseHighScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        File.Delete(path);
        LoadHighScores();
    }
}
