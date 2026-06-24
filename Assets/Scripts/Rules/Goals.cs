using UnityEngine;
using System.Collections.Generic;

public class Goals : Rule
{
    List<Bullet> flagsInOrder = new();
    int currentNum = 0;
    protected override void Awake()
    {
        base.Awake();
        foreach(Bullet bullet in flagsInOrder)
            bullet.gameObject.SetActive(false);
    }
    protected override void ActivateRule()
    {
        currentNum = 0;
        for (int i = 0; i<flagsInOrder.Count; i++)
        {
            int thisNum = i;
            Bullet nextFlag = flagsInOrder[i];
            
            //BulletInfo info = new BulletInfo 
            //nextFlag.AssignInfo
        }
    }
}
