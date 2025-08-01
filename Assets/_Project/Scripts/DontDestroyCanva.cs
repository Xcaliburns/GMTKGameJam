using UnityEngine;

public class DontDestroyCanva : MonoBehaviour
{
    private void Awake()
    {
        // Vérifie s'il existe déjà une instance de ce Canvas
        if (FindObjectsByType<DontDestroyCanva>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject); // Détruit les doublons
            return;
        }

        // Rend ce GameObject persistant entre les scènes
        DontDestroyOnLoad(gameObject);
    }
}
