using UnityEngine;
using System.Collections.Generic;

public class GameFiles : MonoBehaviour
{
    [SerializeField] List<Level> listOfLevels;
    [SerializeField] List<BaseEnemy> enemiesToSpawn;
    [SerializeField] List<Rule> listOfRules;
    Dictionary<string, BaseEnemy> enemyDictionary = new Dictionary<string, BaseEnemy>();
    public static GameFiles inst;

    void Awake()
    {
        inst = this;
        foreach (BaseEnemy enemy in enemiesToSpawn)
            enemyDictionary.Add(enemy.name, enemy);
    }
    public BaseEnemy RandomEnemy() => enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
    public BaseEnemy GetEnemy(string enemy) => enemyDictionary[enemy];
    public List<Level> AllLevels() => listOfLevels;
    public Level CurrentLevel()
    {
        return listOfLevels[PrefManager.GetCurrentLevel()];
    }
    public List<Rule> AllRules() => listOfRules;    
    public HashSet<Rule> SavedRules()
    {
        HashSet<Rule> toReturn = new();
        for (int i = 0; i<NewCustomizer.numTwists; i++)
        {
            int savedNumber = PrefManager.GetRule(i);
            if (savedNumber != -1)
                toReturn.Add(listOfRules[savedNumber]);
        }
        while (toReturn.Count < NewCustomizer.numTwists)
        {
            int randomNumber = Random.Range(0, listOfRules.Count);
            toReturn.Add(listOfRules[randomNumber]);                
        }
        return toReturn;
    }
}