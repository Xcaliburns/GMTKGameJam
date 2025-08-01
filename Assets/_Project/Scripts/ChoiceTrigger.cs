using UnityEngine;
using System.Collections.Generic;

public class ChoiceTrigger : MonoBehaviour
{
    public GameObject choicePanel; // Référence au panneau de choix
    
    private void Start()
    {
        // Vérifier que le panneau est correctement assigné
        if (choicePanel == null)
        {
            Debug.LogError("Le panneau de choix n'est pas assigné sur " + gameObject.name);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si l'objet qui entre en collision est le joueur
        if (other.CompareTag("Player"))
        {
            Debug.Log("Joueur détecté par le trigger");
            
            // Vérifier que le panneau est valide
            if (choicePanel == null)
            {
                Debug.LogError("choicePanel n'est pas assigné!");
                return;
            }
            
            int nbrChoice = Random.Range(1, 4); // Génère un nombre aléatoire entre 1 et 3
            Debug.Log("Nombre de choix à afficher: " + nbrChoice);
            
            // Active le panneau de choix
            choicePanel.SetActive(true);
            
            // Vérifie si le panneau a des enfants
            if (choicePanel.transform.childCount == 0)
            {
                Debug.LogError("Le panneau de choix n'a pas d'enfants (boutons)!");
                return;
            }
            
            // Crée une liste des boutons disponibles
            List<GameObject> allButtons = new List<GameObject>();
            for (int i = 0; i < choicePanel.transform.childCount; i++)
            {
                GameObject button = choicePanel.transform.GetChild(i).gameObject;
                button.SetActive(false); // Désactive d'abord tous les boutons
                allButtons.Add(button);
                Debug.Log("Bouton trouvé: " + button.name);
            }
            
            // Mélange la liste pour une sélection aléatoire
            ShuffleList(allButtons);
            
            // Active seulement le nombre de boutons requis
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
}
