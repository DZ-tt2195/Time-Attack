using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected AttackInfo info;
    protected Vector3 direction;
    public Entity owner { get; private set; }
    float bulletSpeed;
    public SpriteRenderer spriteRenderer { get; private set; }
    [SerializeField] bool disappearOnWall = true;
    int damage;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void TryAndReturn(bool landed)
    {
        if (owner == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            owner.ReturnBullet(this, landed);
            if (owner == Player.instance && !landed)
                AudioManager.instance.Miss(0.3f);
        }
    }
    public virtual void AssignInfo(AttackInfo info, Entity owner)
    {
        this.transform.position = info.spawnPosition;
        this.tag = owner.tag;
        this.bulletSpeed = info.bulletSpeed;
        this.direction = info.direction;
        this.owner = owner;
        this.damage = info.damage;
        Movement();
        this.gameObject.SetActive(true);
    }
    void Update()
    {
        Movement();
        if (disappearOnWall && (this.transform.position.x < WaveManager.minX - 0.5f || this.transform.position.x > WaveManager.maxX + 0.5f ||
            this.transform.position.y < WaveManager.minY - 0.5f || this.transform.position.y > WaveManager.maxY + 0.5f))
        {
            Exited();
        }
    }
    protected virtual void Exited()
    {
        TryAndReturn(false);        
    }
    protected virtual void Movement()
    {
        this.transform.Translate(bulletSpeed * Time.deltaTime * direction, Space.World);
        this.transform.localEulerAngles = new(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);
    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entity target) && target.CanTakeDamage() && !target.CompareTag(this.tag))
        {
            HitEntity(target);
        }
        else if (disappearOnWall && collision.CompareTag("Wall") && !collision.transform.parent.CompareTag(this.tag))
        {
            if (this.CompareTag("Player")) AudioManager.instance.Miss(0.1f);
            TryAndReturn(false);
        }
    }
    protected virtual void HitEntity(Entity target)
    {
        target.ChangeHealth(-damage);
        TryAndReturn(true);        
    }
}