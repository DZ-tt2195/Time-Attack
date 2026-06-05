using UnityEngine;

public class Sun : BaseEnemy
{
    Vector3 moveDirection;
    protected override void Awake()
    {
        base.Awake();
        moveDirection = Random.Range(0, 2) == 0 ? Vector3.down : Vector3.up;
    }

    protected override void Movement()
    {
        this.transform.Translate(moveSpeed * Time.deltaTime * moveDirection); 
        if (this.transform.position.y < WaveManager.minY)
            moveDirection = Vector3.up;
        else if (transform.position.y > WaveManager.maxY)
            moveDirection = Vector3.down;
    }

    protected override void ShootBullet()
    {
        for (int i = -1; i<=1; i+=2)
        {
            for (int j = -1; j<=1; j++)
                CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, new(i, j), damage));
        }
    }
}
