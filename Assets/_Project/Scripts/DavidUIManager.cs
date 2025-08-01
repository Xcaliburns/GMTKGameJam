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
    public TextMeshProUGUI swordsText;
    public TextMeshProUGUI shieldsText;
    public TextMeshProUGUI MagicText;

    public GameObject mainMenuPanel;
    public GameObject gameOverPanel;
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
        playerController.nbrSword = 5;
        playerController.nbrShield = 5;
        playerController.nbrMagic = 5;
        player.transform.position = StartPosition;
        Debug.Log("StartPosition is set to: " + StartPosition);

        //// Set up button listeners programmatically
        //if (startButton != null)
        //{
        //    startButton.onClick.AddListener(StartGame);
        //    Debug.Log("Start button listener added");
        //}
        //else
        //{
        //    Debug.LogError("Start button reference is missing");
        //}

        //if (exitButton != null)
        //{
        //    exitButton.onClick.AddListener(ExitGame);
        //    Debug.Log("Exit button listener added");
        //}

        //// Set up retry button
        //if (retryButton != null)
        //{
        //    retryButton.onClick.AddListener(() => {
        //        if (DavidGameManager.Instance != null)
        //        {
        //            DavidGameManager.Instance.RestartGame();
        //        }
        //    });
        //}

        // Initial UI update
        UpdateUI();
    }

    void Update()
    {
        // Update the UI every frame
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (playerController != null)
        {
            if (swordsText != null)
                swordsText.text = "Épées: " + playerController.nbrSword;
            else
                Debug.LogError("swordsText is not assigned in DavidUIManager!");

            if (shieldsText != null)
                shieldsText.text = "Boucliers: " + playerController.nbrShield;
            else
                Debug.LogError("shieldsText is not assigned in DavidUIManager!");

            if (MagicText != null)
                MagicText.text = "Magie: " + playerController.nbrMagic;
            else
                Debug.LogError("MagicText is not assigned in DavidUIManager!");
        }
        else
        {
            Debug.LogError("PlayerController is not assigned in DavidUIManager!");
        }
    }

    public void StartGame()
    {
        if (mainMenuPanel != null)
        {
            Debug.Log("Hiding main menu panel");
            mainMenuPanel.SetActive(false);

            playerController.nbrSword = 5;
            playerController.nbrShield = 5;
            playerController.nbrMagic = 5;

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

        gameOverPanel.SetActive(true);
    }

    public void Retry()
    {
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


}
