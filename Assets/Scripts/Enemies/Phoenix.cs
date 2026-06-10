using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Phoenix : BaseEnemy
{
    [SerializeField] float respawnTime;
    [SerializeField] TMP_Text textBox;

    protected override void Awake()
    {
        base.Awake();
        respawnTime *= 2 - PrefManager.GetDifficulty();
        textBox.text = "";
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

            ChangeHealth(this.maxHealth);
            textBox.text = "";
        }
    }
}
