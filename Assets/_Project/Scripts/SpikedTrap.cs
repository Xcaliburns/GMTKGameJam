using UnityEngine;
using System.Collections;

public class SpikedTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public float spikesInterval = 2.0f;
    public float extendedDuration = 1.0f;
   
    private bool spikesExtended = false;
    private Collider2D spikeCollider;

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
    
    void ActivateSpikes()
    {
        spikeCollider.enabled = true;
        spikeCollider.isTrigger = true;
        spikesExtended = true;
        
        UpdateTrapColor(Color.red);
    }
    
    void DeactivateSpikes()
    {
        spikeCollider.enabled = false;
        spikeCollider.isTrigger = false;
        spikesExtended = false;
        
        UpdateTrapColor(Color.green);
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
