using UnityEngine;
using UnityEngine.UI; // For Button elements
using TMPro; // For TextMeshProUGUI

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;


public class ShiFuMi : MonoBehaviour
{
    public enum Choice
    {
        Sword,
        Shield,
        Magic
    }

    [Header("UI References")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI playerChoiceText;
    public TextMeshProUGUI bossChoiceText;
    public TextMeshProUGUI playerStatsText;
    public TextMeshProUGUI bossStatsText;
    public Button swordButton;
    public Button shieldButton;
    public Button magicButton;

    [Header("Game Settings")]
    public float responseTime = 3f;
    public Color winColor = Color.green;
    public Color loseColor = Color.red;
    public Color drawColor = Color.yellow;

    [Header("Boss Stats")]
    public int bossSwordStats = 3;
    public int bossShieldStats = 3;
    public int bossMagicStats = 3;

    [Header("Player Reference")]
    public PlayerController playerController;

    [Header("Auto Start Game")]
    public bool autoStartGame = false;

    private Choice playerChoice;
    private Choice bossChoice;
    private Color defaultTextColor;
    private bool canPlayerRespond = false;
    private int dodgePhaseCount = 0;
    private bool gameOver = false;
    private Coroutine timerCoroutine;
    
    // Event that gets fired when the game ends
    public System.Action<bool> onGameComplete;
    public GameObject panelwin, panelloose;
    void Start()
    {
        // Find player controller if not assigned
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
            Debug.LogError("No PlayerController found! ShiFuMi game will use default values.");

        // Set up button listeners if UI elements are assigned
        if (swordButton != null)
            swordButton.onClick.AddListener(() => MakeChoice(Choice.Sword));

        if (shieldButton != null)
            shieldButton.onClick.AddListener(() => MakeChoice(Choice.Shield));

        if (magicButton != null)
            magicButton.onClick.AddListener(() => MakeChoice(Choice.Magic));

        if (resultText != null)
            defaultTextColor = resultText.color;

        // Only auto-start if configured to do so
        if (autoStartGame)
        {
            StartGame();
        }
        else
        {
            // Initialize UI without starting the game
            if (timerText != null)
                timerText.text = "";
            if (resultText != null)
                resultText.text = "Waiting for battle...";
            if (playerChoiceText != null)
                playerChoiceText.text = "";
            if (bossChoiceText != null)
                bossChoiceText.text = "";
                
            UpdateStatsDisplay();
            UpdateButtonInteractability();
        }
    }

    // New method to start the game when triggered
    public void StartGame()
    {
        // Reset game state
        gameOver = false;
        canPlayerRespond = false;
        dodgePhaseCount = 0;
        
        // Start the first boss turn
        UpdateStatsDisplay();
        UpdateButtonInteractability();
        StartNextBossTurn();
    }

    void Update()
    {
        // Handle keyboard input
        if (canPlayerRespond)
        {
            if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && GetPlayerSwordStat() > 0)
                MakeChoice(Choice.Sword);
            else if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && GetPlayerShieldStat() > 0)
                MakeChoice(Choice.Shield);
            else if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) && GetPlayerMagicStat() > 0)
                MakeChoice(Choice.Magic);
        }
    }

    // Methods to access player stats
    private int GetPlayerSwordStat()
    {
        return playerController != null ? playerController.nbrSword : 0;
    }

    private int GetPlayerShieldStat()
    {
        return playerController != null ? playerController.nbrShield : 0;
    }

    private int GetPlayerMagicStat()
    {
        return playerController != null ? playerController.nbrMagic : 0;
    }

    private void UpdateButtonInteractability()
    {
        // Enable/disable buttons based on available stats
        if (swordButton != null)
            swordButton.interactable = GetPlayerSwordStat() > 0;

        if (shieldButton != null)
            shieldButton.interactable = GetPlayerShieldStat() > 0;

        if (magicButton != null)
            magicButton.interactable = GetPlayerMagicStat() > 0;
    }

    private void StartNextBossTurn()
    {
        if (gameOver) return;

        // Boss selects its choice
        bossChoice = GetBossChoice();

        // Display boss choice
        if (bossChoiceText != null)
            bossChoiceText.text = $"Boss choice: {bossChoice}";

        // Allow player to respond
        canPlayerRespond = true;
        UpdateButtonInteractability();

        // Start timer
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(ResponseTimer());
    }

    private Choice GetBossChoice()
    {
        // Create a list of available choices based on boss stats
        System.Collections.Generic.List<Choice> availableChoices = new System.Collections.Generic.List<Choice>();

        if (bossSwordStats > 0)
            availableChoices.Add(Choice.Sword);
        if (bossShieldStats > 0)
            availableChoices.Add(Choice.Shield);
        if (bossMagicStats > 0)
            availableChoices.Add(Choice.Magic);

        // If no choices available, game should be over
        if (availableChoices.Count == 0)
        {
            EndGame(true);
            return Choice.Sword; // Default, won't be used
        }

        // Randomly select from available choices
        int randomIndex = UnityEngine.Random.Range(0, availableChoices.Count);
        return availableChoices[randomIndex];
    }

    private IEnumerator ResponseTimer()
    {
        float timeLeft = responseTime;

        while (timeLeft > 0 && canPlayerRespond)
        {
            timeLeft -= Time.deltaTime;

            if (timerText != null)
                timerText.text = $"Time: {timeLeft:F1}s";

            yield return null;
        }

        // If player didn't respond in time
        if (canPlayerRespond)
        {
            canPlayerRespond = false;
            HandleTimeOut();
        }
    }

    private void HandleTimeOut()
    {
        if (resultText != null)
        {
            resultText.text = "TIME OUT!\nYou lose a point!";
            resultText.color = loseColor;
        }

        // Player loses a point based on boss choice
        ReducePlayerStat(bossChoice);
        UpdateStatsDisplay();
        UpdateButtonInteractability();

        // Check if game should continue
        if (!gameOver)
            StartCoroutine(DelayNextTurn(2f));
    }

    public void MakeChoice(Choice choice)
    {
        if (!canPlayerRespond) return;

        // Check if player has stats for this choice
        if ((choice == Choice.Sword && GetPlayerSwordStat() <= 0) ||
            (choice == Choice.Shield && GetPlayerShieldStat() <= 0) ||
            (choice == Choice.Magic && GetPlayerMagicStat() <= 0))
        {
            return; // Cannot make this choice
        }

        playerChoice = choice;
        canPlayerRespond = false;

        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        if (playerChoiceText != null)
            playerChoiceText.text = $"You choose: {playerChoice}";

        DetermineWinner();
    }

    private void DetermineWinner()
    {
        if (resultText == null)
            return;

        string result;
        Color resultColor;

        if (IsCounterTo(playerChoice, bossChoice))
        {
            result = "YOU WIN!\nBoss loses a point!";
            resultColor = winColor;
            ReduceBossStat(bossChoice);
        }
        else if (playerChoice == bossChoice)
        {
            // In case of a draw, player wins and boss loses a point
            result = "DRAW - YOU LOSE!\nYou lose a point!";
            resultColor = loseColor;
            ReducePlayerStat(playerChoice);
        }
        else
        {
            result = "YOU LOSE!\nYou lose a point!";
            resultColor = loseColor;
            ReducePlayerStat(playerChoice);
        }

        // Display the result with appropriate color
        resultText.text = result;
        resultText.color = resultColor;

        // Add explanation
        string explanation = GetMatchupExplanation(playerChoice, bossChoice);
        if (!string.IsNullOrEmpty(explanation))
            resultText.text += "\n" + explanation;

        UpdateStatsDisplay();
        UpdateButtonInteractability();

        // Check if we should continue
        if (!gameOver)
            StartCoroutine(DelayNextTurn(2f));
    }

    private bool IsCounterTo(Choice attacker, Choice defender)
    {
        return (attacker == Choice.Sword && defender == Choice.Magic) ||
               (attacker == Choice.Magic && defender == Choice.Shield) ||
               (attacker == Choice.Shield && defender == Choice.Sword);
    }

    private void ReduceBossStat(Choice choice)
    {
        switch (choice)
        {
            case Choice.Sword:
                bossSwordStats = Mathf.Max(0, bossSwordStats - 1);
                CheckBossPhaseTransition();
                break;
            case Choice.Shield:
                bossShieldStats = Mathf.Max(0, bossShieldStats - 1);
                CheckBossPhaseTransition();
                break;
            case Choice.Magic:
                bossMagicStats = Mathf.Max(0, bossMagicStats - 1);
                CheckBossPhaseTransition();
                break;
        }

        // Check if boss has been defeated (all stats at zero)
        if (bossSwordStats <= 0 && bossShieldStats <= 0 && bossMagicStats <= 0)
        {
            EndGame(true);
        }
    }

    private void ReducePlayerStat(Choice choice)
    {
        if (playerController == null)
            return;

        switch (choice)
        {
            case Choice.Sword:
                playerController.nbrSword = Mathf.Max(0, playerController.nbrSword - 1);
                break;
            case Choice.Shield:
                playerController.nbrShield = Mathf.Max(0, playerController.nbrShield - 1);
                break;
            case Choice.Magic:
                playerController.nbrMagic = Mathf.Max(0, playerController.nbrMagic - 1);
                break;
        }

        // Check if player has lost (all stats at zero)
        if (GetPlayerSwordStat() <= 0 && GetPlayerShieldStat() <= 0 && GetPlayerMagicStat() <= 0)
        {
            EndGame(false);
        }

        // Update UI if available
        if (DavidUIManager.Instance != null)
            DavidUIManager.Instance.UpdateUI();
    }

    private void CheckBossPhaseTransition()
    {
        // Check if any boss stat has reached zero, but not all
        if ((bossSwordStats <= 0 || bossShieldStats <= 0 || bossMagicStats <= 0) &&
            !(bossSwordStats <= 0 && bossShieldStats <= 0 && bossMagicStats <= 0))
        {
            // Start dodge phase
            StartDodgePhase();
        }
    }

    private void StartDodgePhase()
    {
        dodgePhaseCount++;

        if (resultText != null)
        {
            resultText.text = $"Dodge Phase {dodgePhaseCount} Starting!\nDifficulty: {dodgePhaseCount}/4";
            resultText.color = defaultTextColor;
        }

        // TODO: Implement the actual dodge phase mechanic
        // For now, we'll just simulate it with a delay
        StartCoroutine(SimulateDodgePhase(3f));
    }

    private IEnumerator SimulateDodgePhase(float duration)
    {
        // Disable ShiFuMi controls during dodge phase
        canPlayerRespond = false;

        yield return new WaitForSeconds(duration);

        if (resultText != null)
        {
            resultText.text = "Dodge Phase Complete!";
            resultText.color = defaultTextColor;
        }

        yield return new WaitForSeconds(1f);

        // Continue with next boss turn
        StartNextBossTurn();
    }

    private void EndGame(bool playerWon)
    {
        gameOver = true;

        if (resultText != null)
        {
            if (playerWon)
            {
                resultText.text = "VICTORY!\nThe boss has lost all stats!";
                resultText.color = winColor;
                var dddd = Object.FindFirstObjectByType<DontDestroyCanva>();
                Instantiate(panelwin,  dddd.transform);
                // Add final message about replacing the boss
                StartCoroutine(ShowFinalMessage("You have replaced the boss!\nEnd of run.", 3f));
            }
            else
            {
                var dddd = Object.FindFirstObjectByType<DontDestroyCanva>();
                Instantiate(panelloose, dddd.transform);
                resultText.text = "DEFEAT!\nYou have lost all your stats!";
                resultText.color = loseColor;
            }
        }
        
        // Call the onGameComplete event
        onGameComplete?.Invoke(playerWon);
    }

    private IEnumerator ShowFinalMessage(string message, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (resultText != null)
        {
            resultText.text = message;
        }
    }

    private void UpdateStatsDisplay()
    {
        if (playerStatsText != null)
        {
            playerStatsText.text = $"Player Stats:\nSword: {GetPlayerSwordStat()}\nShield: {GetPlayerShieldStat()}\nMagic: {GetPlayerMagicStat()}";
        }

        if (bossStatsText != null)
        {
            bossStatsText.text = $"Boss Stats:\nSword: {bossSwordStats}\nShield: {bossShieldStats}\nMagic: {bossMagicStats}";
        }
    }

    private string GetMatchupExplanation(Choice player, Choice boss)
    {
        if (player == boss)
            return "Both chose the same option";

        switch (player)
        {
            case Choice.Sword:
                return boss == Choice.Magic ? "Sword beats Magic!" : "Shield blocks Sword!";
            case Choice.Shield:
                return boss == Choice.Sword ? "Shield blocks Sword!" : "Magic penetrates Shield!";
            case Choice.Magic:
                return boss == Choice.Shield ? "Magic penetrates Shield!" : "Sword cuts through Magic!";
            default:
                return "";
        }
    }

    private IEnumerator DelayNextTurn(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (resultText != null)
            resultText.color = defaultTextColor;

        StartNextBossTurn();
    }

    private IEnumerator ResetTextColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (resultText != null)
            resultText.color = defaultTextColor;
    }
}
