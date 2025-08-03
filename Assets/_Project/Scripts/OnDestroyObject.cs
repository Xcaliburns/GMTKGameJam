using UnityEngine;
using UnityEngine.Audio;

public class OnDestroyObject : MonoBehaviour
{
    public GameObject animationPrefab;
    public AudioClip deathSound;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Only one OnDestroy method is allowed
    void OnDestroy()
    {
        if (animationPrefab != null)
        {
            audioSource.PlayOneShot(deathSound);
            GameObject fx = Instantiate(animationPrefab, transform.position, Quaternion.identity);
            fx.transform.SetParent(null); // détaché du parent
        }
    }
}
