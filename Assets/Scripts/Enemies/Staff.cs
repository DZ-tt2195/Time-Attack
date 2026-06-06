using UnityEngine;

public class Staff : BaseEnemy
{
    GameObject wall;
    Star starPrefab;
    [SerializeField] float bulletOffset;
    [SerializeField] int numBullets;

    protected override void Awake()
    {
        base.Awake();
        wall = transform.Find("Wall").gameObject;

        starPrefab = transform.Find("Star").GetComponent<Star>();
        starPrefab.gameObject.SetActive(false);
        starPrefab.transform.SetParent(null);
        starPrefab.tag = this.tag;
    }

    protected override void ShootBullet()
    {
        Vector2 target = AimAtPlayer();
        target.Normalize();

        for (int i = 0; i < numBullets; i++)
            CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, new(target.x + RandomOffSet(), target.y + RandomOffSet()), damage));

        if (currentHealth > 0 && !starPrefab.gameObject.activeSelf)
        {
            starPrefab.transform.position = this.transform.position;
            starPrefab.AssignInfo(new AttackInfo(this.transform.position, bulletSpeed, new(target.x + RandomOffSet(), target.y + RandomOffSet()), damage), this);
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
