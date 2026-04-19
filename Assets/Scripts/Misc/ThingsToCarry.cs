using UnityEngine;
using System.Collections.Generic;

public class ThingsToCarry : MonoBehaviour
{
    [SerializeField] List<Level> listOfLevels;
    [SerializeField] List<BaseEnemy> enemiesToSpawn;
    [SerializeField] List<Player> listOfPlayers;
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

    public Player RandomPlayer()
    {
        if (PrefManager.GetCurrentPlayer() == -1)
            return listOfPlayers[UnityEngine.Random.Range(0, listOfPlayers.Count)];
        else
            return listOfPlayers[PrefManager.GetCurrentPlayer()];
    }

    public Level CurrentLevel()
    {
        return listOfLevels[PrefManager.GetCurrentLevel()];
    }
}
