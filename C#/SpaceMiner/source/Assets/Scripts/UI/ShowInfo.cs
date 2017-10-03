using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShowInfo : MonoBehaviour {

    public List<Planet> targets = new List<Planet>();
    private Image myImage;
    public Text name;
    public Text type;
    public RectTransform myCanvas;

    void Start()
    {
        this.gameObject.SetActive(false);
        myImage = this.gameObject.GetComponent<Image>();
    }

    void Update()
    {
        GameObject target = null;
        Vector3 minPos = Vector3.zero;
        if (this.targets.Count > 0)
        {
            minPos = Camera.main.WorldToScreenPoint(targets[0].transform.position + Vector3.up * 1.2f);
            target = targets[0].gameObject;
            name.text = "Name: " + targets[0].planetName;
            type.text = "Type: " + targets[0].type.ToString();
        }
        for(int i = 1; i < this.targets.Count; i++)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(targets[i].transform.position + Vector3.up * 1.2f);
            if(IsOnScreen(pos) &&
                (pos.z < minPos.z || minPos.z < 0 || !IsOnScreen(minPos)))
            {
                target = targets[i].gameObject;
                name.text = "Name: " + targets[i].planetName;
                type.text = "Type: " + targets[i].type.ToString();
                minPos = pos;
            }
        }
        if (target != null)
        {
            minPos = Camera.main.WorldToScreenPoint(target.transform.position + Vector3.up * 1.2f);
            myImage.rectTransform.position = new Vector3(minPos.x + (0.0439f * Screen.currentResolution.width), minPos.y * (minPos.z / 3 < 1 ? minPos.z / 3 : 1), 0);// * (pos.z / 100 < 1 ? pos.z / 100 : 1), 0);
        }
    }

    private bool IsOnScreen(Vector3 pos)
    {
        return pos.x + (0.0439f * Screen.currentResolution.width) > 0 &&
               pos.x + (0.0439f * Screen.currentResolution.width) < Screen.currentResolution.width &&
               pos.y * (pos.z / 3 < 1 ? pos.z / 3 : 1) > 0 &&
               pos.y * (pos.z / 3 < 1 ? pos.z / 3 : 1) < Screen.currentResolution.height;
    }

    public void DeletePlanets()
    {
        while (this.targets.Count > 0)
            this.targets.RemoveAt(0);
        this.gameObject.SetActive(false);
    }
}
