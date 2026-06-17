using UnityEngine;

public class Burn : Rule
{
    protected override void ActivateRule()
    {
        bool hitEnemy = false;
        foreach (BaseEnemy enemy in enemiesInRange)
        {
            if (enemy.CanTakeDamage())
            {
                hitEnemy = true;
                enemy.ChangeHealth(-1);
            }
        }
        if (!hitEnemy)
            Player.instance.ChangeHealth(-1);
    }
}
