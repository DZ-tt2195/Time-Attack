using UnityEngine;

public class Freeze : SubWeapon
{
    [SerializeField] float stunTime;
    public override void UseSubWeapon()
    {
        base.UseSubWeapon();
        foreach (BaseEnemy enemy in enemiesInRange)
            enemy.StunThis(stunTime);
    }
}
