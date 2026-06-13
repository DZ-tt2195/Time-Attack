using UnityEngine;
using MyBox;
using System.Collections;
using System.Collections.Generic;
public class Player : Entity
{

#region Setup

    public static Player instance;

    [Foldout("Player info", true)]
    [SerializeField] float immuneTime;
    int currentEnergy;
    [SerializeField] int maxEnergy;
    [SerializeField] List<Transform> toSpin = new();
    [SerializeField] float spinSpeed;

    protected override void Awake()
    {
        base.Awake();
        instance = this;

        this.tag = "Player";
        immuneTime *= 2 - PrefManager.GetDifficulty();
        currentEnergy = maxEnergy;

        float randomAngle1 = Random.Range(0f, 360f);
        toSpin[0].eulerAngles = new(0, 0, randomAngle1);

        float randomAngle2 = randomAngle1 + Random.Range(60f, 300f);
        toSpin[1].eulerAngles = new(0, 0, randomAngle2);
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
                foreach (Transform next in toSpin)
                    next.Rotate(0, 0, spinSpeed * Time.deltaTime); 

                if (Input.GetKeyDown(KeyCode.Mouse0) && currentEnergy >= 1)
                {
                    currentEnergy--;
                    AudioManager.instance.Shoot(0.3f);

                    for (int i = 0; i<toSpin.Count; i++)
                    {
                        Bullet newBullet = (bulletQueue.Count > 0) ? bulletQueue.Dequeue() : Instantiate(bulletPrefab);
                        Vector2 dir = toSpin[i].right;
                        CreateBullet(DefaultAttack(toSpin[i].position, dir));
                    }
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
            EnergyManager.inst.HitResupply(resupply, this.currentEnergy < this.maxEnergy);
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

        Level currentLevel = GameFiles.inst.CurrentLevel();
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
    public (int, int) EndStats()
    {
        return (firedBullets - landedBullets, tookDamage);
    }
    public (int health, int maxHealth) EnergyInfo()
    {
        return (this.currentEnergy, this.maxEnergy);
    }

    #endregion

}