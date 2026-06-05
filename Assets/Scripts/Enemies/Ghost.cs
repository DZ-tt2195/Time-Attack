using UnityEngine;
using MyBox;

public class Ghost : BaseEnemy
{
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
        if (currentHealth > 0)
        {
            float alpha = Mathf.Max(0, spriteRenderer.color.a - ((4.75f-attackRate) * Time.deltaTime));
            MyExtensions.SetAlpha(spriteRenderer, alpha);
            healthText.SetAlpha(alpha);
        }
    }
}
