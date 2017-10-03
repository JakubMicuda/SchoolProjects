using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;

[Serializable]
public class OreValues
{
    public int bronzeValue;
    public int silverValue;
    public int goldValue;
    public int diamondValue;
}

public class GameController : MonoBehaviourGM{

    [Serializable]
    private class OrePrefabs
    {
        public Ore nothingPrefab;
        public Ore bronzePrefab;
        public Ore silverPrefab;
        public Ore goldPrefab;
        public Ore diamondPrefab;
    }

    [Serializable]
    private class PlanetFields
    {
        public Color Icy;
        public Color Forest;
        public Color Water;
        public Color Desert;
        public Color Tundra;
        public Color Plain;
        public Color Clay;
    }

    [SerializeField]
    private OreValues values = new OreValues();
    public OreValues Values { get { return values;} }

    [SerializeField]
    private OrePrefabs prefabs = new OrePrefabs();

    [SerializeField]
    private PlanetFields planetFields = new PlanetFields();

    [SerializeField]
    private Vector3 initialPosition;
    [SerializeField]
    private int multiplier = 1;

    [SerializeField]
    private ResetButton resetButton;

    [SerializeField]
    private tk2dCamera mainCamera;

    [SerializeField]
    private TryController tryController;
    public TryController TryController { get { return tryController; } }

    [SerializeField]
    private tk2dTextMesh totalValueDisplay;
    private float totalValue = 0;

    [SerializeField]
    private tk2dSprite displaySprite;
    [SerializeField]
    private tk2dSprite mineField;

    private Ore[,] oreArray = new Ore[7, 5];

    private bool hasTries = true;
    public bool HasTries { get { return hasTries;} }

    private GameObject bronzeParent;
    private GameObject silverParent;
    private GameObject goldParent;
    private GameObject diamondParent;
    private GameObject nothingParent;

    private delegate int ApplyOnPosition(Ore orePrefab, int x, int y, int count, GameObject parent = null);
    private delegate int GoDirection(ApplyOnPosition func, int x, int y, int count, Ore orePrefab = null, GameObject parent = null);
    private delegate void ActionOnElement(int x, int y);

    //public Planet currentPlanet { get; private set; }
    private PlanetData data;

    void Start()
    {
        //currentPlanet = new Planet();
        //currentPlanet.InitFromMiniGame(this.gm.generator.currentSystemName, this.gm.currentPlanet);
        //planetName.text = currentPlanet.planetName;
        data = new PlanetData(this.gm.currentPlanetType);
        this.displaySprite.SetSprite("display_" + this.gm.currentPlanetType.ToString());

        switch (this.gm.currentPlanetType)
        {
            case PlanetTypeEnum.Desert: this.mineField.color = planetFields.Desert; break;
            case PlanetTypeEnum.Forest: this.mineField.color = planetFields.Forest; break;
            case PlanetTypeEnum.Clay: this.mineField.color = planetFields.Clay; break;
            case PlanetTypeEnum.Tundra: this.mineField.color = planetFields.Tundra; break;
            case PlanetTypeEnum.Icy: this.mineField.color = planetFields.Icy; break;
            case PlanetTypeEnum.Water: this.mineField.color = planetFields.Water; break;
            case PlanetTypeEnum.Plain: this.mineField.color = planetFields.Plain; break;
        }

        resetButton.Init(this);
        GenerateField(data.data.numOfDiamonds, data.data.numOfGolds, data.data.numOfSilvers, data.data.numOfBronzes);

        tryController.ResetTries(data.data.numOfTries);
        resetButton.Hide();
        totalValue = 0;
        totalValueDisplay.text = "0";
    }

    public void LeavePlanet()
    {
        PlayerPrefs.SetInt(this.gm.generator.currentSystemName + "." + this.gm.currentPlanet, 1);
        this.gm.leftPlanet = true;

        this.gm.Score += (int)totalValue;

        Application.LoadLevel("Game");
    }

