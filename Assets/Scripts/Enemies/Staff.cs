using UnityEngine;

public class Staff : BaseEnemy
{
    [SerializeField] GameObject wall;
    [SerializeField] Bullet star;
    [SerializeField] float bulletOffset;
    [SerializeField] int numBullets;

    protected override void Awake()
    {
        base.Awake();
        star.gameObject.SetActive(false);
        star.transform.SetParent(null);
        star.tag = this.tag;
    }
    protected override void ShootBullet()
    {
        Vector2 target = AimAtPlayer();
        target.Normalize();

        for (int i = 0; i < numBullets; i++)
            CreateBullet(DefaultAttack(this.transform.position, new(target.x + RandomOffSet(), target.y + RandomOffSet())));

        if (currentHealth > 0 && !star.gameObject.activeSelf)
        {
            star.AssignInfo(new BulletInfo(this.transform.position, bulletSpeed, new(target.x + RandomOffSet(), target.y + RandomOffSet()), IsPlayer, Hit, Return), this);
            
            bool IsPlayer(Entity entity, Bullet bullet) => entity is Player;
            void Hit(Entity entity) => this.ChangeHealth(-1);
            void Return(Bullet bullet, bool landed) => bullet.gameObject.SetActive(false);
        }

        float RandomOffSet()
        {
            return Random.Range(-bulletOffset, bulletOffset);
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        Destroy(star.gameObject);
    }
}
