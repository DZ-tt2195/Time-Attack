using System.Collections.Generic;
using UnityEngine;

public class SubWeapon : MonoBehaviour
{
    public static SubWeapon inst;
    float timer;
    [SerializeField] float maxTimer;
    protected HashSet<BaseEnemy> enemiesInRange = new();
    void Awake()
    {
        inst = this;
        this.name = this.name.Replace("(Clone)", "");
    }
    void Update()
    {
        if (WaveManager.state == GameState.Playing)
        {
            timer = Mathf.Min(timer+Time.deltaTime, maxTimer);
            this.transform.position = Player.instance.transform.position;
        }
    }
    public bool CanUse() => timer >= maxTimer;
    public (float timer, float maxTimer) TimerInfo()
    {
        return (this.timer, this.maxTimer);
    }
    public virtual void UseSubWeapon()
    {
        this.timer = 0f;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseEnemy enemy))
            enemiesInRange.Add(enemy);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseEnemy enemy))
            enemiesInRange.Remove(enemy);        
    }
}
