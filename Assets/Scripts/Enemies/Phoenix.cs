using UnityEngine;
using System.Collections;
using TMPro;

public class Phoenix : BaseEnemy
{
    [SerializeField] float respawnTime;
    [SerializeField] TMP_Text textBox;

    protected override void Awake()
    {
        base.Awake();
        respawnTime *= 2 - PrefManager.GetDifficulty();
        textBox.gameObject.SetActive(false);
    }

    protected override void DeathEffect()
    {
        base.DeathEffect();
        StartCoroutine(Revive());

        IEnumerator Revive()
        {
            float timer = respawnTime;
            while (timer > 0)
            {
                textBox.gameObject.SetActive(true);
                timer -= Time.deltaTime;
                textBox.text = $"{timer:F1}";
                yield return null;
            }

            currentHealth = maxHealth;
            immune = false;
            healthText.text = currentHealth.ToString();

            textBox.gameObject.SetActive(false);
            crossedOut.SetActive(false);
            MyExtensions.SetAlpha(this.spriteRenderer, 1f);
        }
    }
}
