using UnityEngine;
using System;
using System.Collections.Generic;
using MyBox;

public class StoreBullets : MonoBehaviour
{
    [Foldout("Bullet info", true)]
    protected int firedBullets;
    [SerializeField] protected float bulletSpeed;
    protected Bullet bulletPrefab { get; private set; }
    protected Queue<Bullet> bulletQueue = new();
    protected HashSet<Bullet> activeBullets = new();
    protected int landedBullets { get; private set; }
    
    protected virtual void Awake()
    {
        try
        {
            bulletPrefab = this.transform.Find("Bullet").GetComponent<Bullet>();
            bulletPrefab.gameObject.SetActive(false);
        } catch { }
    }
    protected Bullet CreateBullet(AttackInfo info)
    {
        firedBullets++;
        Bullet newBullet = (bulletQueue.Count > 0) ? bulletQueue.Dequeue() : Instantiate(bulletPrefab);
        newBullet.AssignInfo(info, this);
        activeBullets.Add(newBullet);
        return newBullet;
    }
    protected void ReturnBullet(Bullet bullet, bool landed)
    {
        activeBullets.Remove(bullet);
        bulletQueue.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
        if (landed)
            landedBullets++;
    }
    protected AttackInfo DefaultAttack(Vector2 spawn, Vector2 direction)
    {
        return new AttackInfo(spawn, bulletSpeed, direction, CanHit, Hit, Return);
        bool CanHit(Entity entity, Bullet bullet) => !bullet.CompareTag(entity.tag) && entity.CanTakeDamage();
        void Hit(Entity entity) => entity.ChangeHealth(-1);
        void Return(Bullet bullet, bool landed)
        {
            if (this == null)
            {
                Destroy(bullet.gameObject);
            }
            else
            {
                this.ReturnBullet(bullet, landed);
                if (this == Player.instance && !landed)
                    AudioManager.instance.Miss(0.3f);                
            }
        } 
    }
    public virtual void OnDestroy()
    {
        while (bulletQueue.Count > 0)
        {
            Bullet nextBullet = bulletQueue.Dequeue();
            if (nextBullet != null)
                Destroy(nextBullet.gameObject);
        }
    }
}
public class AttackInfo
{
    public Vector2 spawnPosition{get; private set;}
    public float bulletSpeed{get; private set;}
    public Vector2 direction{get; private set;}
    public Func<Entity, Bullet, bool> canHit;
    public Action<Entity> hitTarget {get; private set;}
    public Action<Bullet, bool> returnBullet {get; private set;}

    public AttackInfo(Vector2 spawnposition, float bulletSpeed, Vector2 direction, Func<Entity, Bullet, bool> canHit, Action<Entity> hitTarget, Action<Bullet, bool> returnBullet)
    {
        this.spawnPosition = spawnposition;
        this.bulletSpeed = bulletSpeed;
        this.direction = direction;
        this.canHit = canHit;
        this.hitTarget = hitTarget;
        this.returnBullet = returnBullet;
    }
}