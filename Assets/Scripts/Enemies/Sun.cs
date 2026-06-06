using UnityEngine;

public class Sun : BaseEnemy
{
    protected override void ShootBullet()
    {
        for (int i = -1; i<=1; i+=2)
        {
            for (int j = -1; j<=1; j+=2)
                CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, new(i, j), damage));
        }
    }
}
