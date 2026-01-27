using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [SerializeField] protected GameObject target;
    [SerializeField] protected bool defaultToPlayer = true;
    protected bool inRange;
    [SerializeField] private float detectionRange = 20;
    [Space]
    [Header("Movement")]
    [Space]
    [SerializeField] protected float MinDist = 10;
    [SerializeField] protected float moveSpeed = 10;
    [SerializeField] protected float AccelerationSpeed = 0.25f;
    protected float currentSpeed;
    protected EntityManager Enman;
    [SerializeField] private float avoidanceFalloff = 1;
    [SerializeField] private float avoidanceStrength = 1;

    [SerializeField] protected AnimationCurve knockbackCurve;
    protected bool inKnockback;

    protected Rigidbody2D RB;

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
        RB = GetComponent<Rigidbody2D>();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
       
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
    
    }


    public GameObject GetTarget() { return target; }
    public virtual float CalculateSpeed()
    {

        if (Enman != null && Enman.isAlive())
        {

            return (target != null && Vector2.Distance(transform.position, target.transform.position) > MinDist) ? Mathf.MoveTowards(currentSpeed, moveSpeed, AccelerationSpeed) : Mathf.MoveTowards(currentSpeed, 0, AccelerationSpeed);
        }
        return 0;
    }
    public virtual void Update()
    {
        if (!inKnockback && Enman.isAlive()) Move();
        inRange = Vector2.Distance(transform.position, target.transform.position) <= detectionRange;
    }
    public virtual void FixedUpdate()
    {
        currentSpeed = CalculateSpeed();
    }
    public virtual void Move()
    {
        if (target != null && moveSpeed > 0)
        {

            Vector2 TempPos = transform.position;
            Vector2 dir = (target.transform.position - transform.position).normalized;
            TempPos = (Vector2)transform.position + (dir) * currentSpeed * Time.deltaTime;
            transform.position = new Vector3(TempPos.x, TempPos.y, transform.position.z);
        }
    }
    public void StartKnockback(Vector2 _dir, float _amount, float _duration)
    {
    
        StartCoroutine(KnockbackRoutine(_dir, _amount, _duration));
    }
    public void StopVel()
    {
        RB.linearVelocity = Vector2.zero;
    }
    private IEnumerator KnockbackRoutine(Vector2 _dir, float _amount, float _duration)
    {
        if (inKnockback) yield break;
        inKnockback = true;
        StopVel();
        Vector3 originalPos = transform.position;
        _dir = _dir.normalized;
        float distancePerSecond = _amount / -_duration;
        float timer = 0;
        while (true)
        {
            if (!Enman.isAlive()) break;
            timer += Time.deltaTime;
            if (timer >= _duration) break;
            transform.position = originalPos + new Vector3(_dir.x, _dir.y, 0) * knockbackCurve.Evaluate(timer / _duration) * _amount;
            yield return null;
        }
        inKnockback = false;
    }

}
