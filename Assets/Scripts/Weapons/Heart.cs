using System.Collections;
using UnityEngine;

public class Heart : Player
{
    [SerializeField] int extraDamage;
    protected override void FireWeapon()
    {
        int myDamage = this.currentHealth <= 4 ? extraDamage : damage;
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, Vector3.up, myDamage));
    }
}
