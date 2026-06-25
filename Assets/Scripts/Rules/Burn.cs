using UnityEngine;

public class Burn : Rule
{
    [SerializeField] AudioClip burn;
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(burn, 0.5f);
        bool hitEnemy = false;
        foreach (Entity entity in entitiesInRange)
        {
            if (entity == Player.instance)
                continue;

            if (entity.CanTakeDamage())
            {
                hitEnemy = true;
                entity.ChangeHealth(-1);
            }
        }
        if (!hitEnemy)
            Player.instance.ChangeHealth(-1);
    }
}
