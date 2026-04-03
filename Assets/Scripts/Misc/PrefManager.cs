using UnityEngine;

public static class PrefManager
{
    //playerprefs
    public const string Difficulty = nameof(Difficulty);
    public static float GetDifficulty() => PlayerPrefs.GetFloat(Difficulty);
    public static void SetDifficulty(float value) => PlayerPrefs.SetFloat(Difficulty, value);

    public static int GetScore(string levelName) => PlayerPrefs.GetInt($"{levelName} - Best Score");
    public static void SetScore(string levelName, int value) => PlayerPrefs.SetInt($"{levelName} - Best Score", value);

    public const string StartWave = nameof(StartWave);
    public static int GetStartWave() => PlayerPrefs.GetInt(StartWave);
    public static void SetStartWave(int value) => PlayerPrefs.SetInt(StartWave, value);

    public const string CurrentLevel = nameof(CurrentLevel);
    public static int GetCurrentLevel() => PlayerPrefs.GetInt(CurrentLevel);
    public static void SetCurrentLevel(int value) => PlayerPrefs.SetInt(CurrentLevel, value);

}

