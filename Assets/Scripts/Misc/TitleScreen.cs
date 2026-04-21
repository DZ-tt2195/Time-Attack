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
    [SerializeField] Button changeWeapon;
    [SerializeField] TMP_Text currentWeaponText;
    [SerializeField] Transform weaponScreen;
    [SerializeField] Transform storeWeapons;
    [SerializeField] WeaponDisplay displayPrefab;
    [SerializeField] Button randomWeapon;
    [Foldout("Texts", true)]
    [SerializeField] TMP_Text designer;
    [SerializeField] TMP_Text description;
    [SerializeField] TMP_Text lastUpdate;
    [SerializeField] TMP_Text controls;
    [SerializeField] TMP_Text play;
    [SerializeField] TMP_Text chooseLevel;
    [SerializeField] TMP_Text chooseWeapon;
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
        chooseWeapon.text = AutoTranslate.Choose_Weapon();

        LevelInfo();
        WeaponInfo();
        VolumeInfo();
        DifficultyInfo();
    }
    void LevelInfo()
    {
        if (!PlayerPrefs.HasKey(PrefManager.CurrentLevel)) PrefManager.SetCurrentLevel(0);
        List<Level> listOfLevels = ThingsToCarry.inst.AllLevels();

        levelDropdown.onValueChanged.AddListener(ChangeLevelDropdown);
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
            Level newLevel = listOfLevels[n];

            if (PrefManager.GetScore(newLevel.levelName.ToString()) > 0)
                bestRun.text = AutoTranslate.Best_Score($"{PrefManager.GetScore(newLevel.levelName.ToString())}");
            else
                bestRun.text = AutoTranslate.No_Score();
        }        
    }
    void WeaponInfo()
    {
        List<Player> allWeapons = ThingsToCarry.inst.AllWeapons();
        if (!PlayerPrefs.HasKey(PrefManager.CurrentWeapon)) PrefManager.SetCurrentWeapon(-1);
        changeWeapon.onClick.AddListener(() => weaponScreen.gameObject.SetActive(true));
        randomWeapon.onClick.AddListener(() => SetWeapon(-1));
        randomWeapon.transform.GetComponentInChildren<TMP_Text>().text = AutoTranslate.Random();
        
        for (int i = 0; i<allWeapons.Count; i++)
        {
            int n = i;
            WeaponDisplay nextDisplay = Instantiate(displayPrefab, storeWeapons);
            nextDisplay.AssignWeapon(allWeapons[n]);
            nextDisplay.button.onClick.AddListener(() => PickedMe(n));

            void PickedMe(int n)
            {
                SetWeapon(n);
            }
        }
        SetWeapon(PrefManager.GetCurrentWeapon());

        void SetWeapon(int n)
        {
            weaponScreen.gameObject.SetActive(false);
            PrefManager.SetCurrentWeapon(n);
            if (n == -1)
                currentWeaponText.text = AutoTranslate.Random();
            else
                currentWeaponText.text = Translator.inst.Translate(allWeapons[n].name);
        }        
    }
    void VolumeInfo()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        volumeSlider.onValueChanged.AddListener(SetLevel);
        SetLevel(PlayerPrefs.GetFloat("Volume"));

        void SetLevel(float value)
        {
            AudioManager.instance.mixer.SetFloat("Volume", (Mathf.Log10(volumeSlider.value) * 20));
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        }        
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
}
