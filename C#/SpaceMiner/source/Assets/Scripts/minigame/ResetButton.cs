using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviourGM {

    private bool isActive = false;
    [SerializeField]
    private tk2dSprite button;
    [SerializeField]
    private tk2dTextMesh text;

    private GameController pgm;

    void Awake()
    {
        button.SetAlpha(0.5f);
        text.SetAlpha(0.5f);
    }

     void OnMouseDown()
    {
        if (isActive && pgm != null)
            pgm.LeavePlanet();
    }    

    public void Init(GameController pgm)
    {
        this.pgm = pgm;
        //this.text.text = this.gm.currentPlanetType.ToString();
    }

    public void Show()
    {
        isActive = true;
        this.button.AnimateAlpha(button.color.a,1f,0.6f);
        this.text.AnimateAlpha(text.color.a,1f,0.6f);
    }

    public void Hide()
    {
        isActive = false;
        this.button.AnimateAlpha(button.color.a, 0.5f, 0.6f);
        this.text.AnimateAlpha(text.color.a, 0.5f, 0.6f);
    }
}
