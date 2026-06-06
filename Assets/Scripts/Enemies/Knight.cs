using UnityEngine;

public class Knight : BaseEnemy
{
    protected override void ShootBullet()
    {
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, Vector3.left, damage));
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, Vector3.right, damage));
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, Vector3.up, damage));
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, Vector3.down, damage));
    }
}
