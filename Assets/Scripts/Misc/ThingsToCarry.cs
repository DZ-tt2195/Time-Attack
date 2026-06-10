using UnityEngine;
using System.Collections.Generic;

public class ThingsToCarry : MonoBehaviour
{
    [SerializeField] List<Level> listOfLevels;
    [SerializeField] List<BaseEnemy> enemiesToSpawn;
    [SerializeField] List<Rule> listOfRules;
    public static ThingsToCarry inst;

    void Awake()
    {
        inst = this;
    }
    public BaseEnemy RandomEnemy()
    {
        return enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
    }
    public List<Level> AllLevels() => listOfLevels;
    public Level CurrentLevel()
    {
        return listOfLevels[PrefManager.GetCurrentLevel()];
    }
    public List<Rule> AllRules() => listOfRules;    
}
