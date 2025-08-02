using UnityEngine;
using System.Collections.Generic;

public class ChoiceTrigger : MonoBehaviour
{
    public GameObject choicePanel; // R�f�rence au panneau de choix
    private float previousTimeScale; // Pour stocker l'�chelle de temps pr�c�dente
    
    private void Start()
    {
        // V�rifier que le panneau est correctement assign�
        if (choicePanel == null)
        {
            Debug.LogError("Le panneau de choix n'est pas assign� sur " + gameObject.name);
        }
        
        // S'assurer que le panneau est d�sactiv� au d�marrage
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // V�rifie si l'objet qui entre en collision est le joueur
        if (other.CompareTag("Player"))
        {
            // V�rifier que le panneau est valide
            if (choicePanel == null)
            {
                Debug.LogError("choicePanel n'est pas assign�!");
                return;
            }
            
            // Pause le jeu
            PauseGame();
            
            int nbrChoice = Random.Range(1, 4); 
            Debug.Log("Nombre de choix � afficher: " + nbrChoice);
            
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
                button.SetActive(false); // D�sactive d'abord tous les boutons
                allButtons.Add(button);
                Debug.Log("Bouton trouv�: " + button.name);
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
        // Sauvegarder l'�chelle de temps actuelle
        previousTimeScale = Time.timeScale;
        // Mettre l'�chelle de temps � 0 pour arr�ter le jeu
        Time.timeScale = 0f;
        Debug.Log("Jeu en pause");
    }
    
    public void ResumeGame()
    {
        // Restaurer l'�chelle de temps pr�c�dente
        Time.timeScale = previousTimeScale;
        // D�sactiver le panneau de choix
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
        Debug.Log("Jeu repris");
    }
}
