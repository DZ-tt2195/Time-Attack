using UnityEngine;

public class Freeze : Rule
{
    [SerializeField] float stunTime;
    [SerializeField] AudioClip freezeSound;
    public override string MyText => AutoTranslate.Freeze_Text(GetTime.ToString(), stunTime.ToString());
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
