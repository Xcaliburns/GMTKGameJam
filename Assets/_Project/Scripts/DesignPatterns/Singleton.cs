using UnityEngine;

/// <summary>
/// Singleton g�n�rique simple pour MonoBehaviour.
/// Cr�e automatiquement une instance si aucune n'existe dans la sc�ne.
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
                // Cherche une instance existante dans la sc�ne
                _instance = FindFirstObjectByType<T>();

                // Si toujours null, cr�er un nouveau GameObject avec le composant T
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
        // Si une autre instance existe, on se d�truit
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