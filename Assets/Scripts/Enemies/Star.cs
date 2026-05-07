using System.Data.Common;
using UnityEngine;

public class Star : Bullet
{
    protected override void TryAndReturn(bool landed)
    {
        this.gameObject.SetActive(false);
    }
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            this.owner.ChangeHealth(-1);
            TryAndReturn(true);            
        }
        else
        {
            base.OnTriggerStay2D(collision);            
        }
    }

}