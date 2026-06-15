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
            star.transform.position = this.transform.position;
            star.AssignInfo(new AttackInfo(this.transform.position, bulletSpeed, new(target.x + RandomOffSet(), target.y + RandomOffSet()), Hit), this);
            
            void Hit(Entity entity)
            {
                this.ChangeHealth(-damage);
                Debug.Log("hit star");
            }
        }

        float RandomOffSet()
        {
            return Random.Range(-bulletOffset, bulletOffset);
        }
    }

    protected override void DeathEffect()
    {
        base.DeathEffect();
        wall.SetActive(false);
    }
}
