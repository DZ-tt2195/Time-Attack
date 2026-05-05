using UnityEngine;

public class Basics : RulesManager
{
    [SerializeField] float interval;
    [SerializeField] Vector2 movement;
    public override void BeginGame()
    {
        InvokeRepeating(nameof(SpawnResupply), 1f, interval);        
    }
    void SpawnResupply()
    {
        MakeResupply(new(Random.Range(WaveManager.minX + 1f, WaveManager.maxX - 1f), WaveManager.maxY), 
        AutoTranslate.Health_Pack(health.ToString()));
    }
    void Update()
    {
        for (int i = 0; i<activeResupplies.Count; i++)
        {
            Resupply resupply = activeResupplies[i];
            resupply.transform.Translate(movement*Time.deltaTime);
            if (resupply.transform.position.y < WaveManager.minY)
                ReturnResupply(resupply);
        }
    }
}
