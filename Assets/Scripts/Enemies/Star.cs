using UnityEngine;

public class Star : Bullet
{
    protected override void TryAndReturn(bool landed)
    {
        this.gameObject.SetActive(false);
    }
    protected override void HitEntity(Entity target)
    {
        this.owner.ChangeHealth(-1);
        TryAndReturn(true);
    }
}