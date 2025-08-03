using UnityEngine;

public class ShiFuMiTrigger : MonoBehaviour
{
    [Header("Game References")]
    [SerializeField] private ShiFuMi shiFuMiGame;
    [SerializeField] private GameObject shiFuMiPanel; // Renomm� pour correspondre au ShiFuMiPanel
    
    public GameObject PanelToMove;

    [Header("Settings")]
    [SerializeField] private bool activateOnce = true;
    
    private bool hasBeenActivated = false;

    private void Awake()
    {
        // Assurez-vous que le panel est d�sactiv� au d�marrage
        if (shiFuMiPanel != null)
        {
            shiFuMiPanel.SetActive(false);
        }
        else
        {
            // Si aucun panel n'est assign�, essayez de le trouver par son nom
            GameObject panel = GameObject.Find("ShiFuMiPanel");
            if (panel != null)
            {
                shiFuMiPanel = panel;
                shiFuMiPanel.SetActive(false);
            }
            else
            {
                Debug.LogError("ShiFuMiPanel introuvable! Assurez-vous que le panel est assign� ou nomm� 'ShiFuMiPanel'");
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
        // V�rifiez si l'objet entrant a le tag "Player"
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player a d�clench� le ShiFuMi trigger");
            
            // Si configur� pour n'activer qu'une seule fois et d�j� activ�, ne rien faire
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
        
        // Si le composant ShiFuMi a �t� fourni, d�marrer le jeu
        if (shiFuMiGame != null)
        {
            // Trouver le PlayerController si ce n'est pas d�j� fait
            if (shiFuMiGame.playerController == null)
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    shiFuMiGame.playerController = player;
                }
            }
            
            shiFuMiGame.StartGame();
            
            // S'abonner � l'�v�nement de fin de jeu
            shiFuMiGame.onGameComplete += OnShiFuMiComplete;
        }
        else
        {
            // Si aucun ShiFuMi n'est assign�, essayez de le trouver sur le panel
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

    // M�thode appel�e lorsque le jeu ShiFuMi se termine
    private void OnShiFuMiComplete(bool playerWon)
    {
        Debug.Log("ShiFuMi termin�, joueur a " + (playerWon ? "gagn�" : "perdu"));
        
        // Se d�sabonner pour �viter les fuites de m�moire
        if (shiFuMiGame != null)
            shiFuMiGame.onGameComplete -= OnShiFuMiComplete;
            
        // D�sactiver le jeu apr�s un court d�lai
        Invoke("DeactivateShiFuMi", 3f);
    }

    // M�thode pour d�sactiver ShiFuMi
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