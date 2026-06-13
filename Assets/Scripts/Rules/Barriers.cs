using System.Collections.Generic;
using UnityEngine;

public class Barriers : Rule
{
    [SerializeField] List<GameObject> listOfWalls = new();
    protected override void ActivateRule()
    {
        foreach (GameObject next in listOfWalls)
        {
            next.SetActive(!next.activeSelf);
        }
    }
}
