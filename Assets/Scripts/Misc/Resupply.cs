using TMPro;
using UnityEngine;

public class Resupply : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    Vector2 movement;
    
    public void Setup(Vector3 spawn, string text, Vector2 movement)
    {
        this.tag = "Resupply";
        this.transform.position = spawn;
        this.textBox.text = text;
        this.movement = movement;
        this.gameObject.SetActive(true);        
    }
    private void Update()
    {
        this.transform.Translate(movement * Time.deltaTime);
        if (this.transform.position.x < WaveManager.minX || this.transform.position.x > WaveManager.maxX ||
            this.transform.position.y < WaveManager.minY || this.transform.position.y > WaveManager.maxY)
            EnergyManager.inst.ReturnResupply(this);
    }
}