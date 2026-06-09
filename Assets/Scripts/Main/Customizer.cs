using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Customizer : MonoBehaviour
{
    public static Customizer inst;
    [SerializeField] WeaponDisplay weaponInfo;
    [SerializeField] Slider difficultySlider;
    [SerializeField] TMP_Text difficultyLabel;
    [SerializeField] Button random;
    [SerializeField] TMP_Text chooseSubWeaponText;
    [SerializeField] WeaponDisplay displayPrefab;
    [SerializeField] WeaponDisplay displayOnScreen;
    [SerializeField] Transform storeWeapons;
    void Awake()
    {
        inst = this;
        chooseSubWeaponText.text = AutoTranslate.Choose_SubWeapon();
        DifficultyInfo();
        WeaponInfo();
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
    void WeaponInfo()
    {
        chooseSubWeaponText.text = AutoTranslate.Choose_SubWeapon();
        List<SubWeapon> allSubWeapons = ThingsToCarry.inst.AllSubs();
        if (!PlayerPrefs.HasKey(PrefManager.CurrentSub)) PrefManager.SetCurrentSub(-1);
        random.onClick.AddListener(() => SetWeapon(-1));
        random.transform.GetComponentInChildren<TMP_Text>().text = AutoTranslate.Random();

        for (int i = 0; i<allSubWeapons.Count; i++)
        {
            int n = i;
            WeaponDisplay nextDisplay = Instantiate(displayPrefab, storeWeapons);
            nextDisplay.AssignWeapon(allSubWeapons[n]);
            nextDisplay.button.onClick.AddListener(() => SetWeapon(n));
        }
        SetWeapon(PrefManager.GetCurrentSub());

        void SetWeapon(int n)
        {
            AudioManager.instance.Menu();
            PrefManager.SetCurrentSub(n);
            displayOnScreen.AssignWeapon(ThingsToCarry.inst.RandomSub());
        }        
    }    
    public void BeginGame()
    {
        difficultySlider.gameObject.SetActive(false);
        storeWeapons.gameObject.SetActive(false);
        chooseSubWeaponText.gameObject.SetActive(false);
        random.gameObject.SetActive(false);
    }
}
