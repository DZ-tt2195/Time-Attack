using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MyBox;
public class RulesDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    public Toggle toggle;

    public void AssignRule(Rule rule)
    {
        this.gameObject.SetActive(true);
        nameText.text = Translator.inst.Translate(rule.name);
        descriptionText.text = Translator.inst.Translate($"{rule.name}_Text", new() {("Num", rule.GetTime.ToString())});
    }
}
