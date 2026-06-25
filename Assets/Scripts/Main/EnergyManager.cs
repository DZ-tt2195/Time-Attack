using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class EnergyManager : StoreBullets
{
    public static EnergyManager inst;
    [SerializeField] protected int energy;
    [SerializeField] float interval;
    [SerializeField] TMP_Text energyText;
    protected override void Awake()
    {
        base.Awake();
        inst = this;
        bulletSpeed *= PrefManager.GetDifficulty();
        energyText.text = AutoTranslate.Resupply_Text(energy.ToString());
    }
    public void BeginGame()
    {
        InvokeRepeating(nameof(SpawnResupply), interval/2f, interval);        
    }
    void SpawnResupply()
    {
        int randomNum = Random.Range(0, 4);
        Vector2 spawn = Vector2.zero;
        Vector2 direction = Vector2.zero;
        switch (randomNum)
        {
            case 0:
                spawn = new(WaveManager.RandomX(1f), WaveManager.maxY-1f);
                direction = Vector2.down;
                break;
            case 1:
                spawn = new(WaveManager.RandomX(1f), WaveManager.minY+1f);
                direction = Vector2.up;
                break;
            case 2:
                spawn = new(WaveManager.minX+1f, WaveManager.RandomY(1f));
                direction = Vector2.right;
                break;
            case 3:
                spawn = new(WaveManager.maxX-1f, WaveManager.RandomY(1f));
                direction = Vector2.left;
                break;
        }

        CreateBullet(new BulletInfo(spawn, bulletSpeed, direction, IsPlayer, GiveEnergy, ReturnBullet));

        bool IsPlayer(Entity entity, Bullet bullet) => entity is Player;
        void GiveEnergy(Entity entity) => Player.instance.ChangeEnergy(energy);
    }
}