using System.Collections.Generic;
using UnityEngine;

public class Flags : RulesManager
{
    int currentFlag;
    [SerializeField] float interval;
    List<HealthPack> flagsInOrder = new();
    public override void BeginGame()
    {
        RemoveFlags();
    }
    void RemoveFlags()
    {
        for (int i = flagsInOrder.Count-1; i>=0; i--)
        {
            ReturnResupply(flagsInOrder[i]);
            flagsInOrder.RemoveAt(i);
        }
        Invoke(nameof(NewFlags), interval);
    }
    void NewFlags()
    {
        currentFlag = 0;
        List<int> locations = new List<int>() {-6, -3, 0, 3, 6};
        for (int i = 0; i<5; i++)
        {
            int randomSpot = Random.Range(0, locations.Count);
            HealthPack nextFlag = MakeResupply(new(locations[randomSpot], Random.Range(WaveManager.minY+1f, 0)), $"{i+1}", Vector3.zero);
            flagsInOrder.Add(nextFlag);
            locations.RemoveAt(randomSpot);
        }        
    }
    public override void HitResupply(HealthPack resupply, bool needEnergy)
    {
        if (resupply == flagsInOrder[currentFlag])
        {
            ReturnResupply(resupply);
            currentFlag++;
            if (currentFlag == flagsInOrder.Count)
            {
                RemoveFlags();
                Player.instance.ChangeHealth(health);
            }
        }
        else
        {
            RemoveFlags();
        }
    }
}
