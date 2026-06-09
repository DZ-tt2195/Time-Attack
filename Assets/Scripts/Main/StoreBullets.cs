using UnityEngine;
using System.Collections.Generic;
using MyBox;

public class StoreBullets : MonoBehaviour
{
    [Foldout("Bullet info", true)]
    protected int firedBullets;
    [SerializeField] protected float bulletSpeed;
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
    }
    public void ReturnBullet(Bullet bullet, bool landed)
    {
        bulletQueue.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
        if (landed)
            landedBullets++;
    }
}
