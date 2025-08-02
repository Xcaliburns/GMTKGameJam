using UnityEngine;

public class DestroyOnWallTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is tagged as "Wall"
        if (collision.CompareTag("Wall"))
        {
            // Destroy the game object this script is attached to
            Destroy(gameObject);
        }
    }
}
