using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DavidUIManager : MonoBehaviour
{
    public static DavidUIManager Instance { get; private set; }
    
    [Header("References")]
    public PlayerController playerController;
    
    [Header("UI Elements")]
    public TextMeshProUGUI swordsText;
    public TextMeshProUGUI shieldsText;
    public TextMeshProUGUI MagicText;

    public GameObject mainMenuPanel;
    
    // Add button references
    [Header("Buttons")]
    public Button startButton;
    public Button exitButton;

    private void Awake()
    {
        // Simple singleton pattern without DontDestroyOnLoad
        if (Instance == null)
        {
            Instance = this;
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
            playerController = FindObjectOfType<PlayerController>();
        }
        
        // Set up button listeners programmatically
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            Debug.Log("Start button listener added");
        }
        else
        {
            Debug.LogError("Start button reference is missing");
        }
        
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
            Debug.Log("Exit button listener added");
        }
        
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
                
            if (shieldsText != null)
                shieldsText.text = "Boucliers: " + playerController.nbrShield;
            if (MagicText != null)
                MagicText.text = "Magie: " + playerController.nbrMagic;
        }
    }
    
    public void StartGame()
    {
        Debug.Log("StartGame method called!");
        if (mainMenuPanel != null)
        {
            Debug.Log("Hiding main menu panel");
            mainMenuPanel.SetActive(false);
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
}
