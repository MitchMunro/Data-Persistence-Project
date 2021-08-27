using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public TMP_Text highScoreText;
    public GameObject GameOverMenuPopup;
    
    private bool m_Started = false;
    [SerializeField]
    private int m_Points;
    
    private bool m_GameOver = false;

    public string playerName = "";

    public HighScores highScores;
    public bool scoreAdded = false;

    
    // Start is called before the first frame update
    void Start()
    {
        if (MenuAndSettings.Instance != null)
        {
            playerName = MenuAndSettings.Instance.savedPlayerName;
            MenuAndSettings.Instance.workDone = false;
        }

        LoadOrCreateHighScores();

        highScoreText.text = "";
        highScoreText.text = "High Score: " + highScores.scores[0].Item1
            + " - " + highScores.scores[0].Item2;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (scoreAdded == false)
            {
                AddToHighScore();
                SaveHighScoreList();
                scoreAdded = true;
                print(
                    highScores.scores[0] + ", " +
                    highScores.scores[1] + ", " +
                    highScores.scores[2] + ", " +
                    highScores.scores[3] + ", " +
                    highScores.scores[4] + ", " +
                    highScores.scores[5]
                    );
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                scoreAdded = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }



    void AddToHighScore()
    {
        var scoreList = highScores.scores;
        for (int i = scoreList.Length -1; i > -1; i--)
        {
            if (m_Points > scoreList[i]?.Item1)
            {
                scoreList[i] = new Tuple<int, string>(m_Points, playerName);
                break;
            }
            else if (scoreList[i] == null)
            {
                scoreList[i] = new Tuple<int, string>(m_Points, playerName);
                break;
            }
            
        }

        Array.Sort(scoreList, (x, y) => x.Item1.CompareTo(y.Item1));
        Array.Reverse(scoreList);
      
    }

    void SaveHighScoreList()
    {
        var shs = new SerializableHighScores(highScores);
        string json = JsonUtility.ToJson(shs);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    void LoadOrCreateHighScores()
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
    }
    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverMenuPopup.SetActive(true);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void ToHighScore()
    {
        SceneManager.LoadScene(2);
    }
}

[System.Serializable]
public class HighScores
{
    private static int listLength = 6;
    public Tuple<int, string>[] scores = new Tuple<int, string>[listLength];

    public HighScores()
    {
        for (int i = 0; i < listLength; i++)
        {
            scores[i] = new Tuple<int, string>(0, "");
        }
    }

    public HighScores(SerializableHighScores shs)
    {
        for (int i = 0; i < shs.score.Length; i++)
        {
            scores[i] = new Tuple<int, string>(shs.score[i], shs?.name[i]);
        }
    }
}

[System.Serializable]
public class SerializableHighScores
{
    public int[] score = new int[6];
    public string[] name = new string[6];

    public SerializableHighScores(HighScores hs)
    {
        for (int i = 0; i < hs.scores.Length; i++)
        {
            score[i] = hs.scores[i].Item1;

            if (hs.scores[i].Item2 != null)
            {
                name[i] = hs.scores[i].Item2;
            }
        }
    }
}