using UnityEngine;

public class Deplete : Rule
{
    protected override void ActivateRule()
    {
        Player.instance.ChangeEnergy(-5);
    }
}
