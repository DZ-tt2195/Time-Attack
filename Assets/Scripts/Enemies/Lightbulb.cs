using TMPro;
using UnityEngine;

public class Lightbulb : BaseEnemy
{
    [SerializeField] float blackOutTime;

    protected override void Awake()
    {
        base.Awake();
        blackOutTime *= PrefManager.GetDifficulty();
    }

    protected override void DamageEffect(int change)
    {
        base.DamageEffect(change);
        WaveManager.instance.blackOutTime += this.blackOutTime*change;
    }

    protected override void DeathEffect()
    {
        base.DeathEffect();
        WaveManager.instance.blackOutTime += this.blackOutTime;
    }
}
