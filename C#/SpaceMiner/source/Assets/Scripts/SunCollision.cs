using UnityEngine;
using UnityEngine.UI;

public class SunCollision : MonoBehaviour {

    public GameManagerScript gms;
    public Image pauseShader;
	// Use this for initialization
	void OnTriggerEnter(Collider coll)
    {
        this.pauseShader.color = new Color(1, 0, 0, 0.6f);
        gms.EndGame();
    }
}
