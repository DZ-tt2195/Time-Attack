using UnityEngine;

public class ThingsToCarry : MonoBehaviour
{
    Level[] listOfLevels;
    BaseEnemy[] enemiesToSpawn;
    public static ThingsToCarry inst;

    void Awake()
    {
        inst = this;
        listOfLevels = Resources.LoadAll<Level>("Levels");
        enemiesToSpawn = Resources.LoadAll<BaseEnemy>("Enemies");
    }

    public BaseEnemy RandomEnemy()
    {
        return enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Length)];
    }

    public Level[] AllLevels()
    {
        return listOfLevels;
    }

    public Level CurrentLevel()
    {
        return listOfLevels[PrefManager.GetCurrentLevel()];
    }
}
