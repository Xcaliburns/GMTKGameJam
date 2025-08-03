using UnityEngine;
using UnityEngine.UI;

public class BonusPoints : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite swordSprite;
    public Sprite shieldSprite;
    public Sprite magicSprite;

    [Header("Values")]
    public int minSwordPoints = 0;
    public int maxSwordPoints = 3;
    public int minShieldPoints = 0;
    public int maxShieldPoints = 3;
    public int minMagicPoints = 0;
    public int maxMagicPoints = 3;

    [Header("UI References")]
    public Transform container;
    public RectTransform background;
    public Color desactivationColor = Color.black;
    public Canvas canvas;

    [Header("Settings")]
    public float scale = 38f;
    public float iconSpacing = 5f;
    public float lineSpacing = 8f;
    public float backgroundPaddingX = 20f;
    public float backgroundPaddingY = 20f;
    public float offsetX = 0.15f;

    private int swordPoints;
    private int shieldPoints;
    private int magicPoints;
    private float currentY = 0.3f;

    PlayerController playerController;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        swordPoints = Random.Range(minSwordPoints, maxSwordPoints + 1);
        shieldPoints = Random.Range(minShieldPoints, maxShieldPoints + 1);
        magicPoints = Random.Range(minMagicPoints, maxMagicPoints + 1);

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerController = FindFirstObjectByType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateStatsUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatsUI();
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
            canvas.enabled = false;
        }
    }

    public void UpdateStatsUI()
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);

        float maxLineWidth = 0f;
        int linesDisplayed = 0;

        void AddLine(Sprite sprite, int count, float yPos)
        {
            if (sprite == null || count <= 0) return;

            Vector2 spriteSize = sprite.rect.size;
            float ppu = sprite.pixelsPerUnit;
            Vector2 sizeInUnits = spriteSize / ppu;
            float widthPerIcon = sizeInUnits.x * scale;

            float totalLineWidth = count * widthPerIcon + (count - 1) * iconSpacing;
            if (totalLineWidth > maxLineWidth)
                maxLineWidth = totalLineWidth;

            float startX = -totalLineWidth / 2f;

            for (int i = 0; i < count; i++)
            {
                GameObject icon = new GameObject(sprite.name);
                icon.transform.SetParent(container);
                icon.transform.localScale = Vector3.one;

                Image image = icon.AddComponent<Image>();
                image.sprite = sprite;

                RectTransform rt = icon.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(startX + i * (widthPerIcon + iconSpacing) + offsetX, yPos);
                rt.sizeDelta = sizeInUnits * scale;
            }
        }

        currentY = 0.3f;

        if (swordPoints > 0)
        {
            AddLine(swordSprite, swordPoints, currentY);
            currentY -= (scale + lineSpacing);
            linesDisplayed++;
        }
        if (shieldPoints > 0)
        {
            AddLine(shieldSprite, shieldPoints, currentY);
            currentY -= (scale + lineSpacing);
            linesDisplayed++;
        }
        if (magicPoints > 0)
        {
            AddLine(magicSprite, magicPoints, currentY);
            currentY -= (scale + lineSpacing);
            linesDisplayed++;
        }

        if (background != null)
        {
            float totalHeight = (linesDisplayed * scale) + ((linesDisplayed - 1) * lineSpacing);
            background.sizeDelta = new Vector2(
                maxLineWidth + backgroundPaddingX,
                totalHeight + backgroundPaddingY
            );
        }
    }
}
