using UnityEngine;

public class Freeze : Rule
{
    [SerializeField] float stunTime;
    [SerializeField] AudioClip freezeSound;
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(freezeSound, 0.3f);
        foreach (Entity entity in entitiesInRange)
        {
            if (entity != Player.instance)
                entity.StunThis(stunTime);
        }
    }
}
