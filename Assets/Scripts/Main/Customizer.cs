using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using MyBox;
using System.Linq;
[System.Serializable]
public class RulesSlider
{
    public Slider slider;
    public TMP_Text textBox;
}

public class Customizer : MonoBehaviour
{
    public static Customizer inst;
    [Foldout("Difficulty", true)]
    [SerializeField] Slider difficultySlider;
    [SerializeField] TMP_Text difficultyLabel;
    [Foldout("Rules Choosing", true)]
    [SerializeField] TMP_Text chooseRulesText;
    [SerializeField] RulesDisplay displayPrefab;
    [SerializeField] Transform storeWeapons;
    [Foldout("Rules UI", true)]
    [SerializeField] List<RulesDisplay> displaysOnScreen;
    HashSet<Rule> currentRules = new();
    [SerializeField] List<RulesSlider> rulesSliders = new();
    void Awake()
    {
        inst = this;
        DifficultyInfo();
        RulesSetup();
    }
    void DifficultyInfo()
    {
        if (!PlayerPrefs.HasKey(PrefManager.Difficulty)) PrefManager.SetDifficulty(1f);
        difficultySlider.onValueChanged.AddListener(UpdateDifficultyText);
        difficultySlider.value = PrefManager.GetDifficulty();
        UpdateDifficultyText(PrefManager.GetDifficulty());

        void UpdateDifficultyText(float value)
        {
            difficultyLabel.text = AutoTranslate.Difficulty($"{value*100:F0}");
            PrefManager.SetDifficulty(value);
        }
    }    
    void RulesSetup()
    {
        if (GameFiles.inst.CurrentLevel().includeRules)
        {
            chooseRulesText.text = AutoTranslate.Choose_Rule();
            List<Rule> allRules = GameFiles.inst.AllRules();

            foreach (Rule rule in allRules)
            {
                RulesDisplay nextDisplay = Instantiate(displayPrefab, storeWeapons);
                nextDisplay.AssignRule(rule);
                nextDisplay.toggle.isOn = false;
                nextDisplay.toggle.onValueChanged.AddListener(RulesToggle);

                void RulesToggle(bool enabled)
                {
                    if (enabled)
                    {
                        currentRules.Add(rule);
                        if (currentRules.Count > displaysOnScreen.Count)
                            nextDisplay.toggle.isOn = false;
                        else
                            AudioManager.instance.Menu();
                    }
                    else
                    {
                        AudioManager.instance.Menu();
                        currentRules.Remove(rule);
                    }
                }
            }
        }
        else
        {
            storeWeapons.gameObject.SetActive(false);
            chooseRulesText.gameObject.SetActive(false);
        }
    }    
    public void BeginGame()
    {
        difficultySlider.gameObject.SetActive(false);
        storeWeapons.transform.parent.gameObject.SetActive(false);
        chooseRulesText.gameObject.SetActive(false);

        if (GameFiles.inst.CurrentLevel().includeRules)
        {
            List<Rule> allRules = GameFiles.inst.AllRules();
            while (currentRules.Count < displaysOnScreen.Count)
            {
                int randomNumber = Random.Range(0, allRules.Count);
                currentRules.Add(allRules[randomNumber]);
            }

            List<Rule> selectedRules = currentRules.ToList();
            for (int i = 0; i<selectedRules.Count; i++)
            {
                Rule newRule = Instantiate(selectedRules[i]);
                newRule.AssignSlider(rulesSliders[i]);
                displaysOnScreen[i].AssignRule(newRule);
            }
        }
    }
}
