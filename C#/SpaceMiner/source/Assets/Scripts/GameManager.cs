using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviourGM
{
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

    // Use this for initialization
    void Start ()
    {
        this.GetComponent<AudioSource>().time = this.gm.soundTime;
    }

    public void Init()
    {
    }

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

}

