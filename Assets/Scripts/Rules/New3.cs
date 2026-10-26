using System.Collections.Generic;
using UnityEngine;

public class New3 : Rule
{
    protected override void ActivateRule()
    {
        List<BaseEnemy> canHit = new();
        foreach (BaseEnemy enemy in WaveManager.instance.GetEnemies())
        {
            if (enemy.CanTakeDamage())
                canHit.Add(enemy);
        }
        BaseEnemy randomEnemy = canHit[Random.Range(0, canHit.Count)];
        randomEnemy.ChangeHealth(-1*randomEnemy.GetHealth());
        WaveManager.instance.CreateEnemy(Vector2.zero, randomEnemy);
    }
}