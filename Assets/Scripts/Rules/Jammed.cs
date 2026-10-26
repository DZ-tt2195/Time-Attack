using UnityEngine;

public class Jammed : Rule
{
    [SerializeField] AudioClip jammed;
    bool jam = false;
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(jammed);
        jam = !jam;
        foreach (ClockHands hand in Player.instance.clockHands)
            hand.spinSpeed = jam ? 0 : Player.instance.SpinSpeed;
    }
}