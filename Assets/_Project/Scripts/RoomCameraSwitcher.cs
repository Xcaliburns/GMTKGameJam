using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class RoomCameraSwitcher : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    public float fadeInDuration = 0.1f;
    public float fadeOutDuration = 0.1f;
    [Range(0, 1)]
    public float teleportPosition = 0.3f;
    public bool pauseGameDuringTransition = false;
    float extraDelay = 0.1f;
    public float invincibilityAfterTransition = 0.5f;

    // Camera priorities
    private const int DEFAULT_PRIORITY = 10;
    private const int ACTIVE_PRIORITY = 20;

    // References
    private FadeManager fadeManager;
    private CinemachineBrain cinemachineBrain;
    private Transform playerTransform;
    private GameObject Player;
    private Vector3 entrancePosition;
    private MonoBehaviour playerController;
    private float originalTimeScale;

    private void Start()
    {
        fadeManager = FindObjectOfType<FadeManager>();
        if (fadeManager == null)
        {
            Debug.LogWarning("FadeManager not found in scene. Create one for camera transitions to work properly.");
        }

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
            entrancePosition = playerTransform.position;

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
            originalTimeScale = Time.timeScale;

            PlayerController playerControllerTyped = Player.GetComponent<PlayerController>();
            if (playerControllerTyped != null)
            {
                playerControllerTyped.isKnockedBack = true;
                Debug.Log("Player set to invincible (isKnockedBack = true) for transition");

                playerControllerTyped.enabled = false;
                Debug.Log($"Disabled player controls during transition");
            }
            else
            {
                Debug.LogWarning("Could not find PlayerController on Player");
            }

            if (pauseGameDuringTransition)
            {
                Time.timeScale = 0.001f;
                Debug.Log("Game slowed to near-pause during camera transition");
            }

            // 1. Fade to black
            fadeManager.FadeToBlack(fadeOutDuration);
            yield return new WaitForSecondsRealtime(fadeOutDuration + 0.05f);

            // 2. Teleport player while screen is black
            if (playerTransform != null && virtualCamera.Follow != null)
            {
                Vector3 roomCenter = virtualCamera.Follow.position;
                Vector3 targetPosition = Vector3.Lerp(entrancePosition, roomCenter, teleportPosition);
                targetPosition.z = playerTransform.position.z;

                playerTransform.position = targetPosition;
                Debug.Log($"Teleported player from {entrancePosition} to {targetPosition} (toward {roomCenter})");
            }

            // 3. Switch camera
            CinemachineBlendDefinition? originalBlend = null;
            if (cinemachineBrain != null)
            {
                originalBlend = cinemachineBrain.DefaultBlend;
                cinemachineBrain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0);
            }

            virtualCamera.Priority = ACTIVE_PRIORITY;
            yield return new WaitForSecondsRealtime(0.1f);

            // 4. Fade back in
            fadeManager.FadeFromBlack(fadeInDuration);
            yield return new WaitForSecondsRealtime(fadeInDuration);

            if (playerControllerTyped != null)
            {
                playerControllerTyped.enabled = true;
                Debug.Log($"Re-enabled player controls after transition");

                yield return new WaitForSecondsRealtime(invincibilityAfterTransition);
                
                playerControllerTyped.isKnockedBack = false;
                Debug.Log("Player invincibility ended after transition");
            }

            if (pauseGameDuringTransition)
            {
                Time.timeScale = originalTimeScale;
                Debug.Log("Game resumed after camera transition");
            }
        }
        else
        {
            if (virtualCamera != null)
                virtualCamera.Priority = ACTIVE_PRIORITY;
        }
    }
}

// This is a simple marker component to indicate invincibility during transitions
[AddComponentMenu("")] // Hide from Add Component menu
public class TransitionInvincibilityMarker : MonoBehaviour { }
