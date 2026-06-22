using UnityEngine;
using System;
public class Bullet : MonoBehaviour
{
    protected AttackInfo info;
    public StoreBullets owner { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void AssignInfo(AttackInfo info, StoreBullets owner)
    {
        this.transform.position = info.spawnPosition;
        this.tag = owner.tag;
        this.owner = owner;
        this.info = info;
        Movement();
        this.gameObject.SetActive(true);
    }
    void Update()
    {
        Movement();
        if (this.transform.position.x < WaveManager.minX - 0.5f || this.transform.position.x > WaveManager.maxX + 0.5f ||
            this.transform.position.y < WaveManager.minY - 0.5f || this.transform.position.y > WaveManager.maxY + 0.5f)
        {
            ForceReturn();
        }
    }
    protected virtual void Movement()
    {
        this.transform.Translate(info.bulletSpeed * Time.deltaTime * info.direction, Space.World);
        this.transform.localEulerAngles = new(0, 0, Mathf.Atan2(info.direction.y, info.direction.x) * Mathf.Rad2Deg + 90);
    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entity target) && info.canHit(target, this))
        {
            info.hitTarget(target);
            info.returnBullet(this, true);
        }
        else if (collision.CompareTag("Wall") && !collision.transform.parent.CompareTag(this.tag))
        {
            if (this.CompareTag("Player")) AudioManager.instance.Miss(0.1f);
            ForceReturn();
        }
    }
    public void ForceReturn()
    {
        info.returnBullet(this, false);        
    }
}