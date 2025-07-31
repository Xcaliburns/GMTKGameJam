using UnityEngine;

public class ZRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation in degrees per second
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the object around its z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
