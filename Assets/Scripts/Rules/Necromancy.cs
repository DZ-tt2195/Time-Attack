using UnityEngine;

public class Necromancy : Rule
{
    protected override void ActivateRule()
    {
        foreach (BaseEnemy enemy in WaveManager.instance.GetEnemies())
        {
            if (enemy.GetHealth() == 0)
                enemy.ChangeHealth(2);
        }
    }
}
