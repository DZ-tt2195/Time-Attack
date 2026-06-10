using UnityEngine;
using System.Collections.Generic;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager inst;
    protected Resupply resupplyPrefab { get; private set; }
    Queue<Resupply> resupplyQueue = new();
    [SerializeField] protected int energy;
    [SerializeField] float interval;
    [SerializeField] float moveSpeed;
    void Awake()
    {
        inst = this;
        try
        {
            resupplyPrefab = this.transform.Find("Resupply").GetComponent<Resupply>();
            resupplyPrefab.gameObject.SetActive(false);
        } catch { }
    }
    public void BeginGame()
    {
        InvokeRepeating(nameof(SpawnResupply), interval/2f, interval);        
    }
    protected Resupply MakeResupply(Vector2 spawn, Vector2 direction, string text)
    {
        Resupply resupply = (resupplyQueue.Count > 0) ? resupplyQueue.Dequeue() : Instantiate(resupplyPrefab);
        resupply.Setup(spawn, direction, text);
        return resupply;
    }
    public void HitResupply(Resupply resupply, bool needEnergy)
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
    void SpawnResupply()
    {
        int randomNum = Random.Range(0, 4);
        Vector2 spawn = Vector2.zero;
        Vector2 direction = Vector2.zero;
        switch (randomNum)
        {
            case 0:
                spawn = new(Random.Range(WaveManager.minX+1f, WaveManager.maxX-1f), WaveManager.maxY-1f);
                direction = Vector2.down;
                break;
            case 1:
                spawn = new(Random.Range(WaveManager.minX+1f, WaveManager.maxX-1f), WaveManager.minY+1f);
                direction = Vector2.up;
                break;
            case 2:
                spawn = new(WaveManager.minX+1f, Random.Range(WaveManager.minY+1f, WaveManager.maxY-1f));
                direction = Vector2.right;
                break;
            case 3:
                spawn = new(WaveManager.maxX-1f, Random.Range(WaveManager.minY+1f, WaveManager.maxY-1f));
                direction = Vector2.left;
                break;
        }
        MakeResupply(spawn, direction, AutoTranslate.Energy_Pack(energy.ToString()));
    }
}
