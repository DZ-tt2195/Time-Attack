using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class Rule : StoreBullets
{
    [Foldout("Rules info", true)]
    float timer = 0f;
    [SerializeField] float maxTimer = 10f; public float GetTime => maxTimer;
    [SerializeField] bool beOnPlayer;
    protected HashSet<Entity> entitiesInRange = new();
    Slider slider;
    protected override void Awake()
    {
        base.Awake();
        this.name = this.name.Replace("(Clone)", "");
    }
    public void AssignSlider(RulesSlider rulesslider)
    {
        this.slider = rulesslider.slider;
        this.slider.gameObject.SetActive(true);
        rulesslider.textBox.text = Translator.inst.Translate(this.name);
    }
    void Update()
    {
        if (WaveManager.state == GameState.Playing)
        {
            timer = Mathf.Min(timer+Time.deltaTime, maxTimer);
            if (CanUse())
            {
                timer = 0f;
                ActivateRule();
            }
            if (beOnPlayer)
            {
                this.transform.position = Player.instance.transform.position;
            }
            this.slider.value = timer/maxTimer;
        }
    }
    protected virtual bool CanUse()
    { 
        return timer == maxTimer;
    }
    protected virtual void ActivateRule()
    {
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entity entity))
            entitiesInRange.Add(entity);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entity entity))
            entitiesInRange.Remove(entity);        
    }
}
