using UnityEngine;

public class Freeze : Rule
{
    [SerializeField] float stunTime;
    protected override void ActivateRule()
    {
        foreach (BaseEnemy enemy in enemiesInRange)
            enemy.StunThis(stunTime);
    }
}
