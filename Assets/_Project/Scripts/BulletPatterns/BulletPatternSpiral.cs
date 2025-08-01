using UnityEngine;

public class BulletPatternSpiral : BulletPattern
{
    [Header("Spiral Settings")]
    public float fireRate = 0.05f;
    public float rotationSpeed = 10f;
    public int nbDirections = 1;
    public bool clockwise = true;

    float fireTimer = 0f;

    private void Awake()
    {
        type = Type.Spiral;
    }

    void Update()
    {
        transform.Rotate(((clockwise ? -rotationSpeed : rotationSpeed) * Time.deltaTime) * Vector3.forward);

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            fireTimer -= fireRate;

            for (int i = 0; i < nbDirections; i++)
            {
                //float angle = transform.eulerAngles.z + i * (360f / nbDirections);
                Bullet bullet = BulletPool.Instance.Pool.Get();
                bullet.transform.position = transform.position;
                //bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                bullet.transform.right = RotateVector(transform.right, +i * (360f / nbDirections));
            }
        }
    }

    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
