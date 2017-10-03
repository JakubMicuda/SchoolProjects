using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpaceshipController : MonoBehaviourGM
{
    public GameObject inGameMenu;
    private Rigidbody rb;
    private float rotateSpeed = 25f;
    private float moveSpeed = 20f;
    //public GameObject ship = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (this.gm.leftPlanet)
        {
            this.transform.position = this.gm.enterCoordinates;
            this.transform.localRotation = this.gm.enterRotation;
            this.gm.leftPlanet = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            inGameMenu.SetActive(true);
        this.transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0));
        Vector3 myPos = this.transform.position;
        myPos += this.transform.forward * (Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : 0)* moveSpeed * Time.deltaTime;
        myPos.y = this.transform.position.y;
        this.transform.position = myPos;
    }

    void OnTriggerEnter(Collider coll) {
        
    }
}
