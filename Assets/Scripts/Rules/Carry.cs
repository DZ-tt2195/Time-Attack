using System;
using UnityEngine;

public class Carry : RulesManager
{
    [SerializeField] Vector2 movement;
    public override void BeginGame()
    {
        MoveBack();
    }
    public override void HitResupply(Resupply resupply, bool needEnergy)
    {
        resupply.transform.Translate(movement*Time.deltaTime, Space.World);
        if (resupply.transform.position.x > WaveManager.maxX)
        {
            ReturnResupply(resupply);
            Player.instance.ChangeHealth(health);
            MoveBack();
        }
    }   
    void MoveBack()
    {
        MakeResupply(new Vector2(WaveManager.minX + 1f, -2f), AutoTranslate.Blank());
    }
}
