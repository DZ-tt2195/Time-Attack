using UnityEngine;

public class Bloodlust : Rule
{
    bool killed;
    [SerializeField] int health;
    protected override void Awake()
    {
        base.Awake();
        EventManager.inst.Subscribe<EventDeath>(DeadEnemy);

        void DeadEnemy(EventDeath info)
        {
            if (info.deadEntity is BaseEnemy)
                killed = true;
        }
    }
    public override string MyText => AutoTranslate.Bloodlust_Text(GetTime.ToString(), health.ToString());
    protected override void ActivateRule()
    {
        if (killed)
            Player.instance.ChangeHealth(health);
        else
            Player.instance.ChangeHealth(-health);
        killed = false;
    }
}