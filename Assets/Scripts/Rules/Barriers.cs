using System.Collections.Generic;
using UnityEngine;

public class Barriers : Rule
{
    [SerializeField] List<GameObject> listOfWalls = new();
    [SerializeField] AudioClip shieldOn;
    [SerializeField] AudioClip shieldOff;
    protected override void ActivateRule()
    {
        foreach (GameObject next in listOfWalls)
        {
            if (next.activeSelf)
            {
                next.SetActive(false);
                AudioManager.instance.PlaySound(shieldOff, 0.1f);
            }
            else
            {
                next.SetActive(true);
                AudioManager.instance.PlaySound(shieldOn, 0.1f);
            }
        }
    }
}
