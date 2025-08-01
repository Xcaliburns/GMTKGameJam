using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float lifetimeMax = 5f;

    float lifeTime;

    private void OnDisable()
    {
        lifeTime = 0;
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        lifeTime += Time.deltaTime;
        if (lifeTime >= lifetimeMax)
        {
            BulletPool.Instance.Pool.ReturnToPool(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerController player))
            {
                if (!player.isKnockedBack)
                {
                    // Calculer la direction du projectile vers le joueur pour le knockback
                    Vector2 hitDirection = transform.position - collision.transform.position;

                    player.nbrShield--;

                    // Appeler HandleDamage pour gérer les dégâts et l'invulnérabilité
                    player.HandleDamage(hitDirection);
                }

                BulletPool.Instance.Pool.ReturnToPool(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Destroy the projectile when it exits any collider
        if (collision.gameObject.CompareTag("Limit"))
        {
            BulletPool.Instance.Pool.ReturnToPool(this);
        }
    }
}
