using UnityEngine;

public class Bomb : Rule
{
    bool canAttack = true;
    float currentStun = 0;
    [SerializeField] int damage;
    [SerializeField] float stunTime;
    [SerializeField] float moveSpeed;
    [SerializeField] AudioClip boom;
    protected override void Awake()
    {
        base.Awake();
        ResetBomb();
    }
    protected override void EveryFrame()
    {
        currentStun = Mathf.Max(0, currentStun-Time.deltaTime);
        if (currentStun == 0f)
        {
            this.transform.Translate(moveSpeed * Time.deltaTime * Vector2.down);
        }
        if (this.transform.position.y < WaveManager.minY && canAttack)
        {
            canAttack = false;
            AudioManager.instance.PlaySound(boom, 0.3f);
            Player.instance.ChangeHealth(-damage);
        }
    }
    protected override void ActivateRule()
    {
        ResetBomb();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            currentStun += stunTime;
            AudioManager.instance.Damage();
        }
    }
    void ResetBomb()
    {
        canAttack = true;
        this.transform.position = new Vector2(WaveManager.RandomX(0), WaveManager.maxY);
    }    
}
