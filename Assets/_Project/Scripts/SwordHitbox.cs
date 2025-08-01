using UnityEngine;
using System.Collections;

public class SwordHitbox : MonoBehaviour
{
    private PlayerController playerController;
    
    void Start()
    {
        // Get reference to the parent's PlayerController
        playerController = GetComponentInParent<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogError("SwordHitbox could not find PlayerController in parents!");
        }
        
        // Make sure this is a trigger collider
        if (GetComponent<Collider2D>())
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only process collisions during attack
        if (playerController != null && playerController.isAttacking && other.CompareTag("Enemy"))
        {
            Debug.Log("Sword hit enemy: " + other.gameObject.name);
            playerController.RegisterSwordHit(); // Register the hit with the player controller
            Destroy(other.gameObject);
        }
    }
}