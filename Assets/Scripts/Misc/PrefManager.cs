using UnityEngine;

public static class PrefManager
{
    public const string Difficulty = nameof(Difficulty);
    public static float GetDifficulty() => PlayerPrefs.GetFloat(Difficulty);
    public static void SetDifficulty(float value) => PlayerPrefs.SetFloat(Difficulty, value);

    public static int GetScore(string levelName) => PlayerPrefs.GetInt($"{levelName} - Best Score");
    public static void SetScore(string levelName, int value) => PlayerPrefs.SetInt($"{levelName} - Best Score", value);

    public const string CurrentLevel = nameof(CurrentLevel);
    public static int GetCurrentLevel() => PlayerPrefs.GetInt(CurrentLevel);
    public static void SetCurrentLevel(int value) => PlayerPrefs.SetInt(CurrentLevel, value);

    public const string CurrentWeapon = nameof(CurrentWeapon);
    public static int GetCurrentWeapon() => PlayerPrefs.GetInt(CurrentWeapon);
    public static void SetCurrentWeapon(int value) => PlayerPrefs.SetInt(CurrentWeapon, value);

    public const string CurrentSub = nameof(CurrentSub);
    public static int GetCurrentSub() => PlayerPrefs.GetInt(CurrentSub);
    public static void SetCurrentSub(int value) => PlayerPrefs.SetInt(CurrentSub, value);

}

