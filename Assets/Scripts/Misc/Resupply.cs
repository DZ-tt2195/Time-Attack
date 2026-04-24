using TMPro;
using UnityEngine;

public class Resupply : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    public int energy {get; private set;}
    Vector2 movement;
    
    public void Setup(Vector3 spawn, int energy, Vector2 movement)
    {
        this.transform.position = spawn;
        this.energy = energy;
        this.textBox.text = AutoTranslate.Resupply(this.energy.ToString());
        this.movement = movement;
        this.gameObject.SetActive(true);        
    }
    private void Update()
    {
        this.transform.Translate(movement * Time.deltaTime);
        if (this.transform.position.x < WaveManager.minX - 0.5f || this.transform.position.x > WaveManager.maxX + 0.5f ||
            this.transform.position.y < WaveManager.minY - 0.5f || this.transform.position.y > WaveManager.maxY + 0.5f)
            EnergyManager.inst.ReturnResupply(this);
    }
}