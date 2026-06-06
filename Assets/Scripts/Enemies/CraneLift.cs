using UnityEngine;

public class CraneLift : MonoBehaviour
{
    [SerializeField] Transform wallToMove;
    [SerializeField] float advanceSpeed;
    [SerializeField] float retreatSpeed;
    [SerializeField] Transform startingPosition;
    [SerializeField] Transform targetPosition;
    [SerializeField] BaseEnemy thisEnemy;
    bool inContact = false;
    LineRenderer lineRender;

    void Awake()
    {
        advanceSpeed *= PrefManager.GetDifficulty();
        lineRender = GetComponent<LineRenderer>();
        lineRender.startWidth = 0.1f;
        lineRender.endWidth = 0.1f;
        lineRender.positionCount = 2;
    }

    void Update()
    {
        if (inContact)
            wallToMove.transform.position = Vector3.MoveTowards(wallToMove.position, targetPosition.position, advanceSpeed*Time.deltaTime);
        else
            wallToMove.transform.position = Vector3.MoveTowards(wallToMove.position, startingPosition.position, retreatSpeed*Time.deltaTime);

        lineRender.SetPosition(0, thisEnemy.transform.position);
        lineRender.SetPosition(1, wallToMove.transform.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Player.instance.gameObject)
            inContact = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Player.instance.gameObject)
            inContact = false;
    }
}
