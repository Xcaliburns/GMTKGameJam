using UnityEngine;

public class BulletPool : Singleton<BulletPool>
{
    [SerializeField] Bullet prefab;
    [SerializeField] int initialSize;
    [SerializeField] Transform parent;

    ObjectPool<Bullet> pool = null;

    protected override void Awake()
    {
        pool = new(prefab, initialSize, parent);
    }

    public ObjectPool<Bullet> Pool => pool;
}
