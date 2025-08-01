using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Attack properties
    public bool IsAttacking { get; private set; }
    public float attackDuration = 0.25f;
    private float attackTimer = 0f;
    private bool attackInput = false;
    private bool currentAttackHasHit = false;


    // Magic properties
    public bool IsMagicOnCooldown { get; private set; }
    public float magicCooldown = 0.5f;
    private float magicCooldownTimer = 0f;
    private bool magicProjectileInput = false;

    // Knockback properties
    public float knockbackForce = 1f;
    public float knockbackPushDuration = 0.2f;
    public float knockbackStunDuration = 0.3f;
    public bool isKnockedBack = false;
    private bool isInPushPhase = false;
    private float knockbackTimer = 0f;

    // Movement properties
    public float moveSpeed = 5f;

    // Resources
    public int nbrSword = 5;
    public int nbrShield = 5;
    public int nbrMagic = 5;

    // References
    private Rigidbody2D rb;
    public GameObject swordHitbox;
    public GameObject MagicProjectile;
    public float projectileOffset = 1f;

    public bool IsPlayerAlive { get; private set; } = true;

    // Invulnerability properties
    public float invulnerabilityDuration = 0.5f;
    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
        HandleInput();
        UpdateTimers();
    }

    void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            PlayerMovement();
        }

        if (attackInput && !isKnockedBack)
        {
            PlayerAttack();
            attackInput = false;
        }

        if (magicProjectileInput && !isKnockedBack)
        {
            ShootMagicProjectile();
            magicProjectileInput = false;
        }
        if (nbrShield < 0 || nbrMagic <0)
        {
            DavidUIManager.Instance.PlayerDied();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isKnockedBack)
        {
            attackInput = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            magicProjectileInput = true;
        }
    }

    void UpdateTimers()
    {
        UpdateAttackTimer();
        UpdateMagicCooldownTimer();
        UpdateKnockbackTimer();
        UpdateInvulnerabilityTimer();
    }

    void UpdateAttackTimer()
    {
        if (IsAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                IsAttacking = false;
                if (swordHitbox != null)
                {
                    swordHitbox.SetActive(false);
                    Debug.Log("Attack ended, hiding sword");
                }
            }
        }
    }

    void UpdateMagicCooldownTimer()
    {
        if (IsMagicOnCooldown)
        {
            magicCooldownTimer -= Time.deltaTime;
            if (magicCooldownTimer <= 0)
            {
                IsMagicOnCooldown = false;
                Debug.Log("Magic ready to use again");
            }
        }
    }

    void UpdateKnockbackTimer()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;

            if (isInPushPhase && knockbackTimer <= knockbackStunDuration)
            {
                isInPushPhase = false;
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    Debug.Log("Push phase ended, transitioning to stun phase");
                }
            }

            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
                isInPushPhase = false;
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                }
                Debug.Log("Knockback ended, player can move again");
            }
        }
    }

    void UpdateInvulnerabilityTimer()
    {
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
                Debug.Log("Player is no longer invulnerable");
            }
        }
    }

    void PlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        if (movement.magnitude > 0.1f)
        {
            if (movement.magnitude > 1f)
            {
                movement.Normalize();
            }

            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0), Space.Self);
        }
    }

    void PlayerAttack()
    {
        if (nbrSword > 0 && !IsAttacking)
        {
            IsAttacking = true;
            attackTimer = attackDuration;
            currentAttackHasHit = false;

            if (swordHitbox != null)
            {
                swordHitbox.SetActive(true);

            }
        }
        else if (nbrSword <= 0)
        {
            Debug.Log("No swords left to attack!");
        }
    }

    public void RegisterSwordHit()
    {
        if (IsAttacking && !currentAttackHasHit)
        {
            currentAttackHasHit = true;
            nbrSword--;

            DavidUIManager.Instance.UpdateUI();
        }
    }

    public bool CanDefend()
    {
        return nbrShield > 0 && !isKnockedBack;
    }

    public void PlayerDefend(Vector2 hitDirection)
    {
        if (!CanDefend())
        {
            if (IsPlayerAlive)
            {
                Debug.Log("Player is dead!");
                IsPlayerAlive = false;
                // Call game manager to handle player death
                if (DavidUIManager.Instance != null)
                {
                    DavidUIManager.Instance.PlayerDied();
                }
            }
        }

        DavidUIManager.Instance.UpdateUI();

        isKnockedBack = true;
        isInPushPhase = true;
        knockbackTimer = knockbackPushDuration + knockbackStunDuration;

        if (rb != null)
        {
            Vector2 knockbackDirection = -hitDirection.normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            Debug.Log("Player knocked back in direction: " + knockbackDirection);
        }
    }

    public void HandleDamage(Vector2 hitDirection)
    {
        if (isInvulnerable)
        {
            Debug.Log("Player is invulnerable, ignoring damage");
            return;
        }

        if (CanDefend())
        {
            PlayerDefend(hitDirection);
        }
        else
        {
            DavidUIManager.Instance.PlayerDied();
        }

        // Activer l'invulnérabilité
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Enemy") && collision.collider.gameObject == this.gameObject && !isKnockedBack)
        {
            Vector2 hitDirection = collision.contacts[0].point - (Vector2)transform.position;
            nbrShield--;
            HandleDamage(hitDirection);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);

        if (other.CompareTag("SpikedTrap") && !isKnockedBack)
        {
            if (nbrSword > 0) nbrSword--;
            if (CanDefend()) PlayerDefend(Vector2.zero);
            DavidUIManager.Instance.UpdateUI();
        }
        else if (other.CompareTag("CursedAxe") && !isKnockedBack)
        {
            Vector2 hitDirection = other.transform.position - transform.position;
            if (nbrSword > 0)
            {
                nbrSword--;
                DavidUIManager.Instance.UpdateUI();
            }
            if (CanDefend()) PlayerDefend(hitDirection);
        }
        else if (other.CompareTag("Pit"))
        {
            Debug.Log("Player fell into a pit!");
            // Call game manager to handle pit fall
            if (DavidUIManager.Instance != null)
            {
                DavidUIManager.Instance.PlayerDied();
            }
        }
        else if (other.CompareTag("Enemy") && other.gameObject.GetComponent<Collider2D>() == GetComponent<Collider2D>() && !isKnockedBack)
        {
            Vector2 hitDirection = other.transform.position - transform.position;
            nbrShield--;
            HandleDamage(hitDirection);
        }
    }

    void ShootMagicProjectile()
    {
        if (MagicProjectile != null && nbrMagic > 0 && !IsMagicOnCooldown)
        {
            Vector3 spawnPosition = transform.position + transform.up * projectileOffset;
            GameObject projectile = Instantiate(MagicProjectile, spawnPosition, transform.rotation);
            nbrMagic--;

            IsMagicOnCooldown = true;
            magicCooldownTimer = magicCooldown;
            DavidUIManager.Instance.UpdateUI();

            Debug.Log("Magic projectile fired!");
        }
        else if (IsMagicOnCooldown)
        {
            Debug.Log("Magic on cooldown!");
        }
        else if (nbrMagic <= 0)
        {
            Debug.Log("Out of magic charges!");
        }
        else
        {
            Debug.LogWarning("Magic Projectile prefab reference not set in PlayerController!");
        }
    }
}
