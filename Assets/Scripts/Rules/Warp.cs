using UnityEngine;

public class Warp : Rule
{
    [SerializeField] float stunTime;
    protected override void ActivateRule()
    {
        foreach (BaseEnemy enemy in WaveManager.instance.GetEnemies())
        {
            int randomNum = Random.Range(0, 4);
            switch (randomNum)
            {
                case 0:
                    enemy.transform.position = new Vector2(WaveManager.minX, WaveManager.minY);
                    break;
                case 1:
                    enemy.transform.position = new Vector2(WaveManager.minX, WaveManager.maxY);
                    break;
                case 2:
                    enemy.transform.position = new Vector2(WaveManager.maxX, WaveManager.minY);
                    break;
                case 3:
                    enemy.transform.position = new Vector2(WaveManager.maxX, WaveManager.maxY);
                    break;
            }
        }
    }
}
