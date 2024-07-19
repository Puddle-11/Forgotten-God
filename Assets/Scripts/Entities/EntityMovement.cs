using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public GameObject target;
    public SpriteRenderer Sprite;
    [SerializeField] private bool defaultToPlayer = true;
    [Space]
    [Header("Movement")]
    [Space]
    [SerializeField] protected float MinDist = 10;
    [SerializeField] protected float moveSpeed = 10; 
    protected float currentSpeed;
    [SerializeField] protected float AccelerationSpeed = 0.25f;
    protected EntityManager Enman;
    private void Awake()
    {
        TryGetComponent<EntityManager>(out Enman);

    }
    private void Start()
    {
        Debug.Log("start called");
        if (defaultToPlayer)
        {
           target = GlobalManager.Player;
        }
    }
    private void FixedUpdate()
    {
        currentSpeed = CalculateSpeed();
    }
    public virtual float CalculateSpeed()
    {
        if (Enman != null && Enman.isAlive())
        {
             return Vector2.Distance(transform.position, target.transform.position) > MinDist ? Mathf.MoveTowards(currentSpeed, moveSpeed, AccelerationSpeed) : Mathf.MoveTowards(currentSpeed, 0, AccelerationSpeed);
        }
        else
        {
            return 0;
        }
    }
    void Update()
    {
        Move();

    }
    public virtual void Move()
    {
        if (target != null)
        {
            Vector2 TempPos = transform.position;
            if (moveSpeed > 0)
            {

                TempPos = Vector2.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);
                transform.position = new Vector3(TempPos.x, TempPos.y, transform.position.z);

            }
        }
    }
}
