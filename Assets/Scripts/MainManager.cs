using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    
    public Text CurrentPlayerName;
    public Text BestPlayerName;

    public GameObject GameOverText;

    private bool m_Started = false;

    private int m_Points;

    private string CurrentPlayer;
    
    public static string BestPlayer;
    public static int BestScore;

    private bool m_GameOver = false;

    private void Awake()
    {
        LoadGamePlayer();
    }


    // Start is called before the first frame update
    void Start()
    {

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
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
        AddCurrentPlayer();
        SetBestPlayer();

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        PlayerDataHandle.Instance.Score = m_Points;
        ScoreText.text = $"Actual Score : {m_Points}";
    }

    void AddCurrentPlayer()
    {
        CurrentPlayer = PlayerDataHandle.Instance.PlayerName;
        CurrentPlayerName.text = $"Current Player : {CurrentPlayer}";
    }

    

    public void GameOver()
    {
        m_GameOver = true;
        CheckBestPlayer();
        GameOverText.SetActive(true);
    }


    private void CheckBestPlayer()
    {
        int CurrentScore = PlayerDataHandle.Instance.Score;

        if (CurrentScore > BestScore)
        {
            BestPlayer = PlayerDataHandle.Instance.PlayerName;
            BestScore = CurrentScore;

            BestPlayerName.text = $"Best Score - {BestPlayer}: {BestScore}";

            SaveGamePlayer(BestPlayer, BestScore);
        }
    }


    private void SetBestPlayer()
    {
        BestPlayerName.text = $"Best Score - {BestPlayer}: {BestScore}";
        /*if (BestPlayer == null && BestScore == 0)
        {
            BestPlayerName.text = "";
        }
        else
        {
            BestPlayerName.text = $"Best Score - {BestPlayer}: {BestScore}";
            
        }*/

    }


    [System.Serializable]
    class SaveData
    {
        public int Score;
        public string NamePlayer;
    }


public void SaveGamePlayer(string player,int score)
    {
        SaveData data = new SaveData();
        
        data.NamePlayer = player;
        data.Score = score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGamePlayer()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            BestPlayer = data.NamePlayer;
            BestScore = data.Score;

            
        }
        
    }

}
