
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{

    public float speed = 10f; // Speed of the projectile
    public float lifetime = 5f; // How long the projectile lasts before being destroyed
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the projectile after its lifetime
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime); // Move the projectile forward
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destroy the enemy on collision
            Destroy(gameObject); // Destroy the projectile on collision
        }
    }
}
