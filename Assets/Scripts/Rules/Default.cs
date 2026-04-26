using UnityEngine;

public class Default : EnergyManager
{
    [SerializeField] float interval;
    public override void BeginGame()
    {
        InvokeRepeating(nameof(SpawnResupply), 1f, interval);        
    }
}
