using UnityEngine;

public class New4 : Rule
{
    [SerializeField] AudioClip jammed;
    bool jam = false;
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(jammed);
        jam = !jam;

        if (jam)
        {
            Player.instance.clockHands[1].toSpin.localEulerAngles = Player.instance.clockHands[0].toSpin.localEulerAngles;
        }
        else
        {
            Player.instance.RandomizeHandTwo();
        }
    }
}