using UnityEngine;
using System.Collections;

public class SpikedTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public float spikesInterval = 2.0f;
    public float extendedDuration = 1.0f;
   
    private bool spikesExtended = false;
    private Collider2D spikeCollider;

    [Header("Animation Settings")]
    public Sprite[] frames;
    public float frameRate = 2f;
    private SpriteRenderer sr;
    private int index;

    [Header("Sound Settings")]
    public AudioClip activateSound;
    private AudioSource audioSource;
    
    void Start()
    {
        spikeCollider = GetComponent<BoxCollider2D>();
        
        if (spikeCollider == null)
        {
            Debug.LogWarning("No collider found for spike trap. Please add a collider.");
        }
        else
        {
            spikeCollider.enabled = false;
            StartCoroutine(SpikeCycle());
        }

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = frames[2];
        
        // Setup audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f; // Son enti�rement spatial
            
            // Utiliser un mode de diminution logarithmique (plus naturel et plus rapide)
            audioSource.rolloffMode = AudioRolloffMode.Custom;
            
            // Diminution plus rapide avec des distances r�duites
            audioSource.minDistance = 1.0f;
            audioSource.maxDistance = 8.0f; // R�duit de 20 � 8 pour une diminution plus rapide
            
            // Cr�er une courbe personnalis�e pour une diminution rapide
            AnimationCurve rolloffCurve = new AnimationCurve();
            rolloffCurve.AddKey(0.0f, 1.0f);            // Distance min = volume max
            rolloffCurve.AddKey(0.25f, 0.5f);           // � 25% de la distance max, volume � 50%
            rolloffCurve.AddKey(0.5f, 0.2f);            // � 50% de la distance max, volume � 20%
            rolloffCurve.AddKey(1.0f, 0.0f);            // Distance max = volume nul
            
            // Appliquer la courbe personnalis�e
            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, rolloffCurve);
        }
    }
    
    IEnumerator SpikeCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(spikesInterval);
            ActivateSpikes();
            yield return new WaitForSeconds(extendedDuration);
            DeactivateSpikes();
        }
    }
    private void OnEnable()
    {
        StartCoroutine(SpikeCycle());
    }
    void ActivateSpikes()
    {
        spikeCollider.enabled = true;
        spikeCollider.isTrigger = true;
        spikesExtended = true;
        
        if (activateSound != null && audioSource != null)
        {
            audioSource.clip = activateSound;
            audioSource.Play();
        }
        
        sr.sprite = frames[2];
        UpdateTrapColor(Color.white);
    }
    
    void DeactivateSpikes()
    {
        spikeCollider.enabled = false;
        spikeCollider.isTrigger = false;
        spikesExtended = false;
        sr.sprite = frames[0];
        UpdateTrapColor(Color.white);
    }
    
    void UpdateTrapColor(Color color)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}
