using UnityEngine;



public abstract class BulletPattern : MonoBehaviour
{
    public enum Type { Spiral }

    [Header("Bullet Settings")]
    [SerializeField] protected Bullet bulletPrefab;
    //[SerializeField] protected float bulletSpeed = 5f;

    protected Type type;
}
