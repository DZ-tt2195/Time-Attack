using UnityEngine;

public class Basics : RulesManager
{
    [SerializeField] float interval;
    public override void BeginGame()
    {
        InvokeRepeating(nameof(SpawnResupply), 1f, interval);        
    }
    void SpawnResupply()
    {
        MakeResupply(new(Random.Range(WaveManager.minX + 1f, WaveManager.maxX - 1f), WaveManager.maxY), 
        AutoTranslate.Health_Pack(health.ToString()), new(0, -1.75f));
    }
}
