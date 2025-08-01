using UnityEngine;
using System.Collections.Generic;

public class ChoiceTrigger : MonoBehaviour
{
    public GameObject choicePanel; // R�f�rence au panneau de choix
    
    private void Start()
    {
        // V�rifier que le panneau est correctement assign�
        if (choicePanel == null)
        {
            Debug.LogError("Le panneau de choix n'est pas assign� sur " + gameObject.name);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // V�rifie si l'objet qui entre en collision est le joueur
        if (other.CompareTag("Player"))
        {
            Debug.Log("Joueur d�tect� par le trigger");
            
            // V�rifier que le panneau est valide
            if (choicePanel == null)
            {
                Debug.LogError("choicePanel n'est pas assign�!");
                return;
            }
            
            int nbrChoice = Random.Range(1, 4); // G�n�re un nombre al�atoire entre 1 et 3
            Debug.Log("Nombre de choix � afficher: " + nbrChoice);
            
            // Active le panneau de choix
            choicePanel.SetActive(true);
            
            // V�rifie si le panneau a des enfants
            if (choicePanel.transform.childCount == 0)
            {
                Debug.LogError("Le panneau de choix n'a pas d'enfants (boutons)!");
                return;
            }
            
            // Cr�e une liste des boutons disponibles
            List<GameObject> allButtons = new List<GameObject>();
            for (int i = 0; i < choicePanel.transform.childCount; i++)
            {
                GameObject button = choicePanel.transform.GetChild(i).gameObject;
                button.SetActive(false); // D�sactive d'abord tous les boutons
                allButtons.Add(button);
                Debug.Log("Bouton trouv�: " + button.name);
            }
            
            // M�lange la liste pour une s�lection al�atoire
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
