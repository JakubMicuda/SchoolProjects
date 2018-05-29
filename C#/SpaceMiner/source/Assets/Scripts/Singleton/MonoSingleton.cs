using UnityEngine;
using System.Collections;


public abstract class IMonoSingleton : MonoBehaviourBase
{
    public abstract void OnLoaded();
    public virtual void OnLoaded(string _loadedSceneID)
    {

    }
}

public class MonoSingleton<T> : IMonoSingleton where T : IMonoSingleton
{
    protected static T _instance;
    private bool isSetDontDestroy = false;

    public static bool IsNull
    {
        get
        {
            if (_instance == null) return true;
            if (MonoBehaviourBase.isQuitingS) return true;
            return false;
        }
    }

    public static T Instance
    {
        get
        {
            return GetInstance();
        }
    }

    public static T GetInstance()
    {
        if (MonoBehaviourBase.isQuitingS)
        {
            Debug.LogWarning("Application Is Quitting !!! " + MonoBehaviourBase.isQuitingS);
            return null;
        }

        if (_instance == null)
        {
            _instance = FindObjectOfType(typeof(T)) as T;

            if (_instance == null)
            {
                string prefabName = "#" + typeof(T).ToString();
                string _objName = "#(SP)" + typeof(T).ToString();
                GameObject _obj = new GameObject(_objName);
                _instance = _obj.AddComponent<T>();

                _instance.OnLoaded();
                
            }
        }

        return _instance;
    }

    public override void OnLoaded()
    {

    }
    
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        this.isSetDontDestroy = true;

        if (_instance == null)
            _instance = this as T;
        else
            Destroy(gameObject);
    }

    public override void Destroy()
    {
        base.Destroy();

        if (gameObject != null)
            Destroy(gameObject);

        _instance = null;
    }
   
    protected override void OnDestroy()
    {
        if (!this.isSetDontDestroy)
        {
            Debug.LogError("Singleton neprezil scenu, asi sa niekde u potomka v awake nevola base.Awake() !!");
        }

        base.OnDestroy();

        if (_instance == this)
        {
            _instance = null;
        }
    }
    
    public static bool MakeSureExists()
    {
        if (MonoSingleton<T>.GetInstance() == null) { return false; }
        return true;
    }

}
