using MyBox;
using UnityEngine;
using TMPro;
public class BaseEnemy : Entity
{
    enum AimBehavior {ToPlayer, Random};
    [Foldout("Enemy info", true)]
    protected GameObject crossedOut { get; private set; }
    [SerializeField] bool lookAtPlayer = true;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float attackRate;
    [SerializeField] AimBehavior myBehavior;
    Vector2 aimPosition;
    Transform tracker;
    public void EnemySetup()
    {
        this.tag = "Enemy";
        crossedOut = transform.Find("X").gameObject;
        crossedOut.SetActive(false);
        healthText.text = currentHealth.ToString();

        tracker = transform.Find("Tracker");
        //tracker.gameObject.SetActive(Application.isEditor);
        tracker.gameObject.SetActive(false);
        tracker.SetParent(null);

        attackRate *= 2-PrefManager.GetDifficulty();
        bulletSpeed *= PrefManager.GetDifficulty();
        moveSpeed *= PrefManager.GetDifficulty();

        InvokeRepeating(nameof(NewOffset), 0f, myBehavior == AimBehavior.ToPlayer ? 1f : 3f);
        if (bulletPrefab != null && attackRate != 0f)
            InvokeRepeating(nameof(TryToShoot), attackRate*0.5f, attackRate);
    }
    void NewOffset()
    {
        if (myBehavior == AimBehavior.ToPlayer)
        {
            aimPosition = Player.instance.transform.position + (Vector3)(Random.insideUnitCircle.normalized * 3f);            
        }
        else if (myBehavior == AimBehavior.Random)
        {
            aimPosition = new Vector2(WaveManager.RandomX(1f), WaveManager.RandomY(1f));
        }
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
    protected override void EveryFrame()
    {
        if (lookAtPlayer)
        {
            Vector2 aimDirection = AimAtPlayer();
            spriteRenderer.transform.localEulerAngles = new(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + 90);
        }        
        transform.position = Vector3.MoveTowards(this.transform.position, aimPosition, moveSpeed*Time.deltaTime);  
        if (Vector3.Distance(transform.position, aimPosition) < 0.01f)
            NewOffset();
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
    public override void OnDestroy()
    {
        base.OnDestroy();
        Destroy(tracker.gameObject);
    }
}