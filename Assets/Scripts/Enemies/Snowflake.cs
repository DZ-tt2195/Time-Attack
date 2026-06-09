using UnityEngine;

public class Snowflake : BaseEnemy
{
    protected override void ShootBullet()
    {
        Vector2 target = AimAtPlayer();
        target.Normalize();

        for (float i = -7f; i <= 7f; i += 1.75f)
        {
            float transformX = this.transform.position.x + i;
            float transformY = (this.transform.position.y > 0) ? WaveManager.maxY : WaveManager.minY;
            CreateBullet(DefaultAttack(new Vector2(transformX, transformY), target));
        }
    }
}
