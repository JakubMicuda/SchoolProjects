using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndingObject : MonoBehaviour {

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Text unitsText;
    [SerializeField]
    private GameObject youWin;

    void Start()
    {
        this.youWin.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        unitsText.text = string.Format("Transport package to the Ice planet\n{0} units from destination", (int)Vector3.Distance(player.transform.position, this.transform.position));
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject == player)
        {
            this.youWin.SetActive(true);
            StartCoroutine(Quit());
        }
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
    }
}
