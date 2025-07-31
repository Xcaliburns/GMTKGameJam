using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction = Vector3.right;

    [SerializeField] float speed = 10f;
    [SerializeField] float lifetimeMax = 5f;

    float lifeTime;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        lifeTime += Time.deltaTime;
        if (lifeTime >= lifetimeMax)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            if (!player.isKnockedBack)
            {
                // Calculer la direction du projectile vers le joueur pour le knockback
                Vector2 hitDirection = transform.position - collision.transform.position;

                // Utiliser la méthode de défense existante
                player.SendMessage("PlayerDefend", hitDirection, SendMessageOptions.DontRequireReceiver);
            }
            Destroy(gameObject); // Destroy the projectile on collision with anything else
        }
    }
}
