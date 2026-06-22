using UnityEngine;
using TMPro;

public class Phoenix : BaseEnemy
{
    [SerializeField] float respawnTime;
    [SerializeField] TMP_Text textBox;
    float currentTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        respawnTime *= 2 - PrefManager.GetDifficulty();
        textBox.text = "";
    }
    protected override void Movement()
    {
        base.Movement();
        if (currentHealth == 0)
        {
            currentTimer -= Time.deltaTime;
            textBox.text = $"{currentTimer:F1}";
            if (currentTimer < 0f)
                ChangeHealth(maxHealth);
        }
        else
        {
            textBox.text = "";
        }
    }

    protected override void DeathEffect()
    {
        base.DeathEffect();
        currentTimer = respawnTime;
    }
}
