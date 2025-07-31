using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class RoomCameraSwitcher : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;
    
    // Default priority for inactive cameras
    private const int DEFAULT_PRIORITY = 10;
    // Priority when camera is active
    private const int ACTIVE_PRIORITY = 20;
    
    // Reference to the FadeManager (must be setup in the scene)
    private FadeManager fadeManager;

    private void Start()
    {
        // Find the FadeManager in the scene
        fadeManager = FindObjectOfType<FadeManager>();
        if (fadeManager == null)
        {
            Debug.LogWarning("FadeManager not found in scene. Create one for camera transitions to work properly.");
        }
        
        if (virtualCamera == null)
        {
            Debug.LogError($"No virtual camera assigned to {gameObject.name}!");
        }
        else
        {
            // Set initial priority to default
            virtualCamera.Priority = DEFAULT_PRIORITY;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player entered trigger for {gameObject.name}, transitioning to camera {virtualCamera?.name}");
            StartCoroutine(TransitionToCamera());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player exited trigger for {gameObject.name}");
            // Reset this camera's priority when player exits
            if (virtualCamera != null)
            {
                virtualCamera.Priority = DEFAULT_PRIORITY;
            }
        }
    }
    
    private IEnumerator TransitionToCamera()
    {
        if (fadeManager != null && virtualCamera != null)
        {
            // Fade to black
            fadeManager.FadeToBlack(fadeOutDuration);
            yield return new WaitForSeconds(fadeOutDuration);
            
            // Switch camera (while screen is black)
            virtualCamera.Priority = ACTIVE_PRIORITY;
            
            // Short delay while black
            yield return new WaitForSeconds(0.1f);
            
            // Fade back in
            fadeManager.FadeFromBlack(fadeInDuration);
        }
        else
        {
            // If no fade manager, just switch camera directly
            if (virtualCamera != null)
                virtualCamera.Priority = ACTIVE_PRIORITY;
        }
    }
}
