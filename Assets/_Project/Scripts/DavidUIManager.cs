using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DavidUIManager : MonoBehaviour
{
    public static DavidUIManager Instance { get; private set; }

    [Header("References")]
    public PlayerController playerController;
    public GameObject player;
    public Vector3 StartPosition;

    [Header("UI Elements")]
    public Transform swordsContainer;
    public Transform shieldsContainer;
    public Transform magicContainer;
    public RectTransform IconsBackground;

    public Sprite swordSprite;
    public Sprite shieldSprite;
    public Sprite magicSprite;

    public GameObject mainMenuPanel;
    public GameObject gameOverPanel;
    public GameObject choicePanel;
    public GameObject playerHUD;//icones de vie, mana, etc.
    [Header("important de le desactiver pendant les menus")]
    public GameObject FadeManager;

    // Add button references
    [Header("Buttons")]
    public Button startButton;
    public Button exitButton;
    public Button retryButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("DavidUIManager instance initialized.");
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // If playerController isn't assigned in the inspector, try to find it
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }
        playerController.nbrSword = 3;
        playerController.nbrShield = 3;
        playerController.nbrMagic = 3;
        player.transform.position = StartPosition;
        Debug.Log("StartPosition is set to: " + StartPosition);
        // mettre le jeu en pause au démarrage
       // Time.timeScale = 0f; // Pause the game at the start


        UpdateUI();
    }

    void Update()
    {
        // Update the UI every frame
        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdateAllSprites(shieldsContainer, IconsBackground,
            swordSprite, playerController.nbrSword,
            shieldSprite, playerController.nbrShield,
            magicSprite, playerController.nbrMagic,
            38f,
            5f,
            15f
            );

        /*if (playerController != null)
        {
            UpdateSprites(swordsContainer, swordSprite, playerController.nbrSword);
            UpdateSprites(shieldsContainer, shieldSprite, playerController.nbrShield);
            UpdateSprites(magicContainer, magicSprite, playerController.nbrMagic);
        }
        else
        {
            Debug.LogError("PlayerController is not assigned in DavidUIManager!");
        }*/
    }

    private void UpdateSprites(Transform container, Sprite sprite, int count)
    {
        // Clear existing sprites
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // Create new UI Images
        for (int i = 0; i < count; i++)
        {
            GameObject newImageObject = new GameObject("Sprite");
            newImageObject.transform.SetParent(container);
            newImageObject.transform.localScale = Vector3.one;

            Image image = newImageObject.AddComponent<Image>();
            image.sprite = sprite;

            Vector2 spriteSize = sprite.rect.size;

            float ppu = sprite.pixelsPerUnit;

            Vector2 sizeInUnits = spriteSize / ppu;
            float offsetX = i * sizeInUnits.x;

            RectTransform rectTransform = newImageObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(offsetX * 38, 0);
            rectTransform.sizeDelta = sizeInUnits * 38;
        }
    }

    public void UpdateAllSprites(
        Transform container, RectTransform background,
        Sprite swordSprite, int swordCount,
        Sprite shieldSprite, int shieldCount,
        Sprite magicSprite, int magicCount,
        float scale = 38f,
        float iconSpacing = 5f,
        float groupSpacing = 15f,
        float backgroundPadding = 10f)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        float GetGroupWidth(Sprite sprite, int count)
        {
            if (sprite == null || count <= 0) return 0f;

            Vector2 spriteSize = sprite.rect.size;
            float ppu = sprite.pixelsPerUnit;
            Vector2 sizeInUnits = spriteSize / ppu;

            float widthPerIcon = sizeInUnits.x * scale;
            return (count * widthPerIcon) + ((count - 1) * iconSpacing);
        }

        float totalWidth = 0f;
        int groupsAdded = 0;

        if (swordCount > 0) { totalWidth += GetGroupWidth(swordSprite, swordCount); groupsAdded++; }
        if (shieldCount > 0) { totalWidth += GetGroupWidth(shieldSprite, shieldCount); groupsAdded++; }
        if (magicCount > 0) { totalWidth += GetGroupWidth(magicSprite, magicCount); groupsAdded++; }

        if (groupsAdded > 1) totalWidth += (groupsAdded - 1) * groupSpacing;

        if (background != null)
        {
            Vector2 bgSize = background.sizeDelta;
            bgSize.x = totalWidth + backgroundPadding;
            background.sizeDelta = bgSize;
        }

        float currentX = -totalWidth / 2f;

        void AddSprites(Sprite sprite, int count)
        {
            if (sprite == null || count <= 0) return;

            Vector2 spriteSize = sprite.rect.size;
            float ppu = sprite.pixelsPerUnit;
            Vector2 sizeInUnits = spriteSize / ppu;

            float widthPerIcon = sizeInUnits.x * scale;

            for (int i = 0; i < count; i++)
            {
                GameObject newImageObject = new GameObject(sprite.name);
                newImageObject.transform.SetParent(container);
                newImageObject.transform.localScale = Vector3.one;

                Image image = newImageObject.AddComponent<Image>();
                image.sprite = sprite;

                RectTransform rectTransform = newImageObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(currentX, 0);
                rectTransform.sizeDelta = sizeInUnits * scale;

                currentX += widthPerIcon + iconSpacing;
            }

            currentX -= iconSpacing;

            currentX += groupSpacing;
        }

        AddSprites(swordSprite, swordCount);
        AddSprites(shieldSprite, shieldCount);
        AddSprites(magicSprite, magicCount);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        if (mainMenuPanel != null)
        {
            Debug.Log("Hiding main menu panel");
            mainMenuPanel.SetActive(false);
            playerHUD.SetActive(true);

            playerController.nbrSword = 3;
            playerController.nbrShield = 3;
            playerController.nbrMagic = 3;

            if (player != null)
            {
                Debug.Log("Teleporting player to StartPosition: " + StartPosition);
                player.transform.position = StartPosition;
            }
            else
            {
                Debug.LogError("Player reference is null in DavidUIManager!");
            }
        }
        else
        {
            Debug.LogError("Main menu panel is not assigned in DavidUIManager");
        }
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame method called!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Testclick()
    {
        Debug.Log("Test click method called!");

    }
    public void PlayerDied()
    {
        playerHUD.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        playerHUD.SetActive(true);
        gameOverPanel.SetActive(false);
        StartGame();

    }

    public void GoToMainMenu()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("GameOver panel désactivé.");
        }
        else
        {
            Debug.LogError("GameOver panel n'est pas assigné.");
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
            Debug.Log("MainMenu panel activé.");
        }
        else
        {
            Debug.LogError("MainMenu panel n'est pas assigné.");
        }
    }
    public void QuitGame()
    {
        QuitGame();
    }

    public void AddSwordPoint()
    {
        playerController.nbrSword++;
        choicePanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
    public void AddShieldPoint()
    {
        playerController.nbrShield++;
        choicePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void AddMagicPoint()
    {
        playerController.nbrMagic++;
        choicePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void AddCustomPoints(int swordPoints, int shieldPoints, int magicPoints)
    {
        playerController.nbrSword += swordPoints;
        playerController.nbrShield += shieldPoints;
        playerController.nbrMagic += magicPoints;
        choicePanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
}