using UnityEngine;
using System.Collections.Generic;
using MyBox;

public class Flag : Rule
{
    [SerializeField] SpriteRenderer flagSprite;
    [SerializeField] int energyCost;
    [SerializeField] int healthGain;
    protected override void Awake()
    {
        base.Awake();
        MoveFlag();
    }
    protected override bool CanUse()
    {
        if (base.CanUse())
        {
            if (flagSprite.color.a < 1f)
            {
                MyExtensions.SetAlpha(flagSprite, 1);
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
        MoveFlag();
    }
    void MoveFlag()
    {
        flagSprite.transform.position = new Vector2(WaveManager.RandomX(1f), WaveManager.RandomY(1f));
        MyExtensions.SetAlpha(flagSprite, 0.5f);
    }
}
