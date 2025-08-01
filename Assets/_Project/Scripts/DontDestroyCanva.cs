using UnityEngine;

public class DontDestroyCanva : MonoBehaviour
{
    private void Awake()
    {
        // V�rifie s'il existe d�j� une instance de ce Canvas
        if (FindObjectsByType<DontDestroyCanva>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject); // D�truit les doublons
            return;
        }

        // Rend ce GameObject persistant entre les sc�nes
        DontDestroyOnLoad(gameObject);
    }
}
