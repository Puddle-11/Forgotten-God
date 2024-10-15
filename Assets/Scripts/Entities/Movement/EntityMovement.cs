using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
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
    public List<Transform> nearbyObjects;
    [SerializeField] private float avoidanceFalloff  = 1;
    [SerializeField] private float avoidanceStrength = 1;
    private void Awake()
    {
        
        TryGetComponent(out Enman);
    }

    public virtual void Start()
    {
        if (defaultToPlayer && target == null)
        {
           target = GlobalManager.Player;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EntityManager _enManRef))
        {
            nearbyObjects.Add(collision.transform);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (nearbyObjects.Contains(collision.transform))
        {
            nearbyObjects.Remove(collision.transform);
        }
    }


    public GameObject GetTarget() {return target;}
    public virtual float CalculateSpeed()
    {
        if (Enman != null && Enman.isAlive())
        {
             return Vector2.Distance(transform.position, target.transform.position) > MinDist ? Mathf.MoveTowards(currentSpeed, moveSpeed, AccelerationSpeed) : Mathf.MoveTowards(currentSpeed, 0, AccelerationSpeed);
        }
        return 0;
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
                Vector2 dir = (target.transform.position - transform.position).normalized;


                TempPos = (Vector2)transform.position + (dir  ) * currentSpeed * Time.deltaTime;
                transform.position = new Vector3(TempPos.x, TempPos.y, transform.position.z);
            }
        }
    }

}
