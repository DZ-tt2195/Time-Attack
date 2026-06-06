using UnityEngine;

public class Compass : Player
{
    protected override void FireWeapon()
    { 
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, new(0.4f, 1f), damage));
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, new(-0.4f, 1f), damage));
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, new(0.4f, -1f), damage));
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, new(-0.4f, -1f), damage));
    }
}