    /*public void RegisterMiniGameManager(PlanetMiniGameManager manager)
    {
        this.miniGameManager = manager;
    }

    public void UnregisterMiniGameManager()
    {
        this.miniGameManager = null;
    }*/

    /// <summary>
    /// tries to reveal ore, if there is no try available, it reveals field and shows reset button 
    /// </summary>
    /// <param name="ore">ore to be revealed</param>
    public void Press(Ore ore)
    {
        tryController.UseTry();
        ore.OreSetAlpha(1.0f, 0.6f);
        LeanTween.value(totalValueDisplay.gameObject, UpdateTotalValue, this.totalValue, this.totalValue + multiplier*ore.Value, 0.6f);
        this.totalValue += multiplier*ore.Value;
        if (!tryController.HasTries)
        {
            DoOnArray(SetAlphaOnElement);

            resetButton.Show();
        }   
    }

    //update function for animating value changing
    private void UpdateTotalValue(float value)
    {
        totalValueDisplay.text = ((int)value).ToString();
    }

    /// <summary>
    /// resets whole scene, removing all ores and generating them again
    /// </summary>
    public void Reset(PlanetData data)
    {
        DoOnArray(DestroyElement);

        Destroy(bronzeParent);
        Destroy(silverParent);
        Destroy(goldParent);
        Destroy(diamondParent);
        Destroy(nothingParent);

        GenerateField(data.data.numOfDiamonds, data.data.numOfGolds, data.data.numOfSilvers, data.data.numOfBronzes);

        tryController.ResetTries(data.data.numOfTries);
        resetButton.Hide();
        totalValue = 0;
        totalValueDisplay.text = multiplier.ToString()+"x0=0";
    }

    /// <summary>
    /// Generates playfield
    /// </summary>
    private void GenerateField(int dia = 3, int gold = 3, int silver = 3, int bronze = 3)
    {
        DoOnArray(FillWithNull);

        bronzeParent = GenerateOres(prefabs.bronzePrefab,bronze);
        silverParent = GenerateOres(prefabs.silverPrefab,silver);
        goldParent = GenerateOres(prefabs.goldPrefab,gold);
        diamondParent = GenerateOres(prefabs.diamondPrefab,dia);
        DoOnArray(FillNothing, new GameObject("Nothing Objects"));
    }

    /// <summary>
    /// Looks for empty space on the field and generates ores with the given prefab
    /// </summary>
    /// <param name="orePrefab">prefab of ores being generated</param>
    /// <returns>parent of generated ores</returns>
    private GameObject GenerateOres(Ore orePrefab, int count = 3)
    {
        if (count <= 0)
            return null;
        bool foundSpace = false;
        int x = -1;
        int y = -1;
        while(!foundSpace)
        {
            x = UnityEngine.Random.Range(0,7);
            y = UnityEngine.Random.Range(0,5);
            if(FreeSpacesOnPosition(x,y) >= count)
                foundSpace = true;
        }
        GameObject parent = new GameObject(orePrefab.Type + " Objects");
        GenerateOnPosition(orePrefab,x,y,count,parent);
        return parent;
    }

    /// <summary>
    /// Instantiates Ore object on specific position with given parent
    /// </summary>
    /// <param name="orePrefab">prefab of ore being generated</param>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    /// <param name="parent">optional parent object of the new Ore object</param>
    /// <returns>Ore object of instantiated gameobject</returns>
    private Ore CreateOre(Ore orePrefab, int x, int y, GameObject parent = null)
    {
        GameObject oreObject = (GameObject)LeanTween.Instantiate(orePrefab.gameObject, initialPosition + new Vector3(x*orePrefab.Width, y*orePrefab.Height),orePrefab.transform.rotation);
        oreObject.name = orePrefab.Type.ToString() + " ["+(x+1).ToString()+", "+(y+1).ToString()+"]";
        if(parent != null)
            oreObject.transform.parent = parent.transform;
        Ore ore = oreObject.GetComponent<Ore>();
        ore.SetGameController(this);
        ore.Init();
        return ore;
    }

