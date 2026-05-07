using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MyBox;

public class Entity : MonoBehaviour
{
    [Foldout("Entity info", true)]
    public SpriteRenderer spriteRenderer;
    [SerializeField] protected int damage = 1;
    public int currentHealth;
    protected int maxHealth { get; private set; }
    protected int firedBullets;
    public bool immune {get; protected set; }
    [SerializeField] protected float bulletSpeed;
    protected int tookDamage;
    protected Bullet bulletPrefab { get; private set; }
    protected Queue<Bullet> bulletQueue = new();
    protected int landedBullets { get; private set; }

    protected virtual void Awake()
    {
        try
        {
            bulletPrefab = this.transform.Find("Bullet").GetComponent<Bullet>();
            bulletPrefab.gameObject.SetActive(false);
        } catch { }
        maxHealth = currentHealth;
    }
    public void ChangeHealth(int change)
    {
        if (immune && change < 0) return;
        currentHealth = Mathf.Clamp(currentHealth + change, 0, maxHealth);
        if (change > 0)
        {
            if (change > 0)
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
    }
    protected virtual void DamageEffect(int amount)
    {
    }
    protected virtual void HealEffect(int amount)
    {
    }
    protected Bullet CreateBullet(Bullet prefab, AttackInfo info)
    {
        firedBullets++;
        Bullet newBullet = (bulletQueue.Count > 0) ? bulletQueue.Dequeue() : Instantiate(prefab);
        newBullet.AssignInfo(info, this);
        return newBullet;
    }
    public void ReturnBullet(Bullet bullet, bool landed)
    {
        bulletQueue.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
        if (landed)
            landedBullets++;
    }
}
public class AttackInfo
{
    public Vector2 spawnPosition{get; private set;}
    public float bulletSpeed{get; private set;}
    public Vector2 direction{get; private set;}
    public int damage {get; private set;}

    public AttackInfo(Vector2 spawnposition, float bulletSpeed, Vector2 direction, int damage)
    {
        this.spawnPosition = spawnposition;
        this.bulletSpeed = bulletSpeed;
        this.direction = direction;
        this.damage = damage;
    }
}