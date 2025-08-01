using UnityEngine;

public abstract class BulletPattern : MonoBehaviour
{
    public enum Type { Spiral }

    [Header("Bullet Settings")]
    [SerializeField] protected Bullet bulletPrefab;

    protected Type type;
}
