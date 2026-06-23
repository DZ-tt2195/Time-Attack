using UnityEngine;

public class Home : Rule
{
    [SerializeField] Transform homeSprite;
    protected override void Awake()
    {
        base.Awake();
        NewHome();
    }
    protected override void ActivateRule()
    {
        Player.instance.transform.position = homeSprite.transform.position;
        NewHome();
    }
    void NewHome()
    {
        homeSprite.transform.position = new Vector2(Random.Range(WaveManager.minX + 1f, WaveManager.maxX - 1f), Random.Range(WaveManager.minY + 1f, WaveManager.maxY - 1f));        
    }
}
