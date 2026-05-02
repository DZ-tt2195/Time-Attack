using UnityEngine;
using System.Collections.Generic;

public class ThingsToCarry : MonoBehaviour
{
    [SerializeField] List<Level> listOfLevels;
    [SerializeField] List<BaseEnemy> enemiesToSpawn;
    [SerializeField] List<Player> listOfPlayers;
    [SerializeField] List<RulesManager> listOfRules;
    public static ThingsToCarry inst;

    void Awake()
    {
        inst = this;
    }

    public BaseEnemy RandomEnemy()
    {
        return enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Count)];
    }

    public List<Level> AllLevels() => listOfLevels;
    public Level CurrentLevel()
    {
        return listOfLevels[PrefManager.GetCurrentLevel()];
    }

    public List<Player> AllWeapons() => listOfPlayers;

    public Player RandomWeapon()
    {
        if (PrefManager.GetCurrentWeapon() == -1)
            return listOfPlayers[UnityEngine.Random.Range(0, listOfPlayers.Count)];
        else
            return listOfPlayers[PrefManager.GetCurrentWeapon()];
    }

    public List<RulesManager> AllRules() => listOfRules;
    
    public RulesManager RandomRule()
    {
        if (PrefManager.GetCurrentRule() == -1)
            return listOfRules[UnityEngine.Random.Range(0, listOfRules.Count)];
        else
            return listOfRules[PrefManager.GetCurrentRule()];
    }
}
