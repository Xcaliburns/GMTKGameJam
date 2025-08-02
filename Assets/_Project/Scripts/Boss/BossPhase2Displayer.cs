using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossPhase2Displayer : MonoBehaviour
{
    [SerializeField] Sprite spellSprite;
    [SerializeField] Sprite swordSprite;
    [SerializeField] Sprite shieldSprite;

    [SerializeField] TextMeshProUGUI spellAmountText;
    [SerializeField] TextMeshProUGUI swordAmountText;
    [SerializeField] TextMeshProUGUI shieldAmountText;

    [SerializeField] Image bossSelectedImage;
    [SerializeField] Image playerSelectedImage;

    public Button spellButton;
    public Button swordButton;
    public Button shieldButton;

    public void SetAttackAmount(AttackType attackType, int amount)
    {
        switch (attackType)
        {
            case AttackType.Magic:
                spellAmountText.text = amount.ToString();
                break;
            case AttackType.Sword:
                swordAmountText.text = amount.ToString();
                break;
            case AttackType.Shield:
                shieldAmountText.text = amount.ToString();
                break;
        }
    }

    public void SetBossAttackSprite(AttackType attackType)
    {
        bossSelectedImage.color = new Color(1, 1, 1, 1);

        switch (attackType)
        {
            case AttackType.Magic:
                bossSelectedImage.sprite = spellSprite;
                break;
            case AttackType.Sword:
                bossSelectedImage.sprite = swordSprite;
                break;
            case AttackType.Shield:
                bossSelectedImage.sprite = shieldSprite;
                break;
        }
    }

    public void SetPlayerAttackSprite(AttackType attackType)
    {
        playerSelectedImage.color = new Color(1, 1, 1, 1);

        switch (attackType)
        {
            case AttackType.Magic:
                playerSelectedImage.sprite = spellSprite;
                break;
            case AttackType.Sword:
                playerSelectedImage.sprite = swordSprite;
                break;
            case AttackType.Shield:
                playerSelectedImage.sprite = shieldSprite;
                break;
        }
    }
    public void SetBossAttackSpriteAlpha(float alpha)
    {
        bossSelectedImage.color = new Color(1, 1, 1, alpha);
    }

    public void SetPlayerAttackSpriteAlpha(float alpha)
    {
        playerSelectedImage.color = new Color(1, 1, 1, alpha);
    }
}
