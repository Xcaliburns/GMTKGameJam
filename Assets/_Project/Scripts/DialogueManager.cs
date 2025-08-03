using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("Références UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    [Header("Paramètres dialogue")]
    [TextArea(3, 5)]
    public string[] dialogueLines; // Les phrases du dialogue
    public float typingSpeed = 0.02f; // Vitesse d'affichage des lettres

    public bool IsDialogueActive => dialoguePanel != null && dialoguePanel.activeSelf;
    private int currentLineIndex;
    private bool isTyping;      // On est en train d'afficher lettre par lettre
    private bool skipTyping;    // Le joueur veut passer directement

    private const string firstTimeKey = "FirstTimePlayed";

    void Start()
    {
        // Vérifie si c'est la première fois que le joueur joue
        if (PlayerPrefs.GetInt(firstTimeKey, 1) == 1)
        {
            // On sauvegarde que le joueur a vu l'intro
            PlayerPrefs.SetInt(firstTimeKey, 0);
            PlayerPrefs.Save();

            StartDialogue();
        }
        else
        {
            // Si ce n'est pas la première fois → ne rien faire
            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);
        }
    }

    void Update()
    {
        // On écoute la touche espace
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Si on tape on skip l'effet et on affiche directement
                skipTyping = true;
            }
            else
            {
                // Sinon ligne suivante
                ShowNextLine();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteKey(firstTimeKey);
            PlayerPrefs.Save();
            Debug.Log("🔄 Dialogue d'intro réinitialisé !");
        }
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        currentLineIndex = 0;
        ShowLine(dialogueLines[currentLineIndex]);
    }

    void ShowNextLine()
    {
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            ShowLine(dialogueLines[currentLineIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowLine(string line)
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        isTyping = true;
        skipTyping = false;

        foreach (char c in line)
        {
            if (skipTyping)
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