    /// <summary>
    /// Randomly generates ores next to each other on specific position
    /// </summary>
    /// <param name="orePrefab">prefab of ore object to be generated</param>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    /// <param name="count">count of ores being generated</param>
    /// <param name="parent">parent object of ores being generated</param>
    private void GenerateOnPosition(Ore orePrefab, int x, int y, int count, GameObject parent = null)
    {
        if (x < 0 || x >= 7 || y < 0 || y >= 5)
            return;

        //if space on given position is not empty, return
        if (this.oreArray[x, y] != null)
            return;
        else
        {
            this.oreArray[x, y] = CreateOre(orePrefab,x,y,parent);
            count--;
        }
        
        List<GoDirection> goDirectionFunctions = new List<GoDirection>{ GoLeft, GoRight, GoTop, GoBot };
        int last = 4;
        int index;

        //chooses random function priority order, so ores won`t be always generated in the same way
        while (last > 0)
        {
            index = UnityEngine.Random.Range(0,last);
            count = goDirectionFunctions[index](FillSpaceWithOre, x, y, count, orePrefab, parent); //executes random goDirection function
            if (count <= 0) return;
            goDirectionFunctions.Remove(goDirectionFunctions[index]); //removes executed function from list
            last--;
        }
    }


    /// <summary>
    /// Calculates how many ores can be placed on specific position
    /// </summary>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    /// <returns>count of empty spaces reachable from specific position</returns>
    private int FreeSpacesOnPosition(int x, int y)
    {
        if (x < 0 || x >= 7 || y < 0 || y >= 5)
            return -1;
        int count = 0;
        if (this.oreArray[x, y] != null)
            return -1;
        else count++;
        count = GoLeft(FreeSpaceCount, x, y, count);
        count = GoRight(FreeSpaceCount, x, y, count);
        count = GoBot(FreeSpaceCount, x, y, count);
        count = GoTop(FreeSpaceCount, x, y, count);
        return count;
    }

