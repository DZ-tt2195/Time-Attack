using UnityEngine;
using System.Collections.Generic;
using MyBox;

public class Heart : Rule
{
    [SerializeField] SpriteRenderer heartSprite;
    [SerializeField] int energyCost;
    [SerializeField] int healthGain;
    protected override void Awake()
    {
        base.Awake();
        MoveHeart();
    }
    public override string MyText => AutoTranslate.Heart_Text(GetTime.ToString(), energyCost.ToString(), healthGain.ToString());
    protected override bool CanUse()
    {
        if (base.CanUse())
        {
            if (heartSprite.color.a < 1f)
            {
                MyExtensions.SetAlpha(heartSprite, 1);
                AudioManager.instance.Menu();
            }
            return entitiesInRange.Contains(Player.instance) && Player.instance.EnergyInfo().currentEnergy >= energyCost;
        }
        else
        {
            return false;
        }
    }
    protected override void ActivateRule()
    {
        Player.instance.ChangeEnergy(-energyCost);
        Player.instance.ChangeHealth(healthGain);
        MoveHeart();
    }
    void MoveHeart()
    {
        heartSprite.transform.position = new Vector2(WaveManager.RandomX(1f), WaveManager.RandomY(1f));
        MyExtensions.SetAlpha(heartSprite, 0.5f);
    }
}