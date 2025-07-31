using UnityEngine;

public class BossState
{
    public BulletPattern bulletPattern;
    public int phase;
    public int shieldPoints, swordPoints, magicPoints;
    public BossState(BossData data)
    {
        phase = data.phase;
        shieldPoints = data.shieldPoints;
        swordPoints = data.swordPoints;
        magicPoints = data.magicPoints;
    }
}
