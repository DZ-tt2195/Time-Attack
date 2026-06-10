using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using TMPro;
using System;
public enum Protection {Barrier, Immunity, Dead}

public class Entity : StoreBullets
{
    [Foldout("Entity info", true)]
    public SpriteRenderer spriteRenderer;
    [SerializeField] protected int damage = 1;
    protected int currentHealth {get; private set;}
    [SerializeField] protected int maxHealth;
    [ReadOnly] public List<Protection> protectionSources = new();
    protected int tookDamage;
    [SerializeField] protected TMP_Text healthText;

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
        healthText.text = maxHealth.ToString();
    }
    public bool CanTakeDamage() => protectionSources.Count == 0;
    public void ChangeHealth(int change)
    {
        if (!CanTakeDamage() && change < 0) return;
        currentHealth = Mathf.Clamp(currentHealth + change, 0, maxHealth);
        healthText.text = currentHealth.ToString();

        if (change > 0)
        {
            AudioManager.instance.Heal(0.3f);
            HealEffect(change); 
        }
        else
        {
            tookDamage-=change;
            AudioManager.instance.Damage(this is Player ? 0.5f : 0.25f);

            if (currentHealth <= 0)
                DeathEffect();
            else
                DamageEffect(change);
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
    protected AttackInfo DefaultAttack(Vector2 spawn, Vector2 direction)
    {
        return new AttackInfo(spawn, bulletSpeed, direction, Hit);
        void Hit(Entity entity) => entity.ChangeHealth(-damage);        
    }
    protected Bullet CreateBullet(AttackInfo info)
    {
        firedBullets++;
        Bullet newBullet = (bulletQueue.Count > 0) ? bulletQueue.Dequeue() : Instantiate(bulletPrefab);
        newBullet.AssignInfo(info, this);
        return newBullet;
    }
    public int GetHealth() => currentHealth;
}
public class AttackInfo
{
    public Vector2 spawnPosition{get; private set;}
    public float bulletSpeed{get; private set;}
    public Vector2 direction{get; private set;}
    public Action<Entity> hitTarget {get; private set;}

    public AttackInfo(Vector2 spawnposition, float bulletSpeed, Vector2 direction, Action<Entity> hitTarget)
    {
        this.spawnPosition = spawnposition;
        this.bulletSpeed = bulletSpeed;
        this.direction = direction;
        this.hitTarget = hitTarget;
    }
}