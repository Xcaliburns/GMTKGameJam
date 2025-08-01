using UnityEngine;

public class CursedAxe : MonoBehaviour
{
    [Header("Movement Settings")]
    public float rotationSpeed = 360f;
    public float moveSpeed = 2f;
    public Transform startPoint;
    public Transform endPoint;

    [Header("Debug Visualization")]
    public bool showPath = true;
    public Color pathColor = Color.red;

    private float journeyProgress = 0f;
    private bool movingForward = true;
    
    void Start()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
        
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("Cursed Axe missing start or end point references!");
        }
    }

    void Update()
    {
        if (startPoint == null || endPoint == null) return;
        
        // Rotate the axe
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        
        // Update journey progress
        if (movingForward)
        {
            journeyProgress += moveSpeed * Time.deltaTime;
            if (journeyProgress >= 1f)
            {
                journeyProgress = 1f;
                movingForward = false;
            }
        }
        else
        {
            journeyProgress -= moveSpeed * Time.deltaTime;
            if (journeyProgress <= 0f)
            {
                journeyProgress = 0f;
                movingForward = true;
            }
        }
        
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, journeyProgress);
    }    
      
    void OnDrawGizmos()
    {
        if (!showPath || startPoint == null || endPoint == null) return;
        
        Gizmos.color = pathColor;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
        Gizmos.DrawSphere(startPoint.position, 0.2f);
        Gizmos.DrawSphere(endPoint.position, 0.2f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Vector2 hitDirection = transform.position - collision.transform.position;
                playerController.nbrMagic--; 
                playerController.HandleDamage(hitDirection); // Assuming HandleDamage handles the damage logic
                Debug.Log("Cursed Axe hit the player!");
            }
        }
    }
}
