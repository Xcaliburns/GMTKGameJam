using UnityEngine;

public class SimpleSpawnProjectile : MonoBehaviour
{

    public GameObject projectilePrefab; // The projectile prefab to spawn
    public float rotationOffset = 0f; // Rotation offset in degrees
    public Vector2 positionOffset = Vector2.zero; // Position offset from the spawner
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        // Transform the local position offset to world space
        Vector2 worldOffset = (Vector2)transform.TransformDirection(positionOffset);
        
        // Spawn at position with transformed offset
        GameObject projectile = Instantiate(projectilePrefab, (Vector2)transform.position + worldOffset, rotation);
    }
}
