using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIElements : MonoBehaviourGM {

    public Text timer;
    public Text score;
    private GameManagerScript gms;
    public DateTime endTime;

	// Use this for initialization
	void Start () {
        this.gms = this.gameObject.GetComponent<GameManagerScript>();
        StartCoroutine(TimeSet());
        this.score.text = this.gm.Score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator TimeSet()
    {
        yield return new WaitForEndOfFrame();

        if (this.timer == null)
            yield break;

        string dateTime = PlayerPrefs.GetString("endTime",null);
        if (string.IsNullOrEmpty(dateTime))
            yield break;
        this.endTime = System.DateTime.Parse(dateTime);
        while (true)
        {

            if (this.timer == null)
                yield break;

            System.TimeSpan diff = this.endTime.Subtract(System.DateTime.Now);

            if (diff.TotalSeconds <= 0)
            {
                this.gms.EndGame();
                yield break;
            }

            this.timer.text = String.Format("{0,2:00}:{1,2:00}:{2,2:00}", diff.Minutes >= 30 ? diff.TotalHours - 1 : diff.TotalHours, diff.Minutes, diff.Seconds);
            yield return new WaitForSeconds(1);
        }
    }
}
