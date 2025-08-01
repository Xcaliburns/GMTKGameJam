using UnityEngine;

/// <summary>
/// Singleton générique simple pour MonoBehaviour.
/// Crée automatiquement une instance si aucune n'existe dans la scène.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Cherche une instance existante dans la scène
                _instance = FindFirstObjectByType<T>();

                // Si toujours null, créer un nouveau GameObject avec le composant T
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // Si une autre instance existe, on se détruit
        if (_instance == null)
        {
            _instance = this as T;
            //DontDestroyOnLoad(gameObject); // Optionnel
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}