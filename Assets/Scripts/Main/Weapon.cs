using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public virtual (int energyCost, List<AttackInfo>) BulletAttack(Player player)
    {
        return (0, new());
    }
}