using System.Collections.Generic;
using UnityEngine;

public class Crane : BaseEnemy
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform rotateThis;
    [SerializeField] List<CraneLift> listOfLifts = new();
    protected override void Awake()
    {
        base.Awake();
        rotationSpeed *= PrefManager.GetDifficulty();
        rotationSpeed *= (Random.Range(0, 2) == 0 ? 1 : -1);

        int randomNum = Random.Range(0, listOfLifts.Count);
        listOfLifts[randomNum].gameObject.SetActive(false);
    }
    protected override void Movement()
    {
        base.Movement();
        rotateThis.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
