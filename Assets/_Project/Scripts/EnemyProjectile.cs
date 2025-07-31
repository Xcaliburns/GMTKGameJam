using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public float lifetime = 5f; // How long the projectile lasts before being destroyed
    private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the projectile after its lifetime
        // Get reference to the player's controller
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime); // Move the projectile forward
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            return; // Ignore collision with other enemies
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.nbrShield--; // Decrease the player's shield count
            Destroy(gameObject); 
            return; 
        }       
            Destroy(gameObject); // Destroy the projectile on collision with anything else
        

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!collision.gameObject.CompareTag("Player"))
    //    {

    //        Destroy(gameObject); // Destroy the projectile on collision
    //    }       
    // }
}
