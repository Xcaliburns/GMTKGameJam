using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    float lifetime;
    public float lifetimeMax = 5f;
    private PlayerController playerController;
    private Rigidbody2D rb;


    void Start()
    {
        Destroy(gameObject, lifetimeMax);
        playerController = FindFirstObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        // Check collider setup
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogWarning("No collider found on projectile - adding one");
            CircleCollider2D newCollider = gameObject.AddComponent<CircleCollider2D>();
            newCollider.isTrigger = true;
            newCollider.radius = 0.25f;
        }

        Debug.Log($"Enemy projectile spawned at {transform.position} with velocity {rb.linearVelocity}");
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

    }





    private void OnTriggerEnter2D(Collider2D collision)
    {


        // Don't destroy when hitting other enemies
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy projectile passing through: " + collision.gameObject.name);
            return;
        }

        // Special handling for player hits
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy projectile hit player");
            if (playerController != null && !playerController.isKnockedBack)
            {
                Vector2 hitDirection = transform.position - collision.transform.position;

                if (playerController.nbrShield > 0)
                {
                    playerController.nbrShield--;
                    DavidUIManager.Instance.UpdateUI();

                }

                playerController.HandleDamage(hitDirection);
                Destroy(gameObject);
                Debug.Log(collision.name);
            }

            //  Destroy(gameObject);
            return;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Destroy the projectile when it exits any collider
        if (collision.gameObject.CompareTag("Limit"))
        {

            Destroy(gameObject);
        }
    }
}
