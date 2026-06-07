using UnityEngine;
using UnityEngine.UI;
using MyBox;
using TMPro;
using System.Collections;
using System.Diagnostics;
using System;
using System.Linq;
public class Player : Entity
{

#region Setup

    public static Player instance;

    [Foldout("Player info", true)]
    float immuneTime = 2.5f;
    [SerializeField] int currentEnergy;
    int maxEnergy;

    protected override void Awake()
    {
        base.Awake();
        instance = this;

        this.tag = "Player";
        immuneTime *= 2 - PrefManager.GetDifficulty();
        maxEnergy = currentEnergy;
    }
    public virtual string DamageString()
    {
        if (Translator.inst.TranslationExists($"{this.name}_Damage"))
            return Translator.inst.Translate($"{this.name}_Damage");
        else
            return damage.ToString();
    }

    #endregion

#region Gameplay

    void Update()
    {
        if (WaveManager.state == GameState.Playing)
        {
            if (currentHealth > 0)
            {
                FollowMouse();
                if (Input.GetKeyDown(KeyCode.Mouse0) && currentEnergy >= 1)
                {
                    currentEnergy--;
                    AudioManager.instance.Shoot(0.3f);
                    FireWeapon();
                }
                else if (Input.GetKeyDown(KeyCode.Mouse1) && SubWeapon.inst.CanUse())
                {
                    SubWeapon.inst.UseSubWeapon();
                }
            }
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
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Resupply"))
        {
            Resupply resupply = collision.GetComponent<Resupply>();
            RulesManager.inst.HitResupply(resupply, this.currentEnergy < this.maxEnergy);
        }
        else if (collision.TryGetComponent(out Entity entity))
        {
            this.ChangeHealth(-1);
        }
        else if (collision.CompareTag("Wall") || collision.CompareTag("HurtPlayer"))
        {
            this.ChangeHealth(-1);
        }
    }
    protected override void DamageEffect(int amount)
    {
        base.DamageEffect(amount);
        StartCoroutine(Immunity(true));
    }
    public IEnumerator Immunity(bool animation)
    {
        protectionSources.Add(Protection.Immunity);
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
        protectionSources.Remove(Protection.Immunity);
    }
    protected override void DeathEffect()
    {
        base.DeathEffect();
        tookDamage++;
        MyExtensions.SetAlpha(this.spriteRenderer, 0.5f);

        Level currentLevel = ThingsToCarry.inst.CurrentLevel();
        if (currentLevel.levelType == LevelType.Endless)
        {
            int score = (int)(PrefManager.GetDifficulty() * 100) + (WaveManager.instance.currentWave-1)*10;
            WaveManager.instance.EndGame(AutoTranslate.Lost(), EndStats(), score);
        }
        else
        {
            WaveManager.instance.EndGame(AutoTranslate.Lost(), EndStats(), -1);
        }
    }
    public void ChangeEnergy(int amount)
    {
        this.currentEnergy = Mathf.Min(this.currentEnergy + amount, maxEnergy);
    }
    #endregion

#region Ending

    public (int, int) EndStats()
    {
        return (firedBullets - landedBullets, tookDamage);
    }
    public (int health, int maxHealth) EnergyInfo()
    {
        return (this.currentEnergy, this.maxEnergy);
    }

    #endregion

#region Weapon
    
    protected virtual void FireWeapon()
    { 
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, Vector3.up, damage));
        CreateBullet(bulletPrefab, new AttackInfo(this.transform.position, bulletSpeed, Vector3.down, damage));
    }

#endregion

}