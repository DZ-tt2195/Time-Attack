using UnityEngine;

public class Jammed : Rule
{
    [SerializeField] AudioClip jammed;
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(jammed);
        Player.instance.spinHands = !Player.instance.spinHands;
    }
}
