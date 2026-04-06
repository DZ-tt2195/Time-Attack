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
    [SerializeField] Slider waveSlider;
    [SerializeField] TMP_Text waveLabel;
    [SerializeField] TMP_Text bestRun;
    [SerializeField] TMP_Dropdown levelDropdown;
    [SerializeField] Button deleteScoreButton;

    [Foldout("Texts", true)]
    [SerializeField] TMP_Text designer;
    [SerializeField] TMP_Text description;
    [SerializeField] TMP_Text lastUpdate;
    [SerializeField] TMP_Text controls;
    [SerializeField] TMP_Text play;
    [SerializeField] TMP_Text chooseLevel;
    [SerializeField] TMP_Text deleteScores;

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

        if (!PlayerPrefs.HasKey(PrefManager.StartWave)) PrefManager.SetStartWave(1);
        waveSlider.onValueChanged.AddListener(UpdateWaveText);
        waveSlider.value = PrefManager.GetStartWave();
        UpdateWaveText(PrefManager.GetStartWave());

        void UpdateWaveText(float value)
        {
            waveLabel.text = AutoTranslate.Start_on_Wave(((int)(value)).ToString());
            PrefManager.SetStartWave((int)value);
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

            if (newLevel.levelType == LevelType.Endless || newLevel.listOfWaves.Count == 1)
            {
                waveSlider.value = 1;
                waveSlider.gameObject.SetActive(false);
            }
            else
            {
                waveSlider.maxValue = newLevel.listOfWaves.Count;
                waveSlider.value = 1;
                waveSlider.gameObject.SetActive(true);
            }

            if (PrefManager.GetScore(newLevel.levelName.ToString()) > 0)
                bestRun.text = AutoTranslate.Best_Score($"{PrefManager.GetScore(newLevel.levelName.ToString())}");
            else
                bestRun.text = AutoTranslate.No_Score();
        }
    }

}
