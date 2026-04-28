using System.Collections.Generic;
using UnityEngine;

public class Flags : EnergyManager
{
    int currentFlag;
    List<Resupply> flagsInOrder = new();
    public override void BeginGame()
    {
        CreateFlags();
    }
    void CreateFlags()
    {
        currentFlag = 0;
        for (int i = flagsInOrder.Count-1; i>=0; i--)
        {
            ReturnResupply(flagsInOrder[i]);
            flagsInOrder.RemoveAt(i);
        }
        List<int> locations = new List<int>() {-6, -3, 0, 3, 6};
        for (int i = 0; i<5; i++)
        {
            int randomSpot = Random.Range(0, locations.Count);
            Resupply nextFlag = MakeResupply(new(locations[randomSpot], Random.Range(WaveManager.minY+1f, 0)), $"{i+1}", Vector3.zero);
            flagsInOrder.Add(nextFlag);
            locations.RemoveAt(randomSpot);
        }
    }
    public override void HitResupply(Resupply resupply, bool needEnergy)
    {
        if (resupply == flagsInOrder[currentFlag])
        {
            ReturnResupply(resupply);
            currentFlag++;
            if (currentFlag == flagsInOrder.Count)
            {
                CreateFlags();
                Player.instance.ChangeEnergy(energy);
            }
        }
        else
        {
            CreateFlags();
        }
    }
}
