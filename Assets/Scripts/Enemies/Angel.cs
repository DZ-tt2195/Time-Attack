using UnityEngine;

public class Angel : BaseEnemy
{
    GameObject wall;
    [SerializeField] float wallOff;
    [SerializeField] float wallOn;
    float timer;

    protected override void Awake()
    {
        base.Awake();
        wall = transform.Find("Barrier").gameObject;
        wallOn *= PrefManager.GetDifficulty();
        wallOff *= 2 - PrefManager.GetDifficulty();

        wall.SetActive(true);
        timer = wallOn;
    }

    protected override void Movement()
    {
        base.Movement();
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            if (wall.activeSelf)
            {
                timer = wallOff;
                wall.SetActive(false);
            }
            else
            {
                timer = wallOn;
                wall.SetActive(true);
            }
        }
    }
}
