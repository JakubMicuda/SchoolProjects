using UnityEngine;
using System.Collections;

public class GlobalManager : MonoSingleton<GlobalManager> {

    //public PlayerStats playerStats = new PlayerStats();
    public SystemGenerator generator;
    public string currentPlanet = null;
    public Vector3 enterCoordinates = Vector3.zero;
    public Quaternion enterRotation;
    public bool leftPlanet = false;
    public PlanetTypeEnum currentPlanetType { get; private set; }
    public ShowInfo info;
    public int Score { get; set; }
    public bool gameEnded { get; set; }
    public float soundTime = 0;

    public void Init()
    {
        //this.playerStats.Init();
    }

    public void AddGenerator(SystemGenerator gen)
    {
        if(this.generator!= null)
        {
            Debug.LogError("there is already registered generator");
            return;
        }
        this.generator = gen;
    }

    public void EnterSystem(string key)
    {
        generator.LoadSystemByKey(key);
    }

    public void PlanetLanding(PlanetTypeEnum type,string planetName, Vector3 enterCoordinates, Quaternion enterRotation)
    {
        this.currentPlanet = planetName;
        this.enterCoordinates = enterCoordinates;
        this.enterRotation = enterRotation;
        this.currentPlanetType = type;
    }
}
