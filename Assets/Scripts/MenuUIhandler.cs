using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class MenuUIhandler : MonoBehaviour
{


    [SerializeField] Text PlayerNameInput;
    public Text MenuPlayer;

    private string BestPlayer;
    private int BestScore;

    public void Awake()
    {
        LoadGamePlayer();
    }

    public void Start()
    {
        Show();
    }

    public void startScene()
    {
        SceneManager.LoadScene("main");

    }

    public void Show()
    {
        MenuPlayer.text = $"Best Player : {BestPlayer}         Best Score : {BestScore}";
    }


    public void SetPlayerName()
    {
        PlayerDataHandle.Instance.PlayerName = PlayerNameInput.text;
        
    }



    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class LoadPlayer
    {
        public int Score;
        public string NamePlayer;
    }

    public void LoadGamePlayer()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LoadPlayer data = JsonUtility.FromJson<LoadPlayer>(json);

            BestPlayer = data.NamePlayer;
            BestScore = data.Score;


        }

    }
}


