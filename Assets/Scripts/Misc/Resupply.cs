using TMPro;
using UnityEngine;

public class Resupply : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    public void Setup(Vector3 spawn, string text)
    {
        this.tag = "Resupply";
        this.transform.position = spawn;
        if (textBox != null) this.textBox.text = text;
        this.gameObject.SetActive(true);        
    }
}