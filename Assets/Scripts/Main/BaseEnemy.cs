using MyBox;
using UnityEngine;
using TMPro;
public class BaseEnemy : Entity
{
    [Foldout("Enemy info", true)]
    protected GameObject crossedOut { get; private set; }
    [SerializeField] bool lookAtPlayer = true;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackRate;
    protected TMP_Text healthText;
    Vector3 targetOffSet;
    public void EnemySetup()
    {
        this.tag = "Enemy";
        crossedOut = transform.Find("X").gameObject;
        crossedOut.SetActive(false);
        healthText = transform.Find("Health Text").GetComponent<TMP_Text>();
        healthText.text = currentHealth.ToString();

        attackRate *= 2-PrefManager.GetDifficulty();
        bulletSpeed *= PrefManager.GetDifficulty();
        moveSpeed *= PrefManager.GetDifficulty();

        InvokeRepeating(nameof(NewOffset), 0f, 3f);
        if (bulletPrefab != null && attackRate != 0f)
            InvokeRepeating(nameof(ShootBullet), attackRate*0.5f, attackRate);
    }

    void NewOffset()
    {
        targetOffSet = Random.insideUnitCircle.normalized * 5f;
    }

    protected virtual void ShootBullet()
    {
        Vector2 target = AimAtPlayer();
        target.Normalize();
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, target, damage));
    }

    void Update()
    {
        Movement();
        RotateToPlayer();
    }

    protected virtual void Movement()
    {
        //this.transform.Translate(moveSpeed * Time.deltaTime * moveDirection); 
        transform.position = Vector3.MoveTowards(transform.position, Player.instance.transform.position + targetOffSet, moveSpeed*Time.deltaTime);    
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

    protected override void DamageEffect(int change)
    {
        healthText.text = currentHealth.ToString();
    }

    protected override void DeathEffect()
    {
        immune = true;
        crossedOut.SetActive(true);
        MyExtensions.SetAlpha(this.spriteRenderer, 0.5f);
        healthText.text = "";
    }
    protected override void HealEffect(int amount)
    {
        base.HealEffect(amount);
        crossedOut.SetActive(false);
        immune = false;
        healthText.text = currentHealth.ToString();
        MyExtensions.SetAlpha(this.spriteRenderer, 1f);
    }

    public void OnDestroy()
    {
        while (bulletQueue.Count > 0)
        {
            Bullet nextBullet = bulletQueue.Dequeue();
            if (nextBullet != null)
                Destroy(nextBullet.gameObject);
        }
    }
}