using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class RoomCameraSwitcher : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    public float fadeInDuration = 0.1f;
    public float fadeOutDuration = 0.1f;
    [Range(0, 1)]
    public float teleportPosition = 0.3f; // Position between entrance (0) and center (1)
    public bool pauseGameDuringTransition = false; // Changed to false by default
    float extraDelay = 0.1f;

    // Default priority for inactive cameras
    private const int DEFAULT_PRIORITY = 10;
    // Priority when camera is active
    private const int ACTIVE_PRIORITY = 20;

    // Reference to the FadeManager (must be setup in the scene)
    private FadeManager fadeManager;
    // Reference to the Cinemachine Brain to control transitions
    private CinemachineBrain cinemachineBrain;
    // Current player reference
    private Transform playerTransform;
    private GameObject Player;
    // Position where player entered the room trigger
    private Vector3 entrancePosition;
    // Reference to player's controller component
    private MonoBehaviour playerController;
    // Store original time scale
    private float originalTimeScale;

    
    public float invincibilityAfterTransition = 0.5f;

    private void Start()
    {
        // Find the FadeManager in the scene
        fadeManager = FindObjectOfType<FadeManager>();
        if (fadeManager == null)
        {
            Debug.LogWarning("FadeManager not found in scene. Create one for camera transitions to work properly.");
        }

        // Find the CinemachineBrain
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        if (cinemachineBrain == null)
        {
            Debug.LogWarning("CinemachineBrain not found in scene. Camera transitions might not work as expected.");
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
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player entered trigger for {gameObject.name}, transitioning to camera {virtualCamera?.name}");
            playerTransform = other.transform;
            entrancePosition = playerTransform.position; // Store entrance position

            //  PlayerController reference
            playerController = Player.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogWarning("Could not find PlayerController on Player");
            }

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
            // Store original time scale
            originalTimeScale = Time.timeScale;

            // Invicibility handling
            PlayerController playerControllerTyped = Player.GetComponent<PlayerController>();
            if (playerControllerTyped != null)
            {
                // Activate invincibility
                playerControllerTyped.isKnockedBack = true;
                Debug.Log("Player set to invincible (isKnockedBack = true) for transition");

                // deactivate player controls
                playerControllerTyped.enabled = false;
                Debug.Log($"Disabled player controls during transition");
            }
            else
            {
                Debug.LogWarning("Could not find PlayerController on Player");
            }

            // Instead of completely pausing the game, set it to a very small value if needed
            if (pauseGameDuringTransition)
            {
                Time.timeScale = 0.001f; // Very slow instead of completely stopped
                Debug.Log("Game slowed to near-pause during camera transition");
            }

            // 1. FADE TO BLACK
            fadeManager.FadeToBlack(fadeOutDuration);
            yield return new WaitForSecondsRealtime(fadeOutDuration + 0.05f); // Extra time to ensure fully black

            // 2. TELEPORT PLAYER FIRST (while screen is black)
            if (playerTransform != null && virtualCamera.Follow != null)
            {
                // Calculate position between entrance and the camera's Follow target
                Vector3 roomCenter = virtualCamera.Follow.position;
                Vector3 targetPosition = Vector3.Lerp(entrancePosition, roomCenter, teleportPosition);

                // For 2D games, preserve the player's original z position (depth)
                targetPosition.z = playerTransform.position.z;

                // Teleport the player
                playerTransform.position = targetPosition;
                Debug.Log($"Teleported player from {entrancePosition} to {targetPosition} (toward {roomCenter})");
            }

            // 3. CAMERA CHANGE (after player teleport)
            CinemachineBlendDefinition? originalBlend = null;
            if (cinemachineBrain != null)
            {
                originalBlend = cinemachineBrain.DefaultBlend;
                cinemachineBrain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0);
            }

            // Switch to new camera
            virtualCamera.Priority = ACTIVE_PRIORITY;

            // Wait for camera change to complete
            yield return new WaitForSecondsRealtime(0.1f);

            // 4. FADE BACK IN to reveal the new scene
            fadeManager.FadeFromBlack(fadeInDuration);
            yield return new WaitForSecondsRealtime(fadeInDuration);

            // Re-enable player controls but keep invincibility for a short time
            if (playerControllerTyped != null)
            {
                // re-enable player controls
                playerControllerTyped.enabled = true;
                Debug.Log($"Re-enabled player controls after transition");

                // maintain invincibility for a short duration
                yield return new WaitForSecondsRealtime(invincibilityAfterTransition);
                
                
                playerControllerTyped.isKnockedBack = false;
                Debug.Log("Player invincibility ended after transition");
            }

            // Restore original time scale
            if (pauseGameDuringTransition)
            {
                Time.timeScale = originalTimeScale;
                Debug.Log("Game resumed after camera transition");
            }
        }
        else
        {
            // If no fade manager, just switch camera directly
            if (virtualCamera != null)
                virtualCamera.Priority = ACTIVE_PRIORITY;
        }
    }
}

// This is a simple marker component to indicate invincibility during transitions
[AddComponentMenu("")] // Hide from Add Component menu
public class TransitionInvincibilityMarker : MonoBehaviour { }
