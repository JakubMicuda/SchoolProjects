using UnityEngine;
using System.Collections;

public class Border : MonoBehaviourGM {

    public Transform seEnter;
    public Transform neEnter;
    public Transform nwEnter;
    public Transform swEnter;

    void OnTriggerExit(Collider coll)
    {
        if(coll.transform.position.x >=0 && coll.transform.position.z > 0)
        {
            coll.transform.parent.position = swEnter.position;
            coll.transform.parent.rotation = swEnter.rotation;
            this.gm.EnterSystem("NE");
            Debug.Log("entered ne "+ coll.name);
        }
        else if (coll.transform.position.x >= 0 && coll.transform.position.z <= 0)
        {
            coll.transform.parent.position = nwEnter.position;
            coll.transform.parent.rotation = nwEnter.rotation;
            this.gm.EnterSystem("SE");
            Debug.Log("entered se " + coll.name);
        }
        else if (coll.transform.position.x < 0 && coll.transform.position.z > 0)
        {
            coll.transform.parent.position = seEnter.position;
            coll.transform.parent.rotation = seEnter.rotation;
            this.gm.EnterSystem("NW");
            Debug.Log("entered nw " + coll.name);
        }
        else if (coll.transform.position.x < 0 && coll.transform.position.z <= 0)
        {
            coll.transform.parent.position = neEnter.position;
            coll.transform.parent.rotation = neEnter.rotation;
            this.gm.EnterSystem("SW");
            Debug.Log("entered sw " + coll.name);
        }
    }
}
