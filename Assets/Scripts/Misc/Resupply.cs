using TMPro;
using UnityEngine;

public class Resupply : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    Vector2 direction;
    public void Setup(Vector2 spawn, Vector2 direction, string text)
    {
        this.tag = "Resupply";
        this.transform.position = spawn;
        if (textBox != null) this.textBox.text = text;
        this.direction = direction;
        this.gameObject.SetActive(true);        
    }
    void Update()
    {
        this.transform.Translate(direction*Time.deltaTime);
        if (this.transform.position.x < WaveManager.minX - 0.5f || this.transform.position.x > WaveManager.maxX + 0.5f ||
            this.transform.position.y < WaveManager.minY - 0.5f || this.transform.position.y > WaveManager.maxY + 0.5f)
            RulesManager.inst.ReturnResupply(this);
    }
}