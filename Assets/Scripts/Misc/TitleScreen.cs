using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using MyBox;

public class TitleScreen : MonoBehaviour
{
    [Foldout("UI", true)]
    [SerializeField] Slider difficultySlider;
    [SerializeField] TMP_Text difficultyLabel;
    [SerializeField] TMP_Text bestRun;
    [SerializeField] TMP_Dropdown levelDropdown;
    [SerializeField] Button deleteScoreButton;
    [SerializeField] Slider volumeSlider;
    [Foldout("Texts", true)]
    [SerializeField] TMP_Text designer;
    [SerializeField] TMP_Text description;
    [SerializeField] TMP_Text lastUpdate;
    [SerializeField] TMP_Text controls;
    [SerializeField] TMP_Text play;
    [SerializeField] TMP_Text chooseLevel;
    [SerializeField] TMP_Text deleteScores;
    [SerializeField] TMP_Text volume;

    void Awake()
    {
        designer.text = AutoTranslate.Designer();
        description.text = AutoTranslate.Description();
        lastUpdate.text = AutoTranslate.Last_Update();
        controls.text = AutoTranslate.Controls();
        play.text = AutoTranslate.Play();
        chooseLevel.text = AutoTranslate.Choose_Level();
        deleteScores.text = AutoTranslate.Delete();

        if (!PlayerPrefs.HasKey(PrefManager.Difficulty)) PrefManager.SetDifficulty(1f);
        difficultySlider.onValueChanged.AddListener(UpdateDifficultyText);
        difficultySlider.value = PrefManager.GetDifficulty();
        UpdateDifficultyText(PrefManager.GetDifficulty());

        void UpdateDifficultyText(float value)
        {
            difficultyLabel.text = AutoTranslate.Difficulty($"{value*100:F0}");
            PrefManager.SetDifficulty(value);
        }

        if (!PlayerPrefs.HasKey(PrefManager.CurrentLevel)) PrefManager.SetCurrentLevel(0);
        levelDropdown.onValueChanged.AddListener(ChangeLevelDropdown);
        List<Level> listOfLevels = ThingsToCarry.inst.AllLevels();
        for (int i = 0; i < listOfLevels.Count; i++)
        {
            Level nextLevel = listOfLevels[i];
            if (nextLevel.levelName == AutoTranslate.Blank() && !Application.isEditor)
                continue;
            
            levelDropdown.AddOptions(new List<string>() { Translator.inst.Translate(nextLevel.levelName) });
            if (i == PrefManager.GetCurrentLevel())
            {
                levelDropdown.value = i;
                ChangeLevelDropdown(i);
            }
        }

        deleteScoreButton.onClick.AddListener(ClearScores);
        void ClearScores()
        {
            foreach (Level level in listOfLevels)
                PrefManager.SetScore(level.levelName.ToString(), 0);
            ChangeLevelDropdown(PrefManager.GetCurrentLevel());
        }

        void ChangeLevelDropdown(int n)
        {
            PrefManager.SetCurrentLevel(n);
            Level newLevel = ThingsToCarry.inst.AllLevels()[n];

            if (PrefManager.GetScore(newLevel.levelName.ToString()) > 0)
                bestRun.text = AutoTranslate.Best_Score($"{PrefManager.GetScore(newLevel.levelName.ToString())}");
            else
                bestRun.text = AutoTranslate.No_Score();
        }
    
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        volumeSlider.onValueChanged.AddListener(SetLevel);
        SetLevel(PlayerPrefs.GetFloat("Volume"));

        void SetLevel(float value)
        {
            AudioManager.instance.mixer.SetFloat("Volume", (Mathf.Log10(volumeSlider.value) * 20));
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        }
    }
}
