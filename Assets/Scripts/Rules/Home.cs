using UnityEngine;

public class Home : Rule
{
    [SerializeField] Transform homeSprite;
    [SerializeField] AudioClip warp;
    protected override void Awake()
    {
        base.Awake();
        NewHome();
    }
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(warp, 0.3f);
        Player.instance.transform.position = homeSprite.transform.position;
        NewHome();
    }
    void NewHome()
    {
        homeSprite.transform.position = new Vector2(WaveManager.RandomX(1f), WaveManager.RandomY(1f));        
    }
}
