using UnityEngine;
using UnityEngine.Audio;

public class GuidedProjectile : MonoBehaviour
{
    public float speed = 2f;
    public float lifetimeMax = 5f;
    public float rotationSpeed = 5f; // How quickly the projectile adjusts its direction
    private PlayerController playerController;
    private Rigidbody2D rb;
    private float lifetime;
    private Vector3 velocity;
    public GameObject animationPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        lifetime = 0f;

        // Initial direction towards player
        if (playerController != null)
        {
            Vector2 direction = ((Vector2)(playerController.transform.position - transform.position)).normalized;
            rb.linearVelocity = direction * speed;
            
            // Set initial rotation to face the movement direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Track lifetime and destroy when expired
        lifetime += Time.deltaTime;
        if (lifetime >= lifetimeMax)
        {
            Destroy(gameObject);
            return;
        }

        // Update direction to follow player
        if (playerController != null)
        {
            Vector2 directionToPlayer = ((Vector2)(playerController.transform.position - transform.position)).normalized;
            
            // Directly set velocity towards player for continuous tracking
            rb.linearVelocity = directionToPlayer * speed;

            // Update rotation to face movement direction
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision with player or other objects
        if (other.CompareTag("Player"))
        {
            // Deal damage or apply effects to player
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !playerController.isKnockedBack)
            {
                playerController.audioSource.PlayOneShot(playerController.shieldBreakSound);
                player.nbrShield--;
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                player.HandleDamage(knockbackDirection);
            }
            
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Optional: Handle exit logic if needed
        if (collision.CompareTag("Limit"))
        {
            Destroy(gameObject);
        }
    }

 
        void OnDestroy()
        {
            GameObject fx = Instantiate(animationPrefab, transform.position, Quaternion.identity);
            fx.transform.SetParent(null); // détaché du parent
        }

   
}
