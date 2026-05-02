using UnityEngine;
using System.Collections.Generic;

public class RulesManager : MonoBehaviour
{
    public static RulesManager inst;
    protected HealthPack resupplyPrefab { get; private set; }
    Queue<HealthPack> resupplyQueue = new();
    [SerializeField] protected int health;
    void Awake()
    {
        inst = this;
        try
        {
            resupplyPrefab = this.transform.Find("Resupply").GetComponent<HealthPack>();
            resupplyPrefab.gameObject.SetActive(false);
        } catch { }
    }
    public virtual void BeginGame()
    {
    }
    public virtual void EveryWave()
    {
    }
    protected HealthPack MakeResupply(Vector2 spawn, string text, Vector2 speed)
    {
        HealthPack resupply = (resupplyQueue.Count > 0) ? resupplyQueue.Dequeue() : Instantiate(resupplyPrefab);
        resupply.Setup(spawn, text, speed);
        return resupply;
    }
    public virtual void HitResupply(HealthPack resupply, bool needEnergy)
    {
        if (needEnergy)
        {
            ReturnResupply(resupply);
            Player.instance.ChangeHealth(health);
        }
    }
    public void ReturnResupply(HealthPack resupply)
    {
        resupplyQueue.Enqueue(resupply);
        resupply.gameObject.SetActive(false);        
    }
}
