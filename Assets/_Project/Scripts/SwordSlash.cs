using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public Vector2 direction;
    public float moveDistance = 0.3f;
    public float lifetime = 0.15f;
    public PlayerController playerController;

    private Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
        Destroy(gameObject, lifetime);

        if (playerController == null)
        {
            Debug.LogError("SwordSlash could not find PlayerController in parents!");
        }
    }

    void Update()
    {
        // L'épée avance légèrement vers l'avant
        float t = (Time.time - (startPos.magnitude)) / lifetime;
        Vector2 offset = direction.normalized * moveDistance * Time.deltaTime;
        transform.position += (Vector3)offset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerController == null) return;

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Sword hit enemy: " + other.gameObject.name);
            playerController.RegisterSwordHit(); // Register the hit with the player controller
            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Boss"))
        {
            if (other.TryGetComponent(out Boss boss))
            {
                boss.SwitchPhase();
                playerController.RegisterSwordHit(); // Register the hit with the player controller
            }
        }
        Debug.Log("Sword hit: " + other.gameObject.name);
    }
}
