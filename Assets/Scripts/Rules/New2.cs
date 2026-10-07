using UnityEngine;

public class New2 : Rule
{
    bool faster;
    protected override void Awake()
    {
        base.Awake();
        EventManager.inst.Subscribe<ChangeBulletSpeed, float>(ChangeSpeed);

        float ChangeSpeed(ChangeBulletSpeed info)
        {
            if (info.entity is BaseEnemy)
            {
                if (faster)
                    return info.currentSpeed;
                else
                    return -0.5f*info.currentSpeed;
            }
            else
            {
                return 0f;
            }
        }
    }
    protected override void ActivateRule()
    {
        faster = !faster;
    }
}