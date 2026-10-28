using UnityEngine;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System;
[Serializable]
public class ClockHands
{
    public Transform toSpin;
    [ReadOnly] public float spinSpeed;
}
public class EventEnergy : CustomEvent
{
    public int energyChange { get; private set; }

    public EventEnergy(int energyChange)
    {
        this.energyChange = energyChange;
    }
}
public class PlayerTouch : CustomEvent
{
    public GameObject obj;

    public PlayerTouch(GameObject obj)
    {
        this.obj = obj;
    }
}
public class Player : Entity
{
#region Setup
    public static Player instance;
    [Foldout("Player info", true)]
    [SerializeField] float immuneTime;
    int currentEnergy;
    [SerializeField] int maxEnergy;
    public List<ClockHands> clockHands = new();
    [SerializeField] float moveSpeed;
    [SerializeField] float defaultSpin; public float SpinSpeed => defaultSpin;

    protected override void Awake()
    {
        base.Awake();
        instance = this;

        immuneTime *= 2 - PrefManager.GetDifficulty();
        currentEnergy = maxEnergy;

        float randomAngle1 = UnityEngine.Random.Range(0f, 360f);
        clockHands[0].toSpin.eulerAngles = new(0, 0, UnityEngine.Random.Range(0f, 360f));
        clockHands[0].spinSpeed = defaultSpin;

        clockHands[1].spinSpeed = defaultSpin;
        RandomizeHandTwo();
    }
    public void RandomizeHandTwo()
    {
        float randomAngle1 = clockHands[0].toSpin.eulerAngles.z;        
        float randomAngle2 = randomAngle1 + UnityEngine.Random.Range(60f, 300f);
        clockHands[1].toSpin.eulerAngles = new(0, 0, randomAngle2);
    }
    #endregion
#region Gameplay
    protected override void EveryFrame()
    {
        if (currentHealth == 0)
            return;
            /*
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = Mathf.Abs(WaveManager.instance.mainCamera.transform.position.z);
            Vector3 targetPosition = WaveManager.instance.mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            targetPosition.x = Mathf.Clamp(targetPosition.x, WaveManager.minX, WaveManager.maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, WaveManager.minY, WaveManager.maxY);
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            */

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        transform.position += (Vector3)(moveSpeed * Time.deltaTime * input);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, WaveManager.minX, WaveManager.maxX);
        pos.y = Mathf.Clamp(pos.y, WaveManager.minY, WaveManager.maxY);
        transform.position = pos;

        foreach (ClockHands next in clockHands)
            next.toSpin.Rotate(0, 0, next.spinSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentEnergy >= 1)
            {
                ChangeEnergy(-1);
                AudioManager.instance.Shoot(0.3f);
                foreach (ClockHands next in clockHands)
                    CreateBullet(DefaultAttack(next.toSpin.position, next.toSpin.right));  
            }
            else
            {
                AudioManager.instance.Miss(0.3f);                    
            }               
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    { 
        if (collision.TryGetComponent(out Entity entity))
            this.ChangeHealth(-1);
        else if (collision.CompareTag("Wall") || collision.CompareTag("HurtPlayer"))
            this.ChangeHealth(-1);
        else if (collision.CompareTag("PlayerTrigger"))
            EventManager.inst.RunTriggers(new PlayerTouch(collision.gameObject));
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
                MyExtensions.SetAlpha(this.GetSpriteRenderer, flicker ? (elapsedTime / immuneTime) : 1f);
                Vector3 target = Vector3.Lerp(darkness, gray, elapsedTime / immuneTime);
                WaveManager.instance.mainCamera.backgroundColor = new(target.x, target.y, target.z);
            }
            yield return null;
        }

        WaveManager.instance.mainCamera.backgroundColor = new(gray.x, gray.y, gray.z);
        MyExtensions.SetAlpha(this.GetSpriteRenderer, 1);
        protectionSources.Remove(Protection.Immunity);
    }
    protected override void DeathEffect()
    {
        base.DeathEffect();
        MyExtensions.SetAlpha(this.GetSpriteRenderer, 0.5f);

        Level currentLevel = GameFiles.inst.CurrentLevel();
        if (currentLevel.levelType == LevelType.Endless)
        {
            int score = (int)(PrefManager.GetDifficulty() * 100) + (WaveManager.instance.currentWave-1)*10;
            WaveManager.instance.EndGame(AutoTranslate.Lost(), tookDamage, score);
        }
        else
        {
            WaveManager.instance.EndGame(AutoTranslate.Lost(), tookDamage, -1);
        }
    }
    public void ChangeEnergy(int amount)
    {
        this.currentEnergy = Mathf.Clamp(this.currentEnergy + amount, 0, maxEnergy);
        if (amount > 0)
            AudioManager.instance.Heal(0.3f);
        if (amount != 0)
            EventManager.inst.RunTriggers(new EventEnergy(amount));
    }
    public int DamageTaken() => tookDamage;
    public (int currentEnergy, int maxEnergy) EnergyInfo()
    {
        return (this.currentEnergy, this.maxEnergy);
    }
    #endregion
}