using UnityEngine;

public class Fluctuate : Rule
{
    bool faster;
    [SerializeField] float percent;
    public override string MyText => AutoTranslate.Fluctuate_Text(GetTime.ToString(), (percent*100).ToString());
    protected override void Awake()
    {
        base.Awake();
        EventManager.inst.Subscribe<ChangeBulletSpeed, float>(ChangeSpeed);

        float ChangeSpeed(ChangeBulletSpeed info)
        {
            if (info.entity is BaseEnemy)
            {
                if (faster)
                {
                    return info.currentSpeed*percent;
                }
                else
                {
                    return info.currentSpeed*-percent;
                }
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