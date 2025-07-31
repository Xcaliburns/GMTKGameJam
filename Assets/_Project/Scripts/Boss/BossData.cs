using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Scriptable Objects/BossData")]
public class BossData : ScriptableObject
{
    [Header("First encounter")]
    [Range(1,2)] public int phase = 1;
    public int shieldPoints = 3;
    public int swordPoints = 3;
    public int magicPoints = 3;

    [Header("Bullet Patterns")]
    public float bulletPatternSwitchRate = 3f;
    public bool canSwitchBulletPattern = true;
    public List<BulletPattern> bulletPaterns;

    [Header("Attack Types")]
    public float attackTypeSwitchRate = 3f;
    public bool canSwitchAttackType = true;
    //public List<BulletPattern> bulletPaterns;
}
