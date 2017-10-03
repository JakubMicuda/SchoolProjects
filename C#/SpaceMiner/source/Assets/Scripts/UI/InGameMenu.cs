using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviourGM {

    private DateTime timeWhileActive;
    private float currentTimeScale;
    public UIElements uiEle;
    public Button resumeButton;
    public Text score; 

	// Use this for initialization
	void OnEnable () {
        this.score.text = this.gm.Score.ToString();
        if (this.gm.gameEnded)
            resumeButton.enabled = false;
        this.currentTimeScale = Time.timeScale;
        Time.timeScale = 0;
        timeWhileActive = DateTime.Now;
	}

    public void ReturnToMainMenu()
    {
        int highestScore = PlayerPrefs.GetInt("highscore", 0);
        if (this.gm.Score > highestScore)
            PlayerPrefs.SetInt("highscore", this.gm.Score);
        Application.LoadLevel("MainMenu");
    }
	
	public void Resume()
    {
        this.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        uiEle.endTime = uiEle.endTime.Add(DateTime.Now.Subtract(timeWhileActive));
        PlayerPrefs.SetString("endTime", uiEle.endTime.ToString());
        Time.timeScale = this.currentTimeScale;
    }
    
}
