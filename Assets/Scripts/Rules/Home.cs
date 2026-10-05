using UnityEngine;

public class Home : Rule
{
    [SerializeField] SpriteRenderer homeSprite;
    protected override void Awake()
    {
        base.Awake();
        MoveHome();
        EventManager.inst.Subscribe<EventEnergy>(energy => ChangedEnergy(energy));

        void ChangedEnergy(EventEnergy info)
        {
            if (info.energyChange > 0)
                Player.instance.transform.position = homeSprite.transform.position;
        }
    }
    protected override void ActivateRule()
    {
        MoveHome();
    }
    void MoveHome()
    {
        homeSprite.transform.position = new Vector2(WaveManager.RandomX(1f), WaveManager.RandomY(1f));        
    }
}