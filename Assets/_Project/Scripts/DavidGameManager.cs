using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DavidGameManager : MonoBehaviour
{
    // Singleton pattern
    public static DavidGameManager Instance { get; private set; }

    // Game state
    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public GameState currentState = GameState.MainMenu;

    // Player reference
    public PlayerController player;
    public Canvas canvas;


    // Game settings
    public float gameSpeed = 1f;
    public bool isMusicEnabled = true;
    public bool isSoundEffectsEnabled = true;
    
    // References
    public DavidUIManager uiManager;

    private void Awake()
    {
        // Singleton implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Find player if not assigned
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
        }
        
        // Find UI manager if not assigned
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<DavidUIManager>();
        }
        
        // Initialize game
        SetGameState(GameState.Playing);
    }

    void Update()
    {
        // Handle pause toggle
        if (Input.GetKeyDown(KeyCode.Escape) && (currentState == GameState.Playing || currentState == GameState.Paused))
        {
            TogglePause();
        }
    }
    
    public void SetGameState(GameState newState)
    {
        currentState = newState;
        
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                // Show main menu UI
                break;
                
            case GameState.Playing:
                Time.timeScale = gameSpeed;
                // Hide any menus
                break;
                
            case GameState.Paused:
                Time.timeScale = 0f;
                // Show pause menu
                break;
                
            case GameState.GameOver:
                Time.timeScale = 1f;
                
                // Show game over screen
                if (uiManager != null && uiManager.gameOverPanel != null)  
                {
                    uiManager.gameOverPanel.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Game Over screen reference missing!");
                }
                break;
        }
        
        // Update UI if UI manager exists
        if (uiManager != null)
        {
            uiManager.UpdateUI();
        }
    }
    
    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            SetGameState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            SetGameState(GameState.Playing);
        }
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void TestClick()
    {
        Debug.Log("Clic détecté !");
    }

    public void GameOver()
    {
        SetGameState(GameState.GameOver);
        Debug.Log("Game Over!");
    } 
    
    public void PlayerDied()
    {
        // Handle player death - could trigger game over or respawn depending on your game
        GameOver();
    }

    public void PlayerFellIntoPit()
    {
        // Handle player falling into a pit
        PlayerDied();
    }

   
}
