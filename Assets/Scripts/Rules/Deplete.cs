using UnityEngine;

public class Deplete : Rule
{
    [SerializeField] AudioClip deplete;
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(deplete, 0.5f);
        Player.instance.ChangeEnergy(-10);
    }
}
