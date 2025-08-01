using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossState
{
    public BulletPattern bulletPat;
    public Coroutine attackCoroutine;
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
