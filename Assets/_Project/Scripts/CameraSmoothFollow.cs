using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    public Transform target; // The target to follow
    public float smoothSpeed = 5f; // Speed of the camera smoothing
    public Vector3 offset = new Vector3(0, 0, -10); // Offset from the target position

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the desired position with offset
            // For a 2D game, we typically want to follow X and Y but keep Z fixed
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            offset.z
        );

        // Use SmoothDamp for more natural camera movement
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            desiredPosition, 
            ref velocity, 
            1f / smoothSpeed
        );
    }

    void Start()
    {
        // Make sure we're using an orthographic camera for 2D
        if (GetComponent<Camera>())
        {
            GetComponent<Camera>().orthographic = true;
        }
    }
}
