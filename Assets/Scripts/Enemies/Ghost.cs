using UnityEngine;
using MyBox;

public class Ghost : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
        moveDirection = Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right;
    }

    protected override void ShootBullet()
    {
        base.ShootBullet();
        MyExtensions.SetAlpha(spriteRenderer, 1f);
    }

    protected override void DamageEffect(int change)
    {
        base.DamageEffect(change);
        MyExtensions.SetAlpha(spriteRenderer, 1f);
        healthText.SetAlpha(1f);
    }
    protected override void Movement()
    {
        base.Movement();
        if (this.transform.position.x < WaveManager.minX)
            moveDirection = Vector3.right;
        else if (transform.position.x > WaveManager.maxX)
            moveDirection = Vector3.left;
        
        if (currentHealth > 0)
        {
            float alpha = Mathf.Max(0, spriteRenderer.color.a - ((4.75f-attackRate) * Time.deltaTime));
            MyExtensions.SetAlpha(spriteRenderer, alpha);
            healthText.SetAlpha(alpha);
        }
    }
}
