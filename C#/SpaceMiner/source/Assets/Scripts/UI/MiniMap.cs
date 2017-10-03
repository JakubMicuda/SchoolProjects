using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour {

    [SerializeField]
    Image player;

    [SerializeField]
    GameObject playerObj;
    [SerializeField]
    GameObject planetPrefab;

    List<Image> planets = new List<Image>();

    void Update()
    {
        TranslateFunction(playerObj, player);
    }

    public void TranslateFunction(GameObject go, Image obj)
    {
        if (go == null || obj == null)
            return;

        RectTransform trans = obj.gameObject.GetComponent<RectTransform>();

        Vector3 minimapPos = new Vector3(go.transform.position.x / 2.75f, go.transform.position.z / 2.75f, 0);
        trans.localPosition = minimapPos;
        trans.localRotation = Quaternion.Euler(0, 0, -go.transform.rotation.eulerAngles.y);
    }

    public Image InitPlanet(GameObject planet, bool isMined)
    {
        GameObject go = Instantiate(planetPrefab);
        go.transform.SetParent(this.gameObject.transform);
        Image planetImg = go.GetComponent<Image>();
        planetImg.rectTransform.localScale = Vector3.one;
        if (isMined)
            planetImg.color = Color.red;
        else
            planetImg.color = Color.cyan;
        TranslateFunction(planet, planetImg);
        planets.Add(planetImg);
        return planetImg;
    }

    public void DeletePlanets()
    {
        while(planets.Count > 0)
        {
            Destroy(planets[0].gameObject);
            planets.RemoveAt(0);
        }
    }
}
