using UnityEngine;
using System.Collections;

public enum OreType
{
    nothing,
    gold,
    silver,
    diamond,
    bronze,
}

public class Ore : MonoBehaviour {
    [SerializeField]
    private tk2dTextMesh textValue = null;
    [SerializeField]
    private tk2dSprite oreTexture;
    [SerializeField]
    private tk2dSprite numBg = null;
    
    private bool pressed = false;
    public bool Pressed { get { return pressed;} }

    private int value = 0;
    public int Value { get { return value;} }

    [SerializeField]
    private OreType type;
    public OreType Type{ get {return type;} }
    public float Width
    {
        get
        {
            return oreTexture.GetBounds().extents.x * 2;
        }
    }

    public float Height
    {
        get
        {
            return oreTexture.GetBounds().extents.y * 2;
        }
    }

    GameController gameController;

    /*void Awake()
    {
        gameController = GameController.Instance;
    }*/

    public void SetGameController(GameController gm)
    {
        gameController = gm;
    }

    void OnMouseDown()
    {
        if (!pressed && gameController.TryController.HasTries)
        {
            pressed = true;
            gameController.Press(this);
        }
    }

    //sets alpha of tk2d objects
    public void OreSetAlpha(float to, float time = 0f)
    {
        if(textValue != null)
            this.textValue.AnimateAlpha(textValue.color.a, to, time);
        this.oreTexture.AnimateAlpha(oreTexture.color.a, to, time);
        if(numBg != null)
            this.numBg.AnimateAlpha(numBg.color.a, to, time);
    }

    //hides and sets value of ore
    public void Init()
    {
        if (textValue != null)
            this.textValue.SetAlpha(0f);
        this.oreTexture.SetAlpha(0f);
        if (numBg != null)
            this.numBg.SetAlpha(0f);

        switch (type)
        {
            case OreType.bronze:
                value = gameController.Values.bronzeValue;
                textValue.text = gameController.Values.bronzeValue.ToString();
            break;

            case OreType.silver:
                value = gameController.Values.silverValue;
                textValue.text = gameController.Values.silverValue.ToString();
            break;

            case OreType.gold:
                value = gameController.Values.goldValue;
                textValue.text = gameController.Values.goldValue.ToString();
            break;

            case OreType.diamond:
                value = gameController.Values.diamondValue;
                textValue.text = gameController.Values.diamondValue.ToString();
            break;

            default: break;
        }
    }
}
