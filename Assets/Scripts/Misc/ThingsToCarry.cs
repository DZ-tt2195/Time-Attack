using UnityEngine;
using System.Collections.Generic;

public class ThingsToCarry : MonoBehaviour
{
    [SerializeField] List<Level> listOfLevels;
    [SerializeField] List<BaseEnemy> enemiesToSpawn;
    [SerializeField] List<Player> listOfPlayers;
    [SerializeField] Resupply resupplyPrefab;
    public static ThingsToCarry inst;

    void Awake()
    {
        inst = this;
    }

    public Resupply GetResupply => resupplyPrefab;

    public BaseEnemy RandomEnemy()
    {
        return enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Count)];
    }

    public List<Level> AllLevels() => listOfLevels;

    public List<Player> AllWeapons() => listOfPlayers;

    public Player RandomWeapon()
    {
        if (PrefManager.GetCurrentWeapon() == -1)
            return listOfPlayers[UnityEngine.Random.Range(0, listOfPlayers.Count)];
        else
            return listOfPlayers[PrefManager.GetCurrentWeapon()];
    }

    public Level CurrentLevel()
    {
        return listOfLevels[PrefManager.GetCurrentLevel()];
    }
}
