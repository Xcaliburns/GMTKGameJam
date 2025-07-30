using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Add these new variables
    public bool IsAttacking { get; private set; }
    public float attackDuration = 0.25f; // How long the attack lasts
    private float attackTimer = 0f;
    
    // Reference to the sword hitbox
    public GameObject swordHitbox;
    
    // Your existing variables
    public float moveSpeed = 5f; // Speed of the player movement
    public int nbrEpees = 1; // Number of swords the player has
    public int nbrBouclier = 1; // Number of shields the player has
    
    private bool attackInput = false;

    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        // Check for attack input in Update for better responsiveness
        if (Input.GetKeyDown(KeyCode.Space))
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
    }

    void FixedUpdate()
    {
        PlayerMovement();
        
        // Process attack input in FixedUpdate
        if (attackInput)
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
        if (nbrEpees > 0 && !IsAttacking)
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
            nbrEpees--; // Decrease the number of swords after an attack
            
            // Update UI
            DavidUIManager.Instance.UpdateUI();
        }
        else if (nbrEpees <= 0)
        {
            Debug.Log("No swords left to attack!");
        }
    }
    
    void PlayerDefend()
    {
        // Check if the player has any shields
        if (nbrBouclier > 0)
        {
            // Implement defend logic here
            Debug.Log("Player defends with a shield!");
            nbrBouclier--; // Decrease the number of shields after defending
            Debug.Log("Shields left: " + nbrBouclier);
        }
        if (nbrBouclier <= 0)
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
            if (nbrBouclier > 0)
            {
                PlayerDefend();
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
            if (nbrBouclier > 0)
            {
                PlayerDefend();
            }
            else
            {
                Debug.Log("Player is dead!");
            }
        }
    }
}