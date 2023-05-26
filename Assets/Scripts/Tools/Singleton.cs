using UnityEngine;

/*
    This is a tool that can be inherited by any Game Object,
    will guarantee that there will only be 1 instance allowed in the scene,
    and provide quick access to that instance through the static variable Instance

    A Singleton is an object for which only 1 instance can exist.

    Abstract classes can not be instantiated, but describe common traits and
    behaviours of all the classes that inherit it.

    We use the Generic type "<T>" here which allows access to the variables and functions
    declared in inherited classes after referencing the instance through this Singleton Instance
*/
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //  Static reference to the Singleton, private so it can't be altered
    private static T _instance;

    //  Getter to easily get the Singleton Instance statically, if it exists
    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("There is no " + typeof(T).ToString() + " in the scene.");

            return _instance;
        }
    }
    //  When an instance of this Singleton is created, we need to make sure
    //  it doesn't already exists.  If it does, self-destruct.
    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        //  Here we cast the instance type to the Generic type T which will be set by inheriters
        _instance = this as T;
    }
    //  When the Singleton is destroyed, no reference to it can be allowed to exist
    protected void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}