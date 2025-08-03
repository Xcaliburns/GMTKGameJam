using UnityEngine;

public class BonusPoints : MonoBehaviour
{
    public int swordPoints;
    public int minSwordPoints = 0;
    public int maxSwordPoints = 3;

    public int shieldPoints;
    public int minShieldPoints = 0;
    public int maxShieldPoints = 3;

    public int magicPoints;
    public int minMagicPoints = 0;
    public int maxMagicPoints = 3;
    public Color desactivationColor = Color.black;
    

    PlayerController playerController;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        swordPoints = Random.Range(minSwordPoints, maxSwordPoints + 1);
        swordPoints = Random.Range(minShieldPoints, maxShieldPoints + 1);
        swordPoints = Random.Range(minMagicPoints, maxMagicPoints + 1);

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerController = FindFirstObjectByType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player
        if (collision.CompareTag("Player"))
        {
            playerController.nbrSword += swordPoints; // Award sword points
            playerController.nbrShield += shieldPoints; // Award shield points
            playerController.nbrMagic += magicPoints; // Award magic points

            // Destroy this bonus point object after awarding points
            boxCollider.enabled = false; // Disable the collider to prevent further triggers
            spriteRenderer.color = desactivationColor; // Change color to indicate collection
        }
    }
}
