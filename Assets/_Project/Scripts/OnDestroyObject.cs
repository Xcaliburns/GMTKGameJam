using UnityEngine;

public class OnDestroyObject : MonoBehaviour
{
    public GameObject animationPrefab;

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
            GameObject fx = Instantiate(animationPrefab, transform.position, Quaternion.identity);
            fx.transform.SetParent(null); // détaché du parent
        }
    }
}
