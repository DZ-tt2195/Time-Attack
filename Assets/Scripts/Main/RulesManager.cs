using UnityEngine;
using System.Collections.Generic;

public class RulesManager : MonoBehaviour
{
    public static RulesManager inst;
    protected Resupply resupplyPrefab { get; private set; }
    Queue<Resupply> resupplyQueue = new();
    [SerializeField] protected int health;
    protected List<Resupply> activeResupplies = new();
    void Awake()
    {
        inst = this;
        try
        {
            resupplyPrefab = this.transform.Find("Resupply").GetComponent<Resupply>();
            resupplyPrefab.gameObject.SetActive(false);
        } catch { }
    }
    public virtual void BeginGame()
    {
    }
    public virtual void EveryWave()
    {
    }
    protected Resupply MakeResupply(Vector2 spawn, string text)
    {
        Resupply resupply = (resupplyQueue.Count > 0) ? resupplyQueue.Dequeue() : Instantiate(resupplyPrefab);
        activeResupplies.Add(resupply);
        resupply.Setup(spawn, text);
        return resupply;
    }
    public virtual void HitResupply(Resupply resupply, bool needEnergy)
    {
        if (needEnergy)
        {
            ReturnResupply(resupply);
            Player.instance.ChangeHealth(health);
        }
    }
    public void ReturnResupply(Resupply resupply)
    {
        resupplyQueue.Enqueue(resupply);
        activeResupplies.Remove(resupply);
        resupply.gameObject.SetActive(false);        
    }
}
