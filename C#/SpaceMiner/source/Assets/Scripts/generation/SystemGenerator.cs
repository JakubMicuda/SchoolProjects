using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SystemGenerator : MonoBehaviourGM {
    public List<GameObject> planetTypes = new List<GameObject>();
    private Dictionary<string, string> neighbours = new Dictionary<string, string>();
    public Dictionary<string, string> Neighbours { get { return neighbours; } }
    private List<GameObject> spawnedPlanets = new List<GameObject>();
    private string currentSystemData = null;
    public string currentSystemName = null;
    public MiniMap minimap;
    public ShowInfo info;

    void Awake()
    {
        this.gm.AddGenerator(this);
        this.gm.info = info;
    }

	void Start () {
        this.currentSystemData = PlayerPrefs.GetString("currentSystem");
        if (string.IsNullOrEmpty(currentSystemData))
        {
            currentSystemData = GenerateSystemData();
            PlayerPrefs.SetString("currentSystem", currentSystemData);
        }
        LoadSystem(currentSystemData);
    }

    public void LoadSystemByKey(string key)
    {
        if (!neighbours.ContainsKey(key))
            return;
        LoadSystemData(this.neighbours[key],key);
    }

    void LoadSystemData(string name, string coordinate)
    {
        if (string.IsNullOrEmpty(name))
            return;
        if (!PlayerPrefs.HasKey("System." + name))
            this.currentSystemData = GenerateSystemData(name, this.currentSystemName, coordinate);
        else
            this.currentSystemData = PlayerPrefs.GetString("System." + name);
        PlayerPrefs.SetString("currentSystem", this.currentSystemData);
        LoadSystem(this.currentSystemData);
    }

    void LoadSystem(string systemData)
    {
        ClearPlanets();
        if (string.IsNullOrEmpty(systemData))
        {
            Debug.LogError("systemData non existent");
            return;
        }
        using (StringReader sr = new StringReader(systemData))
        {
            string line;
            line = sr.ReadLine();
            this.currentSystemName = line;
            int planetPos = 1;
            while ((line = sr.ReadLine()) != null)
            {
                string[] lineSplitted = line.Split(':');
                if (lineSplitted.Length < 2)
                    continue;
                if(lineSplitted[0] == "Planet")
                {
                    string[] planetData = lineSplitted[1].Split(';');
                    if (planetData.Length == 4)
                    {
                        Vector2 coords = new Vector2(float.Parse(planetData[1]), float.Parse(planetData[2]));
                        SpawnPlanet(planetData[0], coords, int.Parse(planetData[3]), planetPos);
                        planetPos++;
                    }

                }

                if(lineSplitted[0] == "SE")
                {
                    neighbours["SE"] = lineSplitted[1];
                }
                if (lineSplitted[0] == "NE")
                {
                    neighbours["NE"] = lineSplitted[1];
                }
                if (lineSplitted[0] == "NW")
                {
                    neighbours["NW"] = lineSplitted[1];
                }
                if (lineSplitted[0] == "SW")
                {
                    neighbours["SW"] = lineSplitted[1];
                }
            }

        }
    }

    void SpawnPlanet(string name,Vector2 coordinates,int type,int position)
    {
        //TODO: planet name

        GameObject planetObj = Instantiate(planetTypes[type]);
        planetObj.transform.position = new Vector3(coordinates.x * (position * 20 + 60), 0, coordinates.y * (position * 20 + 60));
        Planet planet = planetObj.GetComponent<Planet>();
        planet.Init(this.currentSystemName, name, (position * 20 + 60),minimap);
        this.spawnedPlanets.Add(planetObj);
    }

    void ClearPlanets()
    {
        while(this.spawnedPlanets.Count > 0)
        {
            Destroy(this.spawnedPlanets[0]);
            this.spawnedPlanets.RemoveAt(0);
        }

        minimap.DeletePlanets();
        info.DeletePlanets();
    }
	
    string GenerateSystemData(string systemName = null, string neighbour = null, string coordinate = null)
    {
        string systemData = "";
        string name;
        if (systemName == null)
            name = GenerateSystemName();
        else
            name = systemName;

        systemData += name + "\r\n";

        int numOfPlanets = Random.Range(4, 10);

        for(int i = 0; i < numOfPlanets; i++)
        {
            string planetData = GeneratePlanetData();
            systemData += planetData + "\r\n";
        }


        string neighName = GenerateSystemName();
        systemData += string.Format("SE:{0}\r\n", Check("NW",neighbour, coordinate) ? neighbour : neighName);

        neighName = GenerateSystemName();
        systemData += string.Format("NE:{0}\r\n", Check("SW", neighbour, coordinate) ? neighbour : neighName);

        neighName = GenerateSystemName();
        systemData += string.Format("NW:{0}\r\n", Check("SE", neighbour, coordinate) ? neighbour : neighName);

        neighName = GenerateSystemName();
        systemData += string.Format("SW:{0}\r\n", Check("NE", neighbour, coordinate) ? neighbour : neighName);

        PlayerPrefs.SetString("System." + name, systemData);
        return systemData;
    }
    bool Check(string key, string neighbour ,string coordinate)
    {
        if (neighbour == null)
            return false;
        else
            return coordinate == key;
    }

    string GeneratePlanetData()
    {
        string planetData = "";
        string planetName = GenerateName();
        Vector2 coordinates = (Vector2)Random.onUnitSphere;
        int planetType = Random.Range(0, planetTypes.Count - 1);

        planetData = string.Format("Planet:{0};{1};{2};{3}", planetName, coordinates.x, coordinates.y, planetType);
        return planetData;
    }

    string GenerateSystemName()
    {
        string name;
        do
        {
            name = GenerateName();
        } while (PlayerPrefs.HasKey("System." + name));

        return name;
    }

    string GenerateName()
    {
        char[] chars = new char[6];
        for (int i = 0; i < chars.Length; i++)
        {
            do
            {
                chars[i] = (char)Random.Range(48, 90);
            } while (chars[i] > 57 && chars[i] < 65);
        }

        return new string(chars);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
