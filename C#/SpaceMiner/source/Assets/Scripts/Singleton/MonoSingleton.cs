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

    /// <summary>
    /// Vrati false ak uz tento singleton bol instancovany, ak nie vrati true. Slouzi treba do metod ajko disable nebo ondestroy na test.
    /// </summary>
    public static bool IsNull
    {
        get
        {
            if (_instance == null) return true;
            if (MonoBehaviourBase.isQuitingS) return true;
            return false;
        }
    }

    /// <summary>
    /// Vrati refernciu na Singleton. Ak singleton este nebol instanced tak sa vytvori instancia. A zaroven sa zavola OnLoaded().
    /// </summary>
    public static T Instance
    {
        get
        {
            return GetInstance();
        }
    }

    /// <summary>
    /// Vrati refernciu na Singleton. Ak singleton este nebol instanced tak sa vytvori instancia. A zaroven sa zavola OnLoaded().
    /// </summary>
    public static T GetInstance()
    {
        // test jestli je aplikace ukoncovana
        if (MonoBehaviourBase.isQuitingS)
        {
            // jestli je TRUE, tak nekde nesprávnì voláš singelton (OnDisable nebo OnDestroy)
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

                //CDebug.Log(_objName + " initialized!");
            }
        }

        return _instance;
    }

    public override void OnLoaded()
    {

    }

    /// <summary>
    /// Klasicky Awake z Monobehaviouru. Pri dedeni, je nutne volat base.Awake();
    /// </summary>
    protected virtual void Awake()
    {
        // Nastavim aby se nenicil pro zniceni sceny
        DontDestroyOnLoad(this.gameObject);

        // ulozim si info ze se provedlo nastaveni dontdestory, pro kontrolu
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

    /// <summary>
    /// Pri destroy je treba zmazat staticku referenciu ak pointuje na tento objekt.
    /// Milan rikal ze u persistentu je to na hovno
    /// </summary>
    protected override void OnDestroy()
    {
        // Test zda-li se vola spravne awake v potomcich
        if (!this.isSetDontDestroy)
        {
            Debug.LogError("Singleton neprezil scenu, nejspis se nekde u potomka v awake nevola base.Awake() !!");
        }

        base.OnDestroy();

        if (_instance == this)
        {
            _instance = null;
        }
    }

    /// <summary>
    /// Funkcia na uistenie sa, ze singleton bol instanced.
    /// </summary>
    public static bool MakeSureExists()
    {
        if (MonoSingleton<T>.GetInstance() == null) { return false; }
        return true;
    }

}
