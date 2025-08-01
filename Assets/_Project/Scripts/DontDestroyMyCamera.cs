using UnityEngine;

public class DontDestroyMyCamera : MonoBehaviour
{
    private static DontDestroyMyCamera instance;

    void Awake()
    {
        // V�rifie s'il existe d�j� une instance de ce script
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // D�truit l'objet en double
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Emp�che la destruction de cet objet entre les sc�nes
    }
}
