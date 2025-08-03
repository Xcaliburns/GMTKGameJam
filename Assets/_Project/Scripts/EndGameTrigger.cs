using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameTrigger : MonoBehaviour
{
    public GameObject ghostPrefab;       // Prefab du fantôme
    public Transform ghostSpawnPoint;    // Où spawn le fantôme
    public GameObject thanksPanel;       // Panel de remerciement
    public float fadeDuration = 2f;      // Temps d'apparition du fantôme
    
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            triggered = true;
            player.blockinputs = true; // Bloque le joueur
            StartCoroutine(PlayEndSequence());
        }
    }

    private IEnumerator PlayEndSequence()
    {
        // 1. Spawn du fantôme
        GameObject ghost = Instantiate(ghostPrefab, ghostSpawnPoint.position, Quaternion.identity);

        // On suppose que le fantôme a un SpriteRenderer
        SpriteRenderer sr = ghost.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;

            // Fade in
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                c.a = Mathf.Clamp01(timer / fadeDuration);
                sr.color = c;
                yield return null;
            }
        }

        // 2. Affiche le panneau de remerciement
        if (thanksPanel != null)
        {
            var canva = Object.FindAnyObjectByType<DontDestroyCanva>();
            //thanksPanel = canva.gameObject.GetComponentInChildren<>;
            thanksPanel.SetActive(true);
        }
    }
}
