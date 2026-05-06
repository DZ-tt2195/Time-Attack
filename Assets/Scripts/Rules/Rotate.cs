using UnityEngine;

public class Rotate : RulesManager
{
    [SerializeField] Vector3 rotate;
    float time = 0f;
    public override void BeginGame()
    {
        MakeResupply(new Vector2(0, -1), AutoTranslate.Blank());
    }
    public override void HitResupply(Resupply resupply, bool needEnergy)
    {
        time+=Time.deltaTime;
        resupply.transform.Rotate(rotate*Time.deltaTime);
        if (time > 1f && needEnergy)
        {
            Player.instance.ChangeHealth(health);
            time = 0f;
        }
    }
}
