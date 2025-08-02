using UnityEngine;

public class BonusPoints : MonoBehaviour
{
    public int swordPoints = 1; // The number of bonus points to award
    public int shieldPoints = 1; // The number of bonus points to award
    public int magicPoints = 1; // The number of bonus points to award
    public Color desactivationColor = Color.yellow; // The color of the bonus point object
    

    PlayerController playerController; // Reference to the PlayerController script
    Rigidbody2D rb; // Reference to the Rigidbody2D component
    BoxCollider2D boxCollider; // Reference to the BoxCollider2D component
    SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    //changer couleur

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject    
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
