using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomizationWindow : MonoBehaviourGM {

    public Text remainingPoints;
    public StatItem[] items;

    // Use this for initialization
    void Start () {
        UpdateRemainingText();
	}

    public void UpdateRemainingText()
    {
        this.remainingPoints.text = "Remaining Points: " + this.gm.playerStats.RemainingPoints;
        this.CheckPoints();
    }

    void CheckPoints()
    {
        for(int i = 0; i < this.items.Length; i++)
        {
            this.items[i].CheckButtons();
        }
    }

}
