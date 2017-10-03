using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum PlanetTypeEnum
{
    Desert,
    Water,
    Icy,
    Forest,
    Clay,
    Plain,
    Tundra
}

public class Planet : MonoBehaviourGM {

    public string planetName;
    private Outline outline;
    [SerializeField]
    private Material disabledState;
    public bool isMined = false;
    private Collider myCollider;
    private Renderer myRenderer;
    public PlanetTypeEnum type;
    private MiniMap minimap;
    private Image minimapObj;


    float angle = 0;
    float speed = (2 * Mathf.PI) / 5; //2*PI in degress is 360, so you get 5 seconds to complete a circle
    float radius = 5;

    void Update()
    {
        angle += speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
        Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        this.transform.position = pos;
        
        if(this.minimap != null)
            this.minimap.TranslateFunction(this.gameObject, this.minimapObj);
    }

    public void InitFromMiniGame(string system, string planetName)
    {
        this.planetName = planetName;
    }

	// Use this for initialization
	public void Init(string system, string planetName, int radius, MiniMap minimap)
    {
        this.outline = this.gameObject.GetComponent<Outline>();
        this.myCollider = this.gameObject.GetComponent<Collider>();
        this.myRenderer = this.gameObject.GetComponent<Renderer>();
        this.radius = radius;
        this.angle = Mathf.Asin(this.gameObject.transform.position.x / radius);
        this.speed = (2 * Mathf.PI) / Random.Range(radius * 2, radius * 5);


        if (PlayerPrefs.HasKey(system + "." + planetName))
            this.isMined = PlayerPrefs.GetInt(system + "." + planetName, 0) == 1;
        else
            PlayerPrefs.SetInt(system + "." + planetName, 0);

        if (this.isMined)
        {
            this.myCollider.enabled = false;
            this.outline.color = 2;
            this.myRenderer.sharedMaterial = disabledState;
        }

        this.planetName = planetName;

        this.minimapObj = minimap.InitPlanet(this.gameObject, this.isMined);
        this.minimap = minimap;
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider coll)
    {
        if (this.isMined)
            return;
        this.gm.PlanetLanding(type, planetName, coll.transform.parent.position, coll.transform.parent.localRotation);
        Application.LoadLevel("planetMiniGame");
    }
}
