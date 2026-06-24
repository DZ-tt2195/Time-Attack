using UnityEngine;

public class Warp : Rule
{
    [SerializeField] float stunTime;
    [SerializeField] AudioClip warp;
    protected override void ActivateRule()
    {
        AudioManager.instance.PlaySound(warp, 0.3f);
        foreach (BaseEnemy enemy in WaveManager.instance.GetEnemies())
        {
            int randomNum = Random.Range(0, 4);
            switch (randomNum)
            {
                case 0:
                    enemy.transform.position = new Vector2(WaveManager.minX, WaveManager.RandomY(0));
                    break;
                case 1:
                    enemy.transform.position = new Vector2(WaveManager.maxX, WaveManager.RandomY(0));
                    break;
                case 2:
                    enemy.transform.position = new Vector2(WaveManager.RandomX(0), WaveManager.minY);
                    break;
                case 3:
                    enemy.transform.position = new Vector2(WaveManager.RandomX(0), WaveManager.maxY);
                    break;
            }
        }
    }
}
