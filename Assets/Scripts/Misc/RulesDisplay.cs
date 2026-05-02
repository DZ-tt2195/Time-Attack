using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MyBox;
public class RulesDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [ReadOnly] public RulesManager thisRule;
    public Button button;

    public void AssignRule(RulesManager rule)
    {
        thisRule = rule;
        nameText.text = Translator.inst.Translate($"{rule.name}");
        descriptionText.text = Translator.inst.Translate($"{rule.name}_Text");
    }
}
