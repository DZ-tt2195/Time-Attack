using UnityEngine;

public class Freeze : Rule
{
    [SerializeField] float stunTime;
    [SerializeField] AudioClip freezeSound;
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(freezeSound, 0.3f);
        foreach (BaseEnemy enemy in enemiesInRange)
            enemy.StunThis(stunTime);
    }
}
