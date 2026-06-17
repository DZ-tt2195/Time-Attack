using MyBox;
using UnityEngine;
using TMPro;
public class BaseEnemy : Entity
{
    [Foldout("Enemy info", true)]
    protected GameObject crossedOut { get; private set; }
    [SerializeField] bool lookAtPlayer = true;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float attackRate;
    Vector2 aimPosition;
    float stunTime = 0f;
    Transform tracker;
    public void EnemySetup()
    {
        this.tag = "Enemy";
        crossedOut = transform.Find("X").gameObject;
        crossedOut.SetActive(false);
        healthText.text = currentHealth.ToString();

        tracker = transform.Find("Tracker");
        tracker.gameObject.SetActive(Application.isEditor);
        tracker.SetParent(null);

        attackRate *= 2-PrefManager.GetDifficulty();
        bulletSpeed *= PrefManager.GetDifficulty();
        moveSpeed *= PrefManager.GetDifficulty();

        InvokeRepeating(nameof(NewOffset), 0f, 1f);
        if (bulletPrefab != null && attackRate != 0f)
            InvokeRepeating(nameof(TryToShoot), attackRate*0.5f, attackRate);
    }
    void NewOffset()
    {
        aimPosition = Player.instance.transform.position + (Vector3)(Random.insideUnitCircle.normalized * 3f);
        tracker.position = aimPosition;
    }
    void TryToShoot()
    {
        if (stunTime <= 0f)
            ShootBullet();
    }
    protected virtual void ShootBullet()
    {
        Vector2 target = AimAtPlayer();
        target.Normalize();
        CreateBullet(DefaultAttack(this.transform.position, target));
    }
    void Update()
    {
        if (stunTime > 0f) 
        {
            stunTime -= Time.deltaTime;
        }
        else
        {
            Movement();
            RotateToPlayer();
        }
    }
    protected virtual void Movement()
    {
        //this.transform.Translate(moveSpeed * Time.deltaTime * moveDirection); 
        transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, moveSpeed*Time.deltaTime);    
    }
    protected virtual void RotateToPlayer()
    {
        if (lookAtPlayer)
        {
            Vector2 aimDirection = AimAtPlayer();
            spriteRenderer.transform.localEulerAngles = new(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + 90);
        }        
    }
    protected Vector2 AimAtPlayer()
    {
        return (Player.instance.transform.position - this.transform.position).normalized;
    }
    protected override void DeathEffect()
    {
        base.DeathEffect();
        crossedOut.SetActive(true);
        MyExtensions.SetAlpha(this.spriteRenderer, 0.5f);
    }
    protected override void HealEffect(int amount)
    {
        base.HealEffect(amount);
        crossedOut.SetActive(false);
        MyExtensions.SetAlpha(this.spriteRenderer, 1f);
    }
    public virtual void OnDestroy()
    {
        while (bulletQueue.Count > 0)
        {
            Bullet nextBullet = bulletQueue.Dequeue();
            if (nextBullet != null)
                Destroy(nextBullet.gameObject);
        }
    }
    public void StunThis(float stunTime)
    {
        this.stunTime+=stunTime;
    }
}