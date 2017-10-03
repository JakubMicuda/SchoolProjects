using UnityEngine;
using System.Collections;

public class ShowInfoCollision : MonoBehaviourGM {

    public Planet myPlanet;

	void OnTriggerEnter(Collider coll)
    {
        if (!myPlanet.isMined)
        {
            this.gm.info.targets.Add(myPlanet);
            this.gm.info.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        this.gm.info.targets.Remove(myPlanet);
        if (this.gm.info.targets.Count == 0)
            this.gm.info.gameObject.SetActive(false);
    }
}
