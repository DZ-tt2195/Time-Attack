using System.Collections.Generic;
using UnityEngine;

public class Castle : BaseEnemy
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform rotateThis;
    protected override void Awake()
    {
        base.Awake();
        rotationSpeed *= PrefManager.GetDifficulty();
        rotationSpeed *= Random.Range(0, 2) == 0 ? 1 : -1;
        rotateThis.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360f));
    }
    protected override void Movement()
    {
        base.Movement();
        rotateThis.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
