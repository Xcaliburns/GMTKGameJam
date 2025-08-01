using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Boss : MonoBehaviour
{
    [SerializeField] BossData data;

    BossState state;

    void Start()
    {
        state = new(data);
        StartAttack();
    }

    void StartAttack()
    {
        if (state.attackCoroutine != null)
        {
            StopCoroutine(state.attackCoroutine);
            state.attackCoroutine = null;
        }

        if (state.bulletPat != null)
            Destroy(state.bulletPat.gameObject);

        switch (state.phase)
        {
            case 1:
                state.attackCoroutine = StartCoroutine(BulletPatternSwitch());
                break;

            case 2:
                state.attackCoroutine = StartCoroutine(AttackTypeSwitch());
                break;

            default:
                break;
        }
    }

    IEnumerator BulletPatternSwitch()
    {
        do
        {
            int rand = Random.Range(0, data.bulletPatPrefabs.Count);

            if (state.bulletPat != null && data.bulletPatPrefabs[rand].name == state.bulletPat.name)
                rand = (rand + 1) % data.bulletPatPrefabs.Count;

            if (state.bulletPat != null)
                Destroy(state.bulletPat.gameObject);

            state.bulletPat = Instantiate(data.bulletPatPrefabs[rand], transform);
            state.bulletPat.name = data.bulletPatPrefabs[rand].name;

            yield return new WaitForSeconds(data.bulletPatternSwitchRate);

        } while (data.canSwitchBulletPattern);
    }

    IEnumerator AttackTypeSwitch()
    {
        while (data.canSwitchAttackType)
        {
            yield return new WaitForSeconds(data.attackTypeSwitchRate);
            //Destroy(state.bulletPattern);
            //state.currentBulletPattern = Instantiate(data.bulletPaterns[Random.Range(0, data.bulletPaterns.Count)], transform);
        }
    }

    public void SwitchPhase()
    {
        state.phase = state.phase == 1 ? 2 : 1;
        StartAttack();
    }
}
