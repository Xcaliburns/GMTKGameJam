using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Stats attaque")]
    public GameObject swordPrefab;
    public float attackDuration = 0.12f;
    public float attackSpeedMultiplier = 0.3f;
    public bool isAttacking = false;
    private bool attackInput = false;
    public bool currentAttackHasHit = false;

    [Header("Stats magie")]
    public bool IsMagicOnCooldown { get; private set; }
    public float magicCooldown = 0.5f;
    private float magicCooldownTimer = 0f;
    private bool magicProjectileInput = false;

    [Header("Stats knockback")]
    public float knockbackForce = 1f;
    public float knockbackPushDuration = 0.2f;
    public float knockbackStunDuration = 0.3f;
    public bool isKnockedBack = false;
    private bool isInPushPhase = false;
    private float knockbackTimer = 0f;

    [Header("Stats d�placement")]
    public float moveSpeed = 5f;
    public float acceleration = 15f;
    public float friction = 10f;

    private Vector2 velocity;
    private Vector2 inputDir;
    private Vector2 lastMoveDir = Vector2.right;

    [Header("Ressources")]
    public int nbrSword = 5;
    public int nbrShield = 5;
    public int nbrMagic = 5;

    [Header("Misc")]
    private Rigidbody2D rb;
    public GameObject MagicProjectile;
    public AudioSource audioSource;
    public float projectileOffset = 1f;

    [Header("Player Sounds")]
    public AudioClip attackSuccessSound;
    public AudioClip attackFailSound;
    public AudioClip magicSound;
    public AudioClip shieldBreakSound;
    public AudioClip swordBreakSound;
    public AudioClip magicBreakSound;
    public AudioClip deathSound;
    public AudioClip shieldSound;
    public AudioClip WalkSound;


    public bool IsPlayerAlive { get; private set; } = true;

    // Invulnerability properties
    public float invulnerabilityDuration = 0.5f;
    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;

    private SpriteRenderer playerSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        playerSprite = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        inputDir = new Vector2(moveX, moveY);

        if (inputDir.sqrMagnitude > 0.01f)
            lastMoveDir = inputDir.normalized;

        if (inputDir.sqrMagnitude > 1f)
            inputDir.Normalize();

        HandleInput();
        UpdateTimers();
    }

    void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            velocity = Vector2.Lerp(velocity, inputDir * moveSpeed, acceleration * Time.fixedDeltaTime);
            velocity = Vector2.Lerp(velocity, Vector2.zero, friction * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (attackInput && !isKnockedBack)
        {
            //PlayerAttack();
            attackInput = false;
        }

        if (magicProjectileInput && !isKnockedBack && nbrMagic>0)
        {
            audioSource.PlayOneShot(magicSound);
            ShootMagicProjectile();
            
            magicProjectileInput = false;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking && nbrSword > 0)
        {
            StartCoroutine(AttackCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            magicProjectileInput = true;
        }
    }

    void UpdateTimers()
    {
        UpdateMagicCooldownTimer();
        UpdateKnockbackTimer();
        UpdateInvulnerabilityTimer();
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        GameObject sword = Instantiate(
            swordPrefab,
            (Vector2)transform.position + lastMoveDir * 0.8f,
            Quaternion.identity
        );

        float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
        sword.transform.rotation = Quaternion.Euler(0, 0, angle);

        SwordSlash slash = sword.GetComponent<SwordSlash>();
        if (slash != null)
            slash.direction = lastMoveDir;

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
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
                rb.linearVelocity = Vector2.zero;
                Debug.Log("Push phase ended, transitioning to stun phase");
            }

            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
                isInPushPhase = false;
                rb.linearVelocity = Vector2.zero;
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

    public void HandleDamage(Vector2 hitDirection)
    {
        if (isInvulnerable)
        {
            Debug.Log("Player is invulnerable, ignoring damage");
            return;
        }

        if (nbrShield > 0)
        {
            // Flash blanc pour montrer que le joueur est protégé
            Debug.Log("Player defended against damage with shield");
            StartCoroutine(FlashEffect());
            
            audioSource.PlayOneShot(shieldSound);
            DavidUIManager.Instance.UpdateUI();
            nbrShield--;
        }
        
        if (CanDefend())
        {
            PlayerDefend(hitDirection);
        }
        else
        {
            DavidUIManager.Instance.PlayerDied();
        }

        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
    }

    public bool CanDefend()
    {
        return nbrShield > 0 && !isKnockedBack;
    }

    public void PlayerDefend(Vector2 hitDirection)
    {
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

    void ShootMagicProjectile()
    {
        if (MagicProjectile != null && nbrMagic > 0 && !IsMagicOnCooldown)
        {
            Vector3 spawnPosition = transform.position + (Vector3)lastMoveDir * projectileOffset;
            GameObject projectile = Instantiate(MagicProjectile, spawnPosition, transform.rotation);
            projectile.transform.up = lastMoveDir;

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

    public void RegisterSwordHit()
    {
        currentAttackHasHit = true;
        Debug.Log("attack 1" + nbrSword);
        nbrSword--;
        Debug.Log("attack 2" + nbrSword);
        Debug.Log("Enemy hit! Sword consumed.");

        DavidUIManager.Instance.UpdateUI();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);


        if (other.CompareTag("Pit"))
        {
            Debug.Log("Player fell into a pit!");
            // Call game manager to handle pit fall
            if (DavidUIManager.Instance != null)
            {
                DavidUIManager.Instance.PlayerDied();
            }
        }
        if (other.CompareTag("SpikedTrap") && !isKnockedBack)
        {
            if (nbrMagic > 0)
            {
                nbrMagic--;
                HandleDamage(Vector2.zero);
            }


            if (other.CompareTag("Enemy") &&  !isKnockedBack)
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
                projectile.transform.up = lastMoveDir;
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
        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);

            if (collision.gameObject.CompareTag("Enemy") &&  !isKnockedBack)
            {
                Vector2 hitDirection = collision.contacts[0].point - (Vector2)transform.position;
                nbrShield--;
                HandleDamage(hitDirection);
            }
        }
    }

    // Coroutine pour l'effet de flash
    private IEnumerator FlashEffect()
    {
        Color originalColor = playerSprite.color;
        
        // Répéter le flash 3 fois
        for (int i = 0; i < 3; i++)
        {
            // Changer en blanc
            playerSprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            
            // Revenir à la couleur d'origine
            playerSprite.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
