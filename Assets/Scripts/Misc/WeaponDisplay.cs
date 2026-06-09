using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MyBox;
public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    public Button button;

    public void AssignWeapon(SubWeapon weapon)
    {
        nameText.text = Translator.inst.Translate(weapon.name);
        descriptionText.text = Translator.inst.Translate($"{weapon.name}_Text");
    }
}
