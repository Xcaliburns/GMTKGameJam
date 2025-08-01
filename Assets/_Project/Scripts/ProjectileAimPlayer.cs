using UnityEngine;

public class ProjectileAimPlayer : MonoBehaviour
{
    public float speed = 5f;
    public float lifetimeMax = 15f;

    private Rigidbody2D rb;
    private PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = FindFirstObjectByType<PlayerController>();

        Destroy(gameObject, lifetimeMax);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;

            rb.linearVelocity = direction * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            Debug.LogWarning("Aucun joueur trouvé dans la scène !");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerController != null && !playerController.isKnockedBack)
            {
                Vector2 hitDirection = transform.position - collision.transform.position;

                playerController.HandleDamage(hitDirection);

                Destroy(gameObject);
            }
            return;
        }

    }
}
