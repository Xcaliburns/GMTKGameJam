using UnityEngine;
using System.Collections.Generic;

public class ChoiceTrigger : MonoBehaviour
{
    public GameObject choicePanel; // Référence au panneau de choix
    private float previousTimeScale; // Pour stocker l'échelle de temps précédente
    
    private void Start()
    {
        // Vérifier que le panneau est correctement assigné
        if (choicePanel == null)
        {
            Debug.LogError("Le panneau de choix n'est pas assigné sur " + gameObject.name);
        }
        
        // S'assurer que le panneau est désactivé au démarrage
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si l'objet qui entre en collision est le joueur
        if (other.CompareTag("Player"))
        {
            // Vérifier que le panneau est valide
            if (choicePanel == null)
            {
                Debug.LogError("choicePanel n'est pas assigné!");
                return;
            }
            
            // Pause le jeu
            PauseGame();
            
            int nbrChoice = Random.Range(1, 4); 
            Debug.Log("Nombre de choix à afficher: " + nbrChoice);
            
            // Active le panneau de choix
            choicePanel.SetActive(true);
            
            if (choicePanel.transform.childCount == 0)
            {
                Debug.LogError("Le panneau de choix n'a pas d'enfants (boutons)!");
                ResumeGame();
                return;
            }
            
            List<GameObject> allButtons = new List<GameObject>();
            for (int i = 0; i < choicePanel.transform.childCount; i++)
            {
                GameObject button = choicePanel.transform.GetChild(i).gameObject;
                button.SetActive(false); // Désactive d'abord tous les boutons
                allButtons.Add(button);
                Debug.Log("Bouton trouvé: " + button.name);
            }
            
            ShuffleList(allButtons);
            
            for (int i = 0; i < nbrChoice && i < allButtons.Count; i++)
            {
                allButtons[i].SetActive(true);
                Debug.Log("Activation du bouton: " + allButtons[i].name);
            }
        }
    }
    
    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    
    private void PauseGame()
    {
        // Sauvegarder l'échelle de temps actuelle
        previousTimeScale = Time.timeScale;
        // Mettre l'échelle de temps à 0 pour arrêter le jeu
        Time.timeScale = 0f;
        Debug.Log("Jeu en pause");
    }
    
    public void ResumeGame()
    {
        // Restaurer l'échelle de temps précédente
        Time.timeScale = previousTimeScale;
        // Désactiver le panneau de choix
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
        Debug.Log("Jeu repris");
    }
}
