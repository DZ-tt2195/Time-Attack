using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MyBox;
public class RulesDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [ReadOnly] public EnergyManager thisRule;
    public Button button;

    public void AssignRule(EnergyManager rule)
    {
        thisRule = rule;
        nameText.text = Translator.inst.Translate($"{rule.name}");
        descriptionText.text = Translator.inst.Translate($"{rule.name}_Text");
    }
}
