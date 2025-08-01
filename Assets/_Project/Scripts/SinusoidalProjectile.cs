using UnityEngine;

public class SinusoidalProjectile : MonoBehaviour
{
    public float speed = 2f;
    public float lifetimeMax = 15f;
    public float amplitude = 1f;     // How wide the sine wave is
    public float frequency = 2f;     // How many oscillations per second
    
    private PlayerController playerController;
    private Rigidbody2D rb;
    private float lifetime;
    private float sinusoidalTime;    // Time tracker for sine wave
    private Vector2 initialDirection; // Store initial direction
    private Vector2 perpendicularDirection; // Store perpendicular for wave

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        lifetime = 0f;
        sinusoidalTime = 0f;

        // Initial direction towards player (just for aiming)
        //if (playerController != null)
        //{
        //    initialDirection = ((Vector2)(playerController.transform.position - transform.position)).normalized;
        //    perpendicularDirection = new Vector2(-initialDirection.y, initialDirection.x);
            
        //    // Set initial rotation to face the movement direction
        //    float angle = Mathf.Atan2(initialDirection.y, initialDirection.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //}
        //else
        //{
            // Default direction if no player is found
            initialDirection = transform.right; // Always move right
            perpendicularDirection = transform.up;
       // }
    }

    void Update()
    {
        // Track lifetime and destroy when expired
        lifetime += Time.deltaTime;
        sinusoidalTime += Time.deltaTime;
        
        if (lifetime >= lifetimeMax)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate sine wave offset
        float sineValue = Mathf.Sin(sinusoidalTime * frequency * 2f * Mathf.PI);
        Vector2 sineOffset = perpendicularDirection * sineValue * amplitude;
        
        // Apply sinusoidal movement along fixed direction
        rb.linearVelocity = (initialDirection * speed) + sineOffset;
        
        // Update rotation to face movement direction
        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if(player.isKnockedBack)
                {
                    return; // Ignore if player is already knocked back
                }
                playerController.nbrShield--;
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                player.HandleDamage(knockbackDirection);
            }
            
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Limit"))
        {
            Destroy(gameObject);
        }
    }
}