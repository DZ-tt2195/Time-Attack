using UnityEngine;

public class Burn : Rule
{
    [SerializeField] AudioClip burn;
    [SerializeField] int damage;
    public override string MyText => AutoTranslate.Burn_Text(GetTime.ToString(), damage.ToString());
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
                entity.ChangeHealth(-damage);
            }
        }
        if (!hitEnemy)
            Player.instance.ChangeHealth(-damage);
    }
}
