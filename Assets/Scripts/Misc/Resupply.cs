using UnityEngine;

public class Resupply : Bullet
{
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entity target) && info.canHit(target, this))
        {
            info.hitTarget(target);
            ForceReturn(true);
        }
    }
    protected override void Movement()
    {
        this.transform.Translate(info.bulletSpeed * Time.deltaTime * info.direction, Space.World);
    }
}
