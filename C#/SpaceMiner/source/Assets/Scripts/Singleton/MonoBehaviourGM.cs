using UnityEngine;
using System.Collections;

public class MonoBehaviourGM : MonoBehaviour
{
    private GlobalManager pgm = null;

    protected GlobalManager gm
    {
        get
        {
            if (!pgm)
            {
                pgm = GlobalManager.Instance;
                pgm.Init();
            }
            return pgm;
        }
    }

    public virtual void OnDestroy()
    {
        this.pgm = null;
    }
}


public class ObjectGM
{
    private GlobalManager pgm = null;
    protected GlobalManager gm
    {
        get
        {
            if (!pgm)
            {
                pgm = GlobalManager.Instance;
                pgm.Init();
            }
            return pgm;
        }
    }

    ~ObjectGM()
    {
        this.pgm = null;
    }

}