    /// <summary>
    /// Loops whole oreArray and executes given ActionOnElement function on each element
    /// </summary>
    /// <param name="Action">Action to be executed on each element of the oreArray</param>
    /// <param name="parent">optional parameter parent gameobject</param>
    private void DoOnArray(ActionOnElement Action, GameObject parent = null)
    {
        if(parent != null)
            nothingParent = parent;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Action(i, j);
            }
        }
    }

    /**********************************************GO DIRECTION FUNCTIONS*****************************************************/

    /// <summary>
    /// Loops from position to the left, while no Ore object or left border is reached
    /// </summary>
    /// <param name="Func">Function to be executed each time empty space is found</param>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    /// <param name="count">count of ores available</param>
    /// <param name="orePrefab">optional parameter for instantiating</param>
    /// <param name="parent">optional parameter to be parent of instantiated objects</param>
    /// <returns>count of ores available</returns>
    private int GoLeft(ApplyOnPosition Func, int x, int y, int count, Ore orePrefab = null, GameObject parent = null)
    {
        for (int i = 1; x - i >= 0; i++)
        {
            if (count <= 0)
                return -1;
            if (this.oreArray[x-i, y] == null)
                count = Func(orePrefab,x-i,y,count,parent);
            else break;
        }
        return count;
    }

    /// <summary>
    /// Loops from position to the right, while no Ore object or right border is reached
    /// </summary>
    /// <param name="Func">Function to be executed each time empty space is found</param>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    /// <param name="count">count of ores available</param>
    /// <param name="orePrefab">optional parameter for instantiating</param>
    /// <param name="parent">optional parameter to be parent of instantiated objects</param>
    /// <returns>count of ores available</returns>
    private int GoRight(ApplyOnPosition Func, int x, int y, int count, Ore orePrefab = null, GameObject parent = null)
    {
        for (int i = 1; x + i < 7; i++)
        {
            if (count <= 0)
                return -1;
            if (this.oreArray[x + i, y] == null)
            {
                count = Func(orePrefab, x + i, y, count, parent);
            }
            else break;

        }
        return count;
    }

    /// <summary>
    /// Loops from position to the top, while no Ore object or top border is reached
    /// </summary>
    /// <param name="Func">Function to be executed each time empty space is found</param>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    /// <param name="count">count of ores available</param>
    /// <param name="orePrefab">optional parameter for instantiating</param>
    /// <param name="parent">optional parameter to be parent of instantiated objects</param>
    /// <returns>count of ores available</returns>
    private int GoTop(ApplyOnPosition Func, int x, int y, int count, Ore orePrefab = null, GameObject parent = null)
    {
        for (int i = 1; y + i < 5; i++)
        {
            if (count <= 0)
                return -1;
            if (this.oreArray[x, y + i] == null)
            {
                count = Func(orePrefab, x, y + i, count, parent);
            }
            else break;
        }
        return count;
    }

    /// <summary>
    /// Loops from position to the bottom, while no Ore object or bottom border is reached
    /// </summary>
    /// <param name="Func">Function to be executed each time empty space is found</param>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    /// <param name="count">count of ores available</param>
    /// <param name="orePrefab">optional parameter for instantiating</param>
    /// <param name="parent">optional parameter to be parent of instantiated objects</param>
    /// <returns>count of ores available</returns>
    private int GoBot(ApplyOnPosition Func, int x, int y, int count, Ore orePrefab = null, GameObject parent = null)
    {
        for (int i = 1; y - i >= 0; i++)
        {
            if (count <= 0)
                return -1;
            if (this.oreArray[x, y - i] == null)
            {
                count = Func(orePrefab, x, y - i, count, parent);
            }
            else break;
        }
        return count;
    }

    /****************************************APPLY ON POSITION FUNCTIONS**************************************/

    /// <summary>
    /// Function returns count increased by 1
    /// </summary>
    /// <param name="orePrefab">prefab of given ore object</param>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    /// <param name="count">count of ores available</param>
    /// <param name="parent">optional parameter to be parent of instantiated objects</param>
    /// <returns>count increased by 1</returns>
    private int FreeSpaceCount(Ore orePrefab, int x, int y, int count, GameObject parent = null)
    {
        return count+1;
    }

    /// <summary>
    /// Function creates new Ore on the specific position and decreases count
    /// </summary>
    /// <param name="orePrefab">prefab of the ore object to be instantiated</param>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    /// <param name="count">count of ores available</param>
    /// <param name="parent">optional parameter to be parent of instantiated object</param>
    /// <returns>count of ores available</returns>
    private int FillSpaceWithOre(Ore orePrefab, int x, int y, int count, GameObject parent = null)
    {
        this.oreArray[x, y] = CreateOre(orePrefab, x, y, parent);
        count--;
        return count;
    }


    /*************************************ACTION ON ELEMENT FUNCTIONS**************************************/

    /// <summary>
    /// Fills empty space on specific position with Nothing Object
    /// </summary>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    private void FillNothing(int x, int y)
    {
        if (oreArray[x, y] == null)
            oreArray[x, y] = CreateOre(prefabs.nothingPrefab, x, y, nothingParent);
    }

    /// <summary>
    /// Fills space on specific position with null reference
    /// </summary>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    private void FillWithNull(int x, int y)
    {
        oreArray[x, y] = null;
    }

    /// <summary>
    /// Destroys GameObject on specific position
    /// </summary>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    private void DestroyElement(int x, int y)
    {
        if(oreArray[x,y] != null)
            Destroy(oreArray[x, y].gameObject);
    }

    /// <summary>
    /// Lowers alpha value if Ore element hasn`t been pressed yet
    /// </summary>
    /// <param name="x">first index of the oreArray</param>
    /// <param name="y">second index of the oreArray</param>
    private void SetAlphaOnElement(int x, int y)
    {
        if (!oreArray[x, y].Pressed)
            oreArray[x, y].OreSetAlpha(0.3f, 0.6f);
    }
}
