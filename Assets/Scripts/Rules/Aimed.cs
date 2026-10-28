using UnityEngine;

public class Aimed : Rule
{
    [SerializeField] AudioClip jammed;
    bool aimed = false;
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(jammed);
        aimed = !aimed;

        if (aimed)
        {
            Player.instance.clockHands[1].toSpin.localEulerAngles = Player.instance.clockHands[0].toSpin.localEulerAngles;
        }
        else
        {
            Player.instance.RandomizeHandTwo();
        }
    }
}