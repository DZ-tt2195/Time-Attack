using UnityEngine;
using TMPro;

public class HealthPack : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    void Awake()
    {
        textBox.text = AutoTranslate.Health_Pack();
    }
    private void Update()
    {
        this.transform.Translate(new Vector2(0, -1f) * Time.deltaTime);
        if (this.transform.position.y < WaveManager.minY)
            Destroy(this.gameObject);
    }
}
