using UnityEngine;

public enum AttackType
{
    Magic, Sword, Shield
}

static class Weak
{
    public static int Success(AttackType hero, AttackType villain)
    {
        int success = 0;

        if ((hero == AttackType.Magic && villain == AttackType.Shield) ||
            (hero == AttackType.Sword && villain == AttackType.Magic) ||
            (hero == AttackType.Shield && villain == AttackType.Sword))
        {
            success = 1;
        }
        else if ((villain == AttackType.Magic && hero == AttackType.Shield) ||
            (villain == AttackType.Sword && hero == AttackType.Magic) ||
            (villain == AttackType.Shield && hero == AttackType.Sword))
        {
            success = -1;
        }

        return success;
    }
}

