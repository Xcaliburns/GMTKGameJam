using UnityEngine;
using TMPro;

public class DavidUIManager : Singleton<DavidUIManager>
{
    [Header("References")]
    public PlayerController playerController;
    
    [Header("UI Elements")]
    public TextMeshProUGUI swordsText;
    public TextMeshProUGUI shieldsText;
    
    // Override the Awake method from Singleton
    protected override void Awake()
    {
        // Call the base implementation to ensure singleton functionality
        base.Awake();
    }
    
    void Start()
    {
        // If playerController isn't assigned in the inspector, try to find it
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
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
                swordsText.text = "Épées: " + playerController.nbrEpees;
                
            if (shieldsText != null)
                shieldsText.text = "Boucliers: " + playerController.nbrBouclier;
        }
    }
}
