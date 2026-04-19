using TMPro;
using UnityEngine;

public class Resupply : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    void Awake()
    {
        textBox.text = AutoTranslate.Resupply();
    }
    private void Update()
    {
        this.transform.Translate(new Vector2(0, -1.75f) * Time.deltaTime);
        if (this.transform.position.y < WaveManager.minY)
            WaveManager.instance.ReturnResupply(this);
    }
}