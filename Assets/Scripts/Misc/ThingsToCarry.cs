using UnityEngine;
using System.Collections.Generic;

public class ThingsToCarry : MonoBehaviour
{
    [SerializeField] List<Level> listOfLevels;
    [SerializeField] List<BaseEnemy> enemiesToSpawn;
    [SerializeField] List<SubWeapon> listOfSubs;
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
    public List<SubWeapon> AllSubs() => listOfSubs;    
    public SubWeapon RandomSub()
    {
        if (PrefManager.GetCurrentSub() == -1)
            return listOfSubs[UnityEngine.Random.Range(0, listOfSubs.Count)];
        else
            return listOfSubs[PrefManager.GetCurrentSub()];
    }
}
