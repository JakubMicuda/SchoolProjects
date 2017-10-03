using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviourGM
{
    public class Planet
    {
        public GameObject model;
        public GameObject collider;

        public Planet (GameObject m, GameObject c)
        {
            model = m;
            collider = c;
        }
    }

    public static GameManagerScript _instance;

    public GameObject playerSpaceship;
    public GameObject GlobalTranslate;
    public GameObject LocalTranslate;
    public GameObject Star;
    public Transform FarCameraT;
    public Transform CloseCameraT;
    public GameObject menu;
    public Light directionalLight;
    private Vector3 globalRegion;
    private const int regionSize = 8000;

    //public Light directionalLight;

    #region variables
    public List<Planet> _planets;
    private System.Random _rnd;
    private bool _crateSpawned;
    private bool _enemySpawned;

    public PlayerStats playerStats;
    #endregion

    // Use this for initialization
    void Start ()
    {   
        //Random.InitState(1);
        _crateSpawned = false;
        _enemySpawned = false;

        playerStats = new PlayerStats();
        playerStats.Init();
        if (playerSpaceship && GlobalTranslate && Star)
        {
            _instance = this;
            Debug.Log("Game started!");
        } else
        {
            Debug.Log("This manager is not real?");
        }
        this.GetComponent<AudioSource>().time = this.gm.soundTime;
    }

    public void Init()
    {
    }
	
	/*// Update is called once per frame
	void Update ()
    {

        int translateX = (int)(playerSpaceship.transform.position.x / regionSize);
        int translateY = (int)(playerSpaceship.transform.position.y / regionSize);
        int translateZ = (int)(playerSpaceship.transform.position.z / regionSize);
        if (translateX != 0 || translateY != 0 || translateZ != 0) {
            Debug.Log("Region += " + translateX + ", " + translateY + ", " + translateZ);
        }

        TranslateGlobal(-translateX, -translateY, -translateZ);

        directionalLight.transform.rotation = Quaternion.LookRotation((playerSpaceship.transform.position / 8000) - Star.transform.position, Vector3.up);
    }*/

    void LateUpdate()
    {
        FarCameraT.localRotation = CloseCameraT.rotation;
        bool tooClose = FarCameraT.localPosition.magnitude < 3;
        if(!tooClose && (CloseCameraT.position / 5).magnitude >= 3)
            FarCameraT.localPosition = CloseCameraT.position / 5;
    }

    void OnDisable()
    {
        this.gm.soundTime = this.GetComponent<AudioSource>().time;
    }

    public void EndGame()
    {
        this.gm.gameEnded = true;
        this.menu.SetActive(true);
    }


    /*
    private void TranslateGlobal(int x, int y, int z)
    {
        LocalTranslate.transform.Translate(x * regionSize, y * regionSize, z * regionSize, Space.World);
        playerSpaceship.transform.Translate(x * regionSize, y * regionSize, z * regionSize, Space.World);
        GlobalTranslate.transform.Translate(x, y, z, Space.World);
    }
    */

}

