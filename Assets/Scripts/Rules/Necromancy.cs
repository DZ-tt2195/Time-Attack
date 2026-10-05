using UnityEngine;

public class Necromancy : Rule
{
    [SerializeField] int healing;
    public override string MyText => AutoTranslate.Necromancy_Text(GetTime.ToString(), healing.ToString());
    protected override void ActivateRule()
    {
        foreach (BaseEnemy enemy in WaveManager.instance.GetEnemies())
        {
            if (enemy.GetHealth() == 0)
                enemy.ChangeHealth(healing);
        }
    }
}