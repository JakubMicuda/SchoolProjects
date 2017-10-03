using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatItem : MonoBehaviourGM {

    public PlayerStatsType statType;
    public Button plusButton;
    public Button minusButton;
    public Text currentValue;
    public float increaseValue = 10;
    public int maxPoints = 10;
    public CustomizationWindow window;
    private int currentPoints = 0;
    
    void OnEnable()
    {
        plusButton.onClick.AddListener(AddPoint);
        minusButton.onClick.AddListener(RemovePoint);
    }

    void Start()
    {
        UpdateValue();
        CheckButtons();
    }

    void OnDisable()
    {
        plusButton.onClick.RemoveListener(AddPoint);
        minusButton.onClick.RemoveListener(RemovePoint);
    }

    void AddPoint()
    {
        float stat = this.gm.playerStats.GetStat(this.statType);
        this.gm.playerStats.SetStat(this.statType, stat + this.increaseValue);
        this.gm.playerStats.RemainingPoints--;
        this.currentPoints++;
        this.CheckButtons();
        this.window.UpdateRemainingText();
        UpdateValue();
    }

    void UpdateValue()
    {
        this.currentValue.text = this.gm.playerStats.GetStat(this.statType).ToString();
    }

    public void CheckButtons()
    {
        if (this.currentPoints >= this.maxPoints || this.gm.playerStats.RemainingPoints <= 0)
            this.plusButton.enabled = false;
        else if (!this.plusButton.enabled)
            this.plusButton.enabled = true;

        if (this.currentPoints <= 0)
            this.minusButton.enabled = false;
        else if (!this.minusButton.enabled)
            this.minusButton.enabled = true;
    }

    void RemovePoint()
    {
        float stat = this.gm.playerStats.GetStat(this.statType);
        this.gm.playerStats.SetStat(this.statType, stat - this.increaseValue);
        this.gm.playerStats.RemainingPoints++;
        this.currentPoints--;
        this.CheckButtons();
        this.window.UpdateRemainingText();
        UpdateValue();
    }
     
}
