using UnityEngine;

public class Angled : Player
{
    protected override void FireWeapon()
    { 
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, new(0.4f, 1f)));
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, new(-0.4f, 1f)));
    }
}
