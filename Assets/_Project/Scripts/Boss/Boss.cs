using System.Collections;
using UnityEngine;

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
        switch (state.phase)
        {
            case 1:
                state.bulletPattern = Instantiate(data.bulletPaterns[Random.Range(0, data.bulletPaterns.Count)], transform);
                StartCoroutine(BulletPatternSwitch());
                break;
            case 2:
                StartCoroutine(AttackTypeSwitch());
                break;

            default:
                break;
        }
    }

    IEnumerator BulletPatternSwitch()
    {
        while (data.canSwitchBulletPattern)
        {
            yield return new WaitForSeconds(data.bulletPatternSwitchRate);
            if (state.bulletPattern != null) Destroy(state.bulletPattern.gameObject);
            state.bulletPattern = Instantiate(data.bulletPaterns[Random.Range(0, data.bulletPaterns.Count)], transform);
        }
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
    }
}
