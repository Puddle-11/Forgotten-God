using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [SerializeField] protected GameObject target;
    [SerializeField] protected bool defaultToPlayer = true;
    [Space]
    [Header("Movement")]
    [Space]
    [SerializeField] protected float MinDist = 10;
    [SerializeField] protected float moveSpeed = 10; 
    [SerializeField] protected float AccelerationSpeed = 0.25f;
    protected float currentSpeed;
    protected EntityManager Enman;

    private void Awake()
    {
        
        TryGetComponent<EntityManager>(out Enman);
    }

    public virtual void Start()
    {
        if (defaultToPlayer && target == null)
        {
           target = GlobalManager.Player;
        }
    }
    public GameObject GetTarget()
    {
        return target;
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
    public virtual void Update()
    {
        Move();
    }
    public virtual void FixedUpdate()
    {
        currentSpeed = CalculateSpeed();

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
