using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab; // The projectile prefab to spawn
    public float spawnInterval = 2.0f; // Time interval between spawns
    public float rotationOffset = 0f; // Rotation offset in degrees
    public float randomizerDirectionFactor = 3f;
    public bool synchronizedFiring = true;
    
    // Salvo parameters
    public bool useSalvo = false;
    public int salvoCount = 3;
    public float salvoDelay = 0.1f;

    private float timer = 0f;
    private bool firingInProgress = false;
    
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
        // Only increment timer if not already firing a salvo
        if (!firingInProgress)
        {
            // Increment timer
            timer += Time.deltaTime;
            
            // Check if it's time to spawn a projectile
            if (timer >= spawnInterval)
            {
                if (useSalvo)
                {
                    StartCoroutine(FireSalvo());
                }
                else
                {
                    SpawnProjectile();
                }
                timer = 0f; // Reset timer
            }
        }
    }
    
    IEnumerator FireSalvo()
    {
        firingInProgress = true;
        
        for (int i = 0; i < salvoCount; i++)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(salvoDelay);
        }
        
        firingInProgress = false;
    }
    
    void SpawnProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("No projectile prefab assigned!");
            return;
        }

        float randomSpread = Random.Range(-randomizerDirectionFactor, randomizerDirectionFactor);

        // Combine the parent's current rotation with the rotation offset
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, rotationOffset + randomSpread);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation);
    }
}
