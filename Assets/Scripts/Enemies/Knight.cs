using UnityEngine;

public class Knight : BaseEnemy
{
    protected override void ShootBullet()
    {
        CreateBullet(DefaultAttack(this.transform.position, Vector3.left));
        CreateBullet(DefaultAttack(this.transform.position, Vector3.right));
        CreateBullet(DefaultAttack(this.transform.position, Vector3.up));
        CreateBullet(DefaultAttack(this.transform.position, Vector3.down));
    }
}
