using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
//using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviourGM {
    public GameObject mainWindow;
    public Text highScore;

    void Start()
    {
        highScore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
    }

    public void BackToMenu()
    {
        this.mainWindow.SetActive(true);
    }

    public void Customize()
    {
        this.mainWindow.SetActive(false);
    }
    
	public void NewGame()
    {
        int highscore = PlayerPrefs.GetInt("highscore", 0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("highscore", highscore);
        DateTime endTime = DateTime.Now.AddMinutes(3);
        PlayerPrefs.SetString("endTime", endTime.ToString());
        this.gm.Score = 0;
        this.gm.gameEnded = false;
        this.gm.soundTime = 0;
        Application.LoadLevel("Game");
    }

    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }
    public void Exit()
    {
        Application.Quit();
    }
}
