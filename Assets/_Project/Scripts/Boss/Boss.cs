using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    [SerializeField] BossData data;
    [SerializeField] BossPhase2Displayer phase2DisplayerPrefab;

    BossState state;
    BossPhase2Displayer phase2Displayer;
    PlayerController player;

    void OnEnable()
    {    
        // Initialize state before using it
        if (state == null)
        {
            state = new BossState(data);
        }
        
        LoadPhase2Displayer();
        StartPhase();

        phase2Displayer.SetAttackAmount(AttackType.Magic, player.nbrMagic);
        phase2Displayer.SetAttackAmount(AttackType.Sword, player.nbrSword);
        phase2Displayer.SetAttackAmount(AttackType.Shield, player.nbrShield);
    }

    private void OnDisable()
    {
        StopCurrentAttack();
    }

    void LoadPhase2Displayer()
    {
        player = FindFirstObjectByType<PlayerController>();

        phase2Displayer = FindFirstObjectByType<BossPhase2Displayer>();
        if (phase2Displayer == null && phase2DisplayerPrefab != null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                phase2Displayer = Instantiate(phase2DisplayerPrefab, canvas.transform);
            }
        }

        phase2Displayer.spellButton.onClick.AddListener(() => SelectAttack(AttackType.Magic));
        phase2Displayer.swordButton.onClick.AddListener(() => SelectAttack(AttackType.Sword));
        phase2Displayer.shieldButton.onClick.AddListener(() => SelectAttack(AttackType.Shield));

        phase2Displayer.SetAttackAmount(AttackType.Magic, player.nbrMagic);
        phase2Displayer.SetAttackAmount(AttackType.Sword, player.nbrSword);
        phase2Displayer.SetAttackAmount(AttackType.Shield, player.nbrShield);

        phase2Displayer.gameObject.SetActive(false);
    }

    void StartPhase()
    {
        StopCurrentAttack();

        switch (state.phase)
        {
            case 1:
                phase2Displayer.gameObject.SetActive(false);
                state.attackCoroutine = StartCoroutine(BulletPatternSwitch());
                break;

            case 2:
                phase2Displayer.gameObject.SetActive(true);
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
        yield return new WaitForSeconds(1);

        do
        {
            phase2Displayer.SetPlayerAttackSpriteAlpha(0);

            int nbAttackType = System.Enum.GetValues(typeof(AttackType)).Length;
            int rand = Random.Range(0, nbAttackType);

            if (state.magicPoints == 0 && rand == (int)AttackType.Magic)
            {
                rand = (rand + 1) % nbAttackType;
            }
            if (state.swordPoints == 0 && rand == (int)AttackType.Sword)
            {
                rand = (rand + 1) % nbAttackType;
            }
            if (state.shieldPoints == 0 && rand == (int)AttackType.Shield)
            {
                rand = (rand + 1) % nbAttackType;
            }

            state.currAttackType = (AttackType)rand;

            phase2Displayer.SetBossAttackSprite(state.currAttackType);

            yield return new WaitForSeconds(data.attackTypeSwitchRate);

            switch (state.currAttackType)
            {
                case AttackType.Magic:
                    player.nbrMagic--;
                    phase2Displayer.SetAttackAmount(AttackType.Magic, player.nbrMagic);
                    break;
                case AttackType.Sword:
                    player.nbrSword--;
                    phase2Displayer.SetAttackAmount(AttackType.Sword, player.nbrSword);
                    break;
                case AttackType.Shield:
                    player.nbrShield--;
                    phase2Displayer.SetAttackAmount(AttackType.Shield, player.nbrShield);
                    break;
            }

        } while (data.canSwitchAttackType);
    }

    void StopCurrentAttack()
    {
        // Safety check to prevent NullReferenceException
        if (state == null)
            return;
            
        if (state.attackCoroutine != null)
        {
            StopCoroutine(state.attackCoroutine);
            state.attackCoroutine = null;
        }

        if (state.bulletPat != null)
            Destroy(state.bulletPat.gameObject);
    }

    public void SwitchPhase()
    {
        if (!state.requiem)
        {
            state.phase = state.phase == 1 ? 2 : 1;
            StartPhase();
        }
        else
        {
            state.magicPoints = player.nbrMagic;
            state.swordPoints = player.nbrSword;
            state.shieldPoints = player.nbrShield;
            GetComponent<SpriteRenderer>().sprite = player.GetComponent<SpriteRenderer>().sprite;
            gameObject.SetActive(false);
        }
    }

    public void SelectAttack(AttackType attackType)
    {

        if (state.magicPoints == 0 && attackType == AttackType.Magic)
        {
            return;
        }
        if (state.swordPoints == 0 && attackType == AttackType.Sword)
        {
            return;
        }
        if (state.shieldPoints == 0 && attackType == AttackType.Shield)
        {
            return;
        }

        StopCurrentAttack();

        phase2Displayer.SetPlayerAttackSprite(attackType);

        int success = Weak.Success(attackType, state.currAttackType);

        if (success > 0)
        {
            phase2Displayer.SetBossAttackSpriteAlpha(0.3f);

            switch (state.currAttackType)
            {
                case AttackType.Magic:
                    state.magicPoints--;
                    if (state.magicPoints == 0)
                    {
                        SwitchPhase();
                    }
                    else
                    {
                        StartPhase();
                    }
                    break;
                case AttackType.Sword:
                    state.swordPoints--;
                    if (state.swordPoints == 0)
                    {
                        SwitchPhase();
                    }
                    else
                    {
                        StartPhase();
                    }
                    break;
                case AttackType.Shield:
                    state.shieldPoints--;
                    if (state.shieldPoints == 0)
                    {
                        SwitchPhase();
                    }
                    else
                    {
                        StartPhase();
                    }
                    break;
            }
        }
        else if (success < 0)
        {
            phase2Displayer.SetPlayerAttackSpriteAlpha(0.3f);

            switch (attackType)
            {
                case AttackType.Magic:
                    player.nbrMagic--;
                    phase2Displayer.SetAttackAmount(AttackType.Magic, player.nbrMagic);
                    break;
                case AttackType.Sword:
                    player.nbrSword--;
                    phase2Displayer.SetAttackAmount(AttackType.Sword, player.nbrSword);
                    break;
                case AttackType.Shield:
                    player.nbrShield--;
                    phase2Displayer.SetAttackAmount(AttackType.Shield, player.nbrShield);
                    break;
            }
            StartPhase();

        }

        //StartCoroutine(WaitNextRound());
    }

    IEnumerator WaitNextRound()
    {
        yield return new WaitForSeconds(1);

        if (state.magicPoints == 0 || state.swordPoints == 0 || state.shieldPoints == 0)
        {
            SwitchPhase();
        }
        else
        {
            StartPhase();
        }


        state.requiem = state.magicPoints == 0 && state.swordPoints == 0 && state.shieldPoints == 0;
    }
}
