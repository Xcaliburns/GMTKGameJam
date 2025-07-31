using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile prefab to spawn
    public float spawnInterval = 2.0f; // Time interval between spawns
    public float rotationOffset = 0f; // Rotation offset in degrees
    public bool synchronizedFiring = true;

    private float timer = 0f;
    
    void Start()
    {
        if (!synchronizedFiring)
        {
            // Initialize timer with a random value to prevent all spawners firing at once
            timer = Random.Range(0f, spawnInterval);
        }
    }

    void Update()
    {
        // Increment timer
        timer += Time.deltaTime;
        
        // Check if it's time to spawn a projectile
        if (timer >= spawnInterval)
        {
            SpawnProjectile();
            timer = 0f; // Reset timer
        }
    }
    
    void SpawnProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("No projectile prefab assigned!");
            return;
        }
        
        // Combine the parent's current rotation with the rotation offset
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, rotationOffset);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation);
        
        // The EnemyProjectile script handles its own movement in its Update method
        // using Vector2.up * speed * Time.deltaTime
    }
}
