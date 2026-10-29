using UnityEngine;

public class Fluctuate : Rule
{
    [SerializeField] AudioClip fasterSound;
    [SerializeField] AudioClip slowerSound;
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
        if (faster)
            AudioManager.instance.PlaySound(fasterSound);
        else
            AudioManager.instance.PlaySound(slowerSound);
    }
}