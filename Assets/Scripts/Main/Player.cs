using UnityEngine;
using UnityEngine.UI;
using MyBox;
using TMPro;
using System.Collections;
using System.Diagnostics;
using System;
using System.Linq;
public enum GameState {Setup, Playing, Paused, Over}
public class Player : Entity
{

#region Setup

    public static Player instance;
    public static GameState state = GameState.Setup;

    [Foldout("Player info", true)]
    protected int currentEnergy;
    [SerializeField] int maxEnergy;
    float immuneTime = 2.5f;
    GameObject blackOutObject;
    [ReadOnly] public float blackOutTime = 0f;
    protected int firedBullets;
    int tookDamage;

    protected override void Awake()
    {
        base.Awake();
        instance = this;

        this.tag = "Player";
        currentEnergy = maxEnergy;
        blackOutObject = this.transform.Find("Blackout").gameObject;
        immuneTime *= 2 - PrefManager.GetDifficulty();
    }

    #endregion

#region Gameplay

    void Update()
    {
        if (state == GameState.Playing)
        {
            if (health > 0)
            {
                FollowMouse();
                if (Input.GetKeyDown(KeyCode.Mouse0) && CanUseWeapon())
                    FireWeapon();
            }

            if (blackOutTime > 0f)
                blackOutTime -= Time.deltaTime;
            blackOutObject.SetActive(blackOutTime > 0f);
        }
    }
    void FollowMouse()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 targetPosition = WaveManager.instance.mainCamera.ScreenToWorldPoint
            (new(mouseScreenPosition.x, mouseScreenPosition.y, WaveManager.instance.mainCamera.nearClipPlane));

        targetPosition.x = Mathf.Clamp(targetPosition.x, WaveManager.minX, WaveManager.maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, WaveManager.minY, WaveManager.maxY);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entity entity))
        {
            this.TakeDamage();
        }
        else if (collision.CompareTag("Wall") || collision.CompareTag("HurtOnTouch"))
        {
            this.TakeDamage();
        }
        else if (collision.CompareTag("Resupply") && currentEnergy < maxEnergy)
        {
            WaveManager.instance.ReturnResupply(collision.GetComponent<Resupply>());
            AddEnergy(2);
        }
        else if (collision.CompareTag("HealthPack") && health < maxHealth)
        {
            Destroy(collision.gameObject);
            health++;
        }
    }
    public void AddEnergy(int addition)
    {
        currentEnergy = Mathf.Min(currentEnergy + addition, maxEnergy);
    }

    #endregion

#region Ending

    protected override void DamageEffect()
    {
        tookDamage++;
        StartCoroutine(Immunity(true));
    }
    public IEnumerator Immunity(bool animation)
    {
        immune = true;
        float elapsedTime = 0f;
        bool flicker = true;

        Vector3 darkness = new(0.1f, 0.1f, 0.1f);
        Vector3 gray = new(0.25f, 0.25f, 0.25f);
        if (animation)
            WaveManager.instance.mainCamera.backgroundColor = new(darkness.x, darkness.y, darkness.z);

        while (elapsedTime < immuneTime)
        {
            elapsedTime += Time.deltaTime;
            if (animation)
            {
                flicker = !flicker;
                MyExtensions.SetAlpha(this.spriteRenderer, flicker ? (elapsedTime / immuneTime) : 1f);
                Vector3 target = Vector3.Lerp(darkness, gray, elapsedTime / immuneTime);
                WaveManager.instance.mainCamera.backgroundColor = new(target.x, target.y, target.z);
            }
            yield return null;
        }

        WaveManager.instance.mainCamera.backgroundColor = new(gray.x, gray.y, gray.z);
        MyExtensions.SetAlpha(this.spriteRenderer, 1);
        immune = false;        
    }
    protected override void DeathEffect()
    {
        immune = true;
        tookDamage++;
        MyExtensions.SetAlpha(this.spriteRenderer, 0.5f);

        Level currentLevel = ThingsToCarry.inst.CurrentLevel();
        if (currentLevel.levelType == LevelType.Endless)
        {
            int score = (int)(PrefManager.GetDifficulty() * 100) + (WaveManager.instance.currentWave-1)*10;
            WaveManager.instance.EndGame(AutoTranslate.Lost(), EndStats(), score);
            if (score > PrefManager.GetScore(currentLevel.levelName.ToString()))
                PrefManager.SetScore(currentLevel.levelName.ToString(), score);
        }
        else
        {
            WaveManager.instance.EndGame(AutoTranslate.Lost(), EndStats(), -1);
        }
    }
    public (int, int) EndStats()
    {
        return (firedBullets - landedBullets, tookDamage);
    }
    public (int health, int maxHealth, int energy, int maxEnergy) HealthEnergy()
    {
        return (this.health, this.maxHealth, this.currentEnergy, this.maxEnergy);
    }

    #endregion

#region Weapon
    
    protected virtual bool CanUseWeapon()
    {
        return currentEnergy >= 1;
    }
    protected virtual void FireWeapon()
    { 
        currentEnergy--;
        firedBullets++;
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, Vector3.up));
    }

#endregion
}