using UnityEngine;
using System.Collections.Generic;

public class ThingsToCarry : MonoBehaviour
{
    [SerializeField] List<Level> listOfLevels;
    [SerializeField] List<BaseEnemy> enemiesToSpawn;
    public static ThingsToCarry inst;

    void Awake()
    {
        inst = this;
    }

    public BaseEnemy RandomEnemy()
    {
        return enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Count)];
    }

    public List<Level> AllLevels()
    {
        return listOfLevels;
    }

    public Level CurrentLevel()
    {
        return listOfLevels[PrefManager.GetCurrentLevel()];
    }
}
