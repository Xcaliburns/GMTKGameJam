using UnityEngine;

public class ShiFuMiTrigger : MonoBehaviour
{
    [Header("Game References")]
    [SerializeField] private ShiFuMi shiFuMiGame;
    [SerializeField] private GameObject shiFuMiPanel; // Renommé pour correspondre au ShiFuMiPanel
    
    public GameObject PanelToMove;

    [Header("Settings")]
    [SerializeField] private bool activateOnce = true;
    
    private bool hasBeenActivated = false;

    private void Awake()
    {
        // Assurez-vous que le panel est désactivé au démarrage
        if (shiFuMiPanel != null)
        {
            shiFuMiPanel.SetActive(false);
        }
        else
        {
            // Si aucun panel n'est assigné, essayez de le trouver par son nom
            GameObject panel = GameObject.Find("ShiFuMiPanel");
            if (panel != null)
            {
                shiFuMiPanel = panel;
                shiFuMiPanel.SetActive(false);
            }
            else
            {
                Debug.LogError("ShiFuMiPanel introuvable! Assurez-vous que le panel est assigné ou nommé 'ShiFuMiPanel'");
            }
        }
    }

    private void Start()
    {
        var CanvaTomove=   Object.FindFirstObjectByType<DontDestroyCanva>();
        PanelToMove.transform.parent = CanvaTomove.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifiez si l'objet entrant a le tag "Player"
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player a déclenché le ShiFuMi trigger");
            
            // Si configuré pour n'activer qu'une seule fois et déjà activé, ne rien faire
            if (activateOnce && hasBeenActivated)
                return;
                
            ActivateShiFuMi();
        }
    }

    private void ActivateShiFuMi()
    {
        Debug.Log("Activation du ShiFuMi");
        
        // Activer le Panel ShiFuMi
        if (shiFuMiPanel != null)
        {
            shiFuMiPanel.SetActive(true);
            shiFuMiGame .enabled=true;
        }
        
        // Si le composant ShiFuMi a été fourni, démarrer le jeu
        if (shiFuMiGame != null)
        {
            // Trouver le PlayerController si ce n'est pas déjà fait
            if (shiFuMiGame.playerController == null)
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    shiFuMiGame.playerController = player;
                }
            }
            
            shiFuMiGame.StartGame();
            
            // S'abonner à l'événement de fin de jeu
            shiFuMiGame.onGameComplete += OnShiFuMiComplete;
        }
        else
        {
            // Si aucun ShiFuMi n'est assigné, essayez de le trouver sur le panel
            if (shiFuMiPanel != null)
            {
                shiFuMiGame = shiFuMiPanel.GetComponent<ShiFuMi>();
                if (shiFuMiGame != null)
                {
                    PlayerController player = FindObjectOfType<PlayerController>();
                    if (player != null)
                    {
                        shiFuMiGame.playerController = player;
                    }
                    
                    shiFuMiGame.StartGame();
                    shiFuMiGame.onGameComplete += OnShiFuMiComplete;
                }
            }
        }
        
        hasBeenActivated = true;
        
        //// Mettre en pause le jeu
        //Time.timeScale = 0f;
        shiFuMiGame.playerController.blockinputs = true;
    }

    // Méthode appelée lorsque le jeu ShiFuMi se termine
    private void OnShiFuMiComplete(bool playerWon)
    {
        Debug.Log("ShiFuMi terminé, joueur a " + (playerWon ? "gagné" : "perdu"));
        
        // Se désabonner pour éviter les fuites de mémoire
        if (shiFuMiGame != null)
            shiFuMiGame.onGameComplete -= OnShiFuMiComplete;
            
        // Désactiver le jeu après un court délai
        Invoke("DeactivateShiFuMi", 3f);
    }

    // Méthode pour désactiver ShiFuMi
    public void DeactivateShiFuMi()
    {
        if (shiFuMiPanel != null)
        {
            shiFuMiPanel.SetActive(false);
        }
        
        // Reprendre le temps de jeu normal
        Time.timeScale = 1f;
    }
}