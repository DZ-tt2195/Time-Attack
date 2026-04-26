using UnityEngine;
using System.Collections.Generic;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager inst;
    protected Resupply resupplyPrefab { get; private set; }
    Queue<Resupply> resupplyQueue = new();
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
        InvokeRepeating(nameof(SpawnResupply), 1f, 2.25f);        
    }
    protected Resupply SpawnResupply()
    {
        Resupply resupply = (resupplyQueue.Count > 0) ? resupplyQueue.Dequeue() : Instantiate(resupplyPrefab);
        resupply.Setup(new(Random.Range(WaveManager.minX + 0.5f,WaveManager. maxX - 0.5f), WaveManager.maxY), 2, new(0, -1.75f));
        return resupply;
    }
    public virtual void ReturnResupply(Resupply resupply)
    {
        resupplyQueue.Enqueue(resupply);
        resupply.gameObject.SetActive(false);
    }
}
