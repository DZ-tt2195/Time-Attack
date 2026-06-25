using UnityEngine;

public class Angel : BaseEnemy
{
    [SerializeField] GameObject wall;
    [SerializeField] float wallOff;
    [SerializeField] float wallOn;
    [SerializeField] AudioClip shieldOn;
    [SerializeField] AudioClip shieldOff;
    float timer;

    protected override void Awake()
    {
        base.Awake();
        wallOn *= PrefManager.GetDifficulty();
        wallOff *= 2 - PrefManager.GetDifficulty();

        wall.SetActive(true);
        timer = wallOn;
    }
    protected override void EveryFrame()
    {
        base.EveryFrame();
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            if (wall.activeSelf)
            {
                timer = wallOff;
                wall.SetActive(false);
                AudioManager.instance.PlaySound(shieldOff, 0.1f);
            }
            else
            {
                timer = wallOn;
                wall.SetActive(true);
                AudioManager.instance.PlaySound(shieldOn, 0.1f);
            }
        }
    }
}
