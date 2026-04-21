using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MyBox;
public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] Image weaponSprite;
    [SerializeField] TMP_Text descriptionText;
    [ReadOnly] public Player thisPlayer;
    public Button button;

    public void AssignWeapon(Player player)
    {
        thisPlayer = player;
        weaponSprite.sprite = player.spriteRenderer.sprite;
        weaponSprite.color = player.spriteRenderer.color;
        descriptionText.text = Translator.inst.Translate($"{player.name}_Text");
    }
}
