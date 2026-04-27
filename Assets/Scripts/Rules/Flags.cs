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
        for (int i = 0; i<5; i++)
        {
            Resupply nextFlag = MakeResupply(new(Random.Range(WaveManager.minX+1f, WaveManager.maxX-1f), Random.Range(WaveManager.minY+1f, 0)),
            $"{i+1}", Vector3.zero);
            flagsInOrder.Add(nextFlag);
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
