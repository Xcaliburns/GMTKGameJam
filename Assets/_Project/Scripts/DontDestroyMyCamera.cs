using UnityEngine;

public class DontDestroyMyCamera : MonoBehaviour
{
    private static DontDestroyMyCamera instance;

    void Awake()
    {
        // Vérifie s'il existe déjà une instance de ce script
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Détruit l'objet en double
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Empêche la destruction de cet objet entre les scènes
    }
}
