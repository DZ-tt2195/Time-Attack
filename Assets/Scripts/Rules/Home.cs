using UnityEngine;

public class Home : Rule
{
    [SerializeField] SpriteRenderer homeSprite;
    int lastEnergy;
    protected override void Awake()
    {
        base.Awake();
        MoveHome();
    }
    protected override void EveryFrame()
    {
        int newEnergy = Player.instance.EnergyInfo().currentEnergy;
        if (newEnergy > lastEnergy)
            Player.instance.transform.position = homeSprite.transform.position;
        lastEnergy = newEnergy;
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
