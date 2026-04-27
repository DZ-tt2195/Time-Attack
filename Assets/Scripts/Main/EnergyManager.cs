using UnityEngine;
using System.Collections.Generic;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager inst;
    protected Resupply resupplyPrefab { get; private set; }
    Queue<Resupply> resupplyQueue = new();
    [SerializeField] protected int energy;
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
    protected Resupply MakeResupply(Vector2 spawn, string text, Vector2 speed)
    {
        Resupply resupply = (resupplyQueue.Count > 0) ? resupplyQueue.Dequeue() : Instantiate(resupplyPrefab);
        resupply.Setup(spawn, text, speed);
        return resupply;
    }
    public virtual void HitResupply(Resupply resupply, bool needEnergy)
    {
        if (needEnergy)
        {
            ReturnResupply(resupply);
            Player.instance.ChangeEnergy(energy);
        }
    }
    public void ReturnResupply(Resupply resupply)
    {
        resupplyQueue.Enqueue(resupply);
        resupply.gameObject.SetActive(false);        
    }
}
