using UnityEngine;
using System.Collections;

public class MonoBehaviourBase : MonoBehaviour
{

    public virtual void Destroy()
    {

    }

    protected virtual void OnDestroy()
    {

    }

    private bool _isQuiting = false;
    public static bool isQuitingS = false;
    protected bool isQuiting
    {
        get
        {
            return this._isQuiting;
        }
        private set
        {
            MonoBehaviourBase.isQuitingS = this._isQuiting = value;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        this.isQuiting = true;
    }
}
