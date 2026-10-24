using UnityEngine;
using MyBox;

public class Ghost : BaseEnemy
{
    protected override void ShootBullet()
    {
        base.ShootBullet();
        MyExtensions.SetAlpha(GetSpriteRenderer, 1f);
    }

    protected override void DamageEffect(int change)
    {
        base.DamageEffect(change);
        MyExtensions.SetAlpha(GetSpriteRenderer, 1f);
        healthText.SetAlpha(1f);
    }
    protected override void EveryFrame()
    {
        base.EveryFrame();
        if (currentHealth > 0)
        {
            float alpha = Mathf.Max(0, GetSpriteRenderer.color.a - ((4.75f-attackRate) * Time.deltaTime));
            MyExtensions.SetAlpha(GetSpriteRenderer, alpha);
            healthText.SetAlpha(alpha);
        }
    }
}
