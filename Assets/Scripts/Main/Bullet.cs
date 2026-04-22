using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Vector3 direction;
    public Entity owner { get; private set; }
    [SerializeField] int damage = 1;
    float bulletSpeed;
    public SpriteRenderer spriteRenderer { get; private set; }
    [SerializeField] bool disappearOnWall = true;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void TryAndReturn(bool landed)
    {
        if (owner == null)
            Destroy(this.gameObject);
        else
            owner.ReturnBullet(this, landed);
    }

    public virtual void AssignInfo(AttackInfo info, Entity owner)
    {
        this.transform.position = info.spawnPosition;
        this.tag = owner.tag;
        this.bulletSpeed = info.bulletSpeed;
        this.direction = info.direction;
        this.owner = owner;
        Movement();
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        Movement();
        if (disappearOnWall && (this.transform.position.x < WaveManager.minX - 0.5f || this.transform.position.x > WaveManager.maxX + 0.5f ||
            this.transform.position.y < WaveManager.minY - 0.5f || this.transform.position.y > WaveManager.maxY + 0.5f))
            TryAndReturn(false);
    }

    protected virtual void Movement()
    {
        this.transform.Translate(bulletSpeed * Time.deltaTime * direction, Space.World);
        this.transform.localEulerAngles = new(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entity target) && !target.immune && !target.CompareTag(this.tag))
        {
            HitEntity(target);
        }
        else if (disappearOnWall && collision.CompareTag("Wall") && !collision.transform.parent.CompareTag(this.tag))
        {
            TryAndReturn(false);
        }
    }
    protected virtual void HitEntity(Entity target)
    {
        target.TakeDamage(damage);
        TryAndReturn(true);        
    }
}