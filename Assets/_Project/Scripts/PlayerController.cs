using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    
    public bool IsAttacking { get; private set; }
    public float attackDuration = 0.25f; // How long the attack lasts
    private float attackTimer = 0f;
    public float knockbackForce = 1f; // Force applied when defending against an attack
    public float knockbackPushDuration = 0.2f; // Duration of the initial push
    public float knockbackStunDuration = 0.3f; // Duration of the stun after push
    
    // Knockback state
    private bool isKnockedBack = false;
    private bool isInPushPhase = false;
    private float knockbackTimer = 0f;
    private Rigidbody2D rb;

    // Reference to the sword hitbox
    public GameObject swordHitbox;

    
    public float moveSpeed = 5f; // Speed of the player movement
    public int nbrSword = 1; // Number of swords the player has
    public int nbrShield = 1; // Number of shields the player has

    private bool attackInput = false;

    void Start()
    {
        // Get reference to rigidbody
        rb = GetComponent<Rigidbody2D>();

        // Make sure sword is initially inactive
        if (swordHitbox != null)
        {
            swordHitbox.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Sword hitbox reference not set in PlayerController!");
        }
    }

    void Update()
    {
        // Check for attack input in Update for better responsiveness
        if (Input.GetKeyDown(KeyCode.Space) && !isKnockedBack)
        {
            attackInput = true;
        }

        // Handle attack timer and sword visibility
        if (IsAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                IsAttacking = false;
                // Hide sword when attack ends
                if (swordHitbox != null)
                {
                    swordHitbox.SetActive(false);
                    Debug.Log("Attack ended, hiding sword");
                }
            }
        }

        // Handle knockback timer
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            
            // Check if we need to transition from push phase to stun phase
            if (isInPushPhase && knockbackTimer <= knockbackStunDuration)
            {
                isInPushPhase = false;
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero; // Stop the push force
                    Debug.Log("Push phase ended, transitioning to stun phase");
                }
            }
            
            // Check if knockback is completely over
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
                isInPushPhase = false;
                // Stop any remaining knockback force when knockback ends
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                }
                Debug.Log("Knockback ended, player can move again");
            }
        }
    }

    void FixedUpdate()
    {
        // Only allow movement if not being knocked back
        if (!isKnockedBack)
        {
            PlayerMovement();
        }

        // Process attack input in FixedUpdate
        if (attackInput && !isKnockedBack)
        {
            PlayerAttack();
            attackInput = false;
        }
    }

    void PlayerMovement()
    {
        // Get the horizontal and vertical input axes
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Only rotate and move if there's actual input
        if (movement.magnitude > 0.1f)
        {
            // Normalize the movement vector to prevent faster diagonal movement
            if (movement.magnitude > 1f)
            {
                movement.Normalize();
            }

            // Calculate rotation based on movement direction
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            // Apply rotation to face the movement direction
            // Subtract 90 degrees because typically sprites face up by default
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            // Move the player
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0), Space.Self);
        }
    }

    void PlayerAttack()
    {
        // Check if the player has any swords
        if (nbrSword > 0 && !IsAttacking)
        {
            // Start attack
            IsAttacking = true;
            attackTimer = attackDuration;

            // Show sword during attack
            if (swordHitbox != null)
            {
                swordHitbox.SetActive(true);
                Debug.Log("Attack started, showing sword");
            }

            Debug.Log("Player attacks with a sword!");
            nbrSword--; // Decrease the number of swords after an attack

            // Update UI
            DavidUIManager.Instance.UpdateUI();
        }
        else if (nbrSword <= 0)
        {
            Debug.Log("No swords left to attack!");
        }
    }

    void PlayerDefend(Vector2 hitDirection)
    {
        // Check if the player has any shields and is not already knocked back
        if (nbrShield > 0 && !isKnockedBack)
        {
            // Implement defend logic here
            Debug.Log("Player defends with a shield!");
            nbrShield--; // Decrease the number of shields after defending

            // Apply knockback force in the opposite direction of the hit
            if (rb != null)
            {
                // Enter knockback state
                isKnockedBack = true;
                isInPushPhase = true;
                knockbackTimer = knockbackPushDuration + knockbackStunDuration;

                // Normalize the direction and invert it to push away from the hit
                Vector2 knockbackDirection = -hitDirection.normalized;

                // Apply the force as an impulse                
                rb.linearVelocity = Vector2.zero; // Reset velocity before applying force
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                Debug.Log("Player knocked back in direction: " + knockbackDirection + ", push phase beginning");
            }
        }
        if (nbrShield <= 0)
        {
            Debug.Log("Player is dead");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the player collides with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collision detected!");

            // Calculate hit direction from the collision contact point
            Vector2 hitDirection = collision.contacts[0].point - (Vector2)transform.position;

            if (nbrShield > 0)
            {
                PlayerDefend(hitDirection);
            }
            else
            {
                Debug.Log("Player is dead!");
            }
        }
    }

    // For trigger collisions in 2D (if your colliders are set as triggers)
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);

        // Check if the player collides with an enemy
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy trigger detected!");

            // Calculate hit direction for triggers
            Vector2 hitDirection = other.transform.position - transform.position;

            if (nbrShield > 0)
            {
                PlayerDefend(hitDirection);
            }
            else
            {
                Debug.Log("Player is dead!");
            }
        }
    }
}
