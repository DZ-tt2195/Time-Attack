using UnityEngine;
using System;
using MyBox;
using System.Collections.Generic;

[Serializable]
public class OrbInfo
{
    public Bullet bullet;
    public float rot;
    public OrbInfo (Bullet bullet)
    {
        this.bullet = bullet;
        rot = 0;
    }
}
public class Crown : Player
{
    [Foldout("Orbit info", true)]
    List<OrbInfo> toSpin = new();
    [SerializeField] float spinSpeed;
    [SerializeField] float distance;
    
    protected override void EveryFrame()
    {
        for (int i = toSpin.Count-1; i>=0; i--)
        {
            OrbInfo nextInfo = toSpin[i];
            if (!nextInfo.bullet.gameObject.activeSelf)
            {
                toSpin.RemoveAt(i);
            }
            else
            {
                nextInfo.rot += spinSpeed * Mathf.Rad2Deg * Time.deltaTime; 
                float rad = nextInfo.rot * Mathf.Deg2Rad; 
                float x = this.transform.position.x + Mathf.Cos(rad) * distance; 
                float y = this.transform.position.y + Mathf.Sin(rad) * distance; 
                nextInfo.bullet.transform.position = new Vector3(x, y, 0);                    
            }
        }
    }
    protected override void FireWeapon()
    {
        Bullet bullet = CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, Vector3.zero));
        toSpin.Add(new OrbInfo(bullet));
    }
}
