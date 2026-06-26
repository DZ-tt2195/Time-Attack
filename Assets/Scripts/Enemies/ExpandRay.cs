using UnityEngine;

public class ExpandRay : Bullet
{
    [SerializeField] float targetXpand;
    [SerializeField] float travelTime;
    float myTimer = 0;

    protected override void Awake()
    {
        base.Awake();
        travelTime *= 2 - PrefManager.GetDifficulty();
    }

    public override void AssignInfo(BulletInfo info, StoreBullets owner)
    {
        base.AssignInfo(info, owner);
        this.transform.localScale = Vector3.zero;
        myTimer = 0f;
    }

    protected override void Movement()
    {
        myTimer += Time.deltaTime;
        this.transform.localScale = Vector2.Lerp(Vector3.zero, new(targetXpand, targetXpand), myTimer / travelTime);
        if (myTimer >= travelTime)
            ForceReturn(false);
    }
}