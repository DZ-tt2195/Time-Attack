using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Mortar : BaseEnemy
{
    [SerializeField] float randomize;
    [SerializeField] List<SpriteRenderer> listOfRadiuses = new();
    [SerializeField] AudioClip attack;

    protected override void Awake()
    {
        base.Awake();
        foreach (SpriteRenderer next in listOfRadiuses)
        {
            next.gameObject.SetActive(false);
            next.transform.SetParent(null);
        }
        Invoke(nameof(ShootBullet), attackRate*0.5f);
    }
    protected override void ShootBullet()
    {
        Vector2 playerPosition = Player.instance.transform.position;
        foreach (SpriteRenderer next in listOfRadiuses)
        {
            next.transform.position = new(playerPosition.x + RandomOffSet(), playerPosition.y + RandomOffSet());
            next.gameObject.SetActive(true);
        }
        StartCoroutine(AttackAll());
        IEnumerator AttackAll()
        {
            float elapsedTime = 0f;
            while (elapsedTime < attackRate)
            {
                elapsedTime += Time.deltaTime;
                foreach (SpriteRenderer next in listOfRadiuses)
                {
                    MyExtensions.SetAlpha(next, elapsedTime / attackRate);
                }
                yield return null;
            }
            AudioManager.instance.PlaySound(attack, 0.1f);
            foreach (SpriteRenderer next in listOfRadiuses)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(next.transform.position, 0.8f);
                foreach (Collider2D nextCollider in colliders)
                {
                    if (nextCollider.gameObject == Player.instance.gameObject)
                        Player.instance.ChangeHealth(-1);
                }
                next.gameObject.SetActive(false);
            }
            Invoke(nameof(ShootBullet), attackRate*0.5f);
        }

        float RandomOffSet()
        {
            return Random.Range(-randomize, randomize);
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        for (int i = 0; i<listOfRadiuses.Count; i++)
            Destroy(listOfRadiuses[i].gameObject);
    }
}
