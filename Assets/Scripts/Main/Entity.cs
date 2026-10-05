using UnityEngine;
using System.Collections.Generic;
using MyBox;
using TMPro;
public enum Protection {Barrier, Immunity, Dead}

public class Entity : StoreBullets
{
    [Foldout("Entity info", true)]
    public SpriteRenderer spriteRenderer;
    protected int currentHealth {get; private set;}
    [SerializeField] protected int maxHealth;
    protected List<Protection> protectionSources = new();
    protected int tookDamage;
    [SerializeField] protected TMP_Text healthText;
    protected float stunTime {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        stunTime = 0f;
        currentHealth = maxHealth;
        healthText.text = maxHealth.ToString();
    }
    public bool CanTakeDamage() => protectionSources.Count == 0;
    public void ChangeHealth(int change)
    {
        if (!CanTakeDamage() && change < 0) return;
        currentHealth = Mathf.Max(currentHealth + change, 0);
        healthText.text = currentHealth.ToString();

        if (change > 0)
        {
            AudioManager.instance.Heal(0.3f);
            HealEffect(change); 
            EventManager.inst.RunTriggers(new EventHeal(this, change));
        }
        else
        {
            tookDamage-=change;
            AudioManager.instance.Damage(this is Player ? 0.5f : 0.3f);

            if (currentHealth == 0)
            {
                DeathEffect();
                EventManager.inst.RunTriggers(new EventDeath(this));
            }
            else
            {
                DamageEffect(change);
            }
        }   
    }
    protected virtual void DeathEffect()
    {
        protectionSources.Add(Protection.Dead);
        healthText.text = "";
    }
    protected virtual void DamageEffect(int amount)
    {
    }
    protected virtual void HealEffect(int amount)
    {
        protectionSources.Remove(Protection.Dead);
    }
    public int GetHealth() => currentHealth;
    public void StunThis(float stunTime)
    {
        this.stunTime+=stunTime;
    }
    void Update()
    {
        stunTime = Mathf.Max(stunTime-Time.deltaTime, 0);
        if (stunTime == 0f && WaveManager.state == GameState.Playing) 
            EveryFrame();
    }
    protected virtual void EveryFrame()
    {
    }
}
public class EventDeath : CustomEvent
{
    public Entity deadEntity { get; private set; }

    public EventDeath(Entity deadEntity)
    {
        this.deadEntity = deadEntity;
    }
}
public class EventHeal : CustomEvent
{
    public Entity healedEntity { get; private set; }
    public int healAmount {get; private set;}

    public EventHeal(Entity healedEntity, int healAmount)
    {
        this.healedEntity = healedEntity;
        this.healAmount = healAmount;
    }
}